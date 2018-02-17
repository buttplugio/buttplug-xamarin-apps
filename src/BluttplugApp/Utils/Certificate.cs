using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System.Linq;

namespace ButtplugApp.Utils
{
    public static class Certificate
    {
        public const int KeyLength = 2048;

        // Note: Much of this code comes from https://stackoverflow.com/a/22247129
        public static X509Certificate2 GenerateSelfSignedCertificate(string subject, IEnumerable<Uri> alternativeNames = default(IEnumerable<Uri>))
        {
            // The Certificate Generator
            var certificateGenerator = new X509V3CertificateGenerator();

            // Generating Random Numbers
            var randomGenerator = new CryptoApiRandomGenerator();
            var random = new SecureRandom(randomGenerator);

            // Subject Public Key
            var keyGenerationParameters = new KeyGenerationParameters(random, KeyLength);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            var issuerKeyPair = keyPairGenerator.GenerateKeyPair();

            // Serial Number
            certificateGenerator.SetSerialNumber(BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(long.MaxValue), random));

            // Issuer
            certificateGenerator.SetIssuerDN(new X509Name("CN=" + subject));
            certificateGenerator.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(issuerKeyPair.Public)));

            // Subject DN
            certificateGenerator.SetSubjectDN(new X509Name("CN=" + subject));

            // Subject Alternative Name
            var subjectAlternativeNames = new List<Asn1Encodable>();

            foreach(var alternativeName in alternativeNames)
            {
                subjectAlternativeNames.Add(new GeneralName(
                    (alternativeName.HostNameType == UriHostNameType.Dns || alternativeName.HostNameType == UriHostNameType.Basic)
                        ? GeneralName.DnsName
                        : GeneralName.IPAddress,
                    alternativeName.Host
                    ));
            };

            certificateGenerator.AddExtension(X509Extensions.SubjectAlternativeName.Id, false, new DerSequence(subjectAlternativeNames.ToArray()));

            // Valid For
            var notBefore = DateTime.UtcNow.Date;
            var notAfter = notBefore.AddYears(2);
            certificateGenerator.SetNotBefore(notBefore);
            certificateGenerator.SetNotAfter(notAfter);

            var subjectKeyPair = keyPairGenerator.GenerateKeyPair();
            certificateGenerator.SetPublicKey(subjectKeyPair.Public);
            certificateGenerator.AddExtension(X509Extensions.SubjectKeyIdentifier.Id, false, new SubjectKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(subjectKeyPair.Public)));

            // Add basic constraint
            certificateGenerator.AddExtension(X509Extensions.BasicConstraints.Id, true, new BasicConstraints(false));

            certificateGenerator.AddExtension(X509Extensions.ExtendedKeyUsage.Id, false, new ExtendedKeyUsage(new[] { KeyPurposeID.IdKPServerAuth }));

            // Signature Algorithm
            const string signatureAlgorithm = "SHA256WithRSA";
            var signatureFactory = new Asn1SignatureFactory(signatureAlgorithm, issuerKeyPair.Private);

            // selfsign certificate
            var certificate = certificateGenerator.Generate(signatureFactory);

            // correcponding private key
            var info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(subjectKeyPair.Private);

            // merge into X509Certificate2
            var x509 = new X509Certificate2(certificate.GetEncoded());
            var seq = (Asn1Sequence)Asn1Object.FromByteArray(info.ParsePrivateKey().GetDerEncoded());
            if (seq.Count != 9)
            {
                // throw new PemException("malformed sequence in RSA private key");
            }

            var rsa = RsaPrivateKeyStructure.GetInstance(seq);
            var rsaparams = new RsaPrivateCrtKeyParameters(
                rsa.Modulus, rsa.PublicExponent, rsa.PrivateExponent, rsa.Prime1, rsa.Prime2, rsa.Exponent1, rsa.Exponent2, rsa.Coefficient);
            x509.PrivateKey = ToDotNetKey(rsaparams); // x509.PrivateKey = DotNetUtilities.ToRSA(rsaparams);
            return x509;
        }

        private static AsymmetricAlgorithm ToDotNetKey(RsaPrivateCrtKeyParameters privateKey)
        {
            var cspParams = new CspParameters
            {
                KeyContainerName = Guid.NewGuid().ToString(),
                KeyNumber = (int)KeyNumber.Exchange,
                Flags = CspProviderFlags.NoFlags,
            };
            var rsaProvider = new RSACryptoServiceProvider(cspParams);
            var parameters = new RSAParameters
            {
                Modulus = privateKey.Modulus.ToByteArrayUnsigned(),
                P = privateKey.P.ToByteArrayUnsigned(),
                Q = privateKey.Q.ToByteArrayUnsigned(),
                DP = privateKey.DP.ToByteArrayUnsigned(),
                DQ = privateKey.DQ.ToByteArrayUnsigned(),
                InverseQ = privateKey.QInv.ToByteArrayUnsigned(),
                D = privateKey.Exponent.ToByteArrayUnsigned(),
                Exponent = privateKey.PublicExponent.ToByteArrayUnsigned(),
            };
            rsaProvider.ImportParameters(parameters);
            return rsaProvider;
        }

        public static X509Certificate2 GetCert(string app, string hostname = "localhost")
        {
            var appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), app);
            var certPfx = Path.Combine(appPath, "cert.pfx");
            if (!File.Exists(certPfx))
            {
                var clientCert = GenerateSelfSignedCertificate(hostname, new []{
                    new Uri($"//{hostname}"),
                    new Uri($"//{Environment.MachineName}"),
                    new Uri($"//127.0.0.1")
                });
                var p12cert = clientCert.Export(X509ContentType.Pfx);

                Directory.CreateDirectory(appPath);

                var w = File.OpenWrite(certPfx);
                w.Write(p12cert, 0, p12cert.Length);
                w.Close();
            }

            return new X509Certificate2(File.ReadAllBytes(certPfx), (string)null, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
        }
    }
}