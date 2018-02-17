using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Android.App;
using Android.Content;
using Javax.Net.Ssl;
using Java.Security;
using Java.IO;
using Android.Security.Keystore;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Java.Security.Interfaces;
using Org.BouncyCastle.Crypto.Operators;
using Java.Security.Cert;
using Android.Runtime;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1;

namespace ButtplugApp.Android.Utils
{
    public class Cert
    {
        private static string privateKeyAlias = "WebSocketPrivateKey";

        public static SSLContext GetSSLContext(Context context)
        {
            var keystoreFilename = context.GetString(Resource.String.keystore);

            KeyManagerFactory keyManagerFactory = KeyManagerFactory.GetInstance("X509");
            TrustManagerFactory trustManagerFactory = TrustManagerFactory.GetInstance("X509");

            var keyStore = KeyStoreManager.GetKeyStore(context);

            if (!keyStore.ContainsAlias(privateKeyAlias))
            {
                CreatePrivateKey(keyStore);
                KeyStoreManager.SaveKeyStore(context, keyStore);
            }

            keyManagerFactory.Init(keyStore, KeyStoreManager.KeyStorePassword);
            trustManagerFactory.Init(keyStore);

            var sslContext = SSLContext.GetInstance("TLS");
            sslContext.Init(keyManagerFactory.GetKeyManagers(), trustManagerFactory.GetTrustManagers(), null);

            return sslContext;
        }

        private static void CreatePrivateKey(KeyStore keyStore)
        {
            var kpg = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmRsa);
            //kpg.Initialize(RSAKeyGenParameterSpec);
            kpg.Initialize(new KeyGenParameterSpec.Builder(privateKeyAlias, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt | KeyStorePurpose.Sign | KeyStorePurpose.Verify)
               .SetDigests(KeyProperties.DigestNone, KeyProperties.DigestSha256, KeyProperties.DigestSha512)
               .SetEncryptionPaddings(KeyProperties.EncryptionPaddingRsaPkcs1)
               .SetSignaturePaddings(KeyProperties.SignaturePaddingRsaPkcs1)
               .SetUserAuthenticationRequired(false)
               .Build());

            KeyPair keyPair = kpg.GenerateKeyPair();

            var subjectName = "localhost";

            var rng = new Java.Security.SecureRandom();

            // The Certificate Generator
            var certificateGenerator = new X509V3CertificateGenerator();
            certificateGenerator.AddExtension(X509Extensions.ExtendedKeyUsage.Id, true, new ExtendedKeyUsage(KeyPurposeID.IdKPServerAuth));

            // Serial Number
            certificateGenerator.SetSerialNumber(BigInteger.ValueOf(rng.NextLong() & long.MaxValue));

            // Issuer and Subject Name
            certificateGenerator.SetIssuerDN(new X509Name("CN=" + subjectName));
            certificateGenerator.SetSubjectDN(new X509Name("CN=" + subjectName));

            // Valid For
            DateTime notBefore = DateTime.UtcNow.Date.AddDays(-1);
            DateTime notAfter = notBefore.AddYears(2);

            certificateGenerator.SetNotBefore(notBefore);
            certificateGenerator.SetNotAfter(notAfter);


            certificateGenerator.SetPublicKey(new RsaKeyParameters(false, new BigInteger(keyPair.Public.JavaCast<IRSAPublicKey>().Modulus.ToByteArray()), new BigInteger(keyPair.Public.JavaCast<IRSAPublicKey>().PublicExponent.ToByteArray())));

            // selfsign certificate
            var certificate = certificateGenerator.Generate(new BCSignatureFactory(keyPair.Private));

            var certChain = new List<Certificate>();

            var cg = CertificateFactory.GetInstance("X.509");

            using (var s = new MemoryStream(certificate.GetEncoded()))
                certChain.Add(cg.GenerateCertificate(s));

            System.Diagnostics.Debug.WriteLine(certChain.First().ToString());
            
            //var keyStoreProtection = new KeyStore.PasswordProtection(KeyStorePassword);

            var myPrivateKey = new KeyStore.PrivateKeyEntry(keyPair.Private, certChain.ToArray());

            keyStore.SetEntry(privateKeyAlias, myPrivateKey, null);

            keyStore.SetCertificateEntry(privateKeyAlias, certChain.First());
        }

        private class BCSignatureFactory : ISignatureFactory, IStreamCalculator
        {
            private readonly IPrivateKey _privateKey;
            private readonly AlgorithmIdentifier _algID;
            private readonly Signature _signature;
            private MemoryStream _stream;

            public BCSignatureFactory(IPrivateKey privateKey)
            {
                _privateKey = privateKey;
                _algID = new AlgorithmIdentifier(PkcsObjectIdentifiers.Sha256WithRsaEncryption);

                _signature = Signature.GetInstance("SHA256withRSA");
                _signature.InitSign(_privateKey);

                _stream = new MemoryStream();
            }
            

            public object AlgorithmDetails => _algID;

            public IStreamCalculator CreateCalculator()
            {
                return this;
            }

            public Stream Stream => _stream;

            public object GetResult()
            {
                _stream.Flush();
                _signature.Update(_stream.ToArray());
                _stream.Dispose();
                _stream = null;

                return new StupidShit(_signature.Sign());
            }

            private class StupidShit : IBlockResult
            {
                private readonly byte[] result;

                public StupidShit(byte[] result)
                {
                    this.result = result;
                }

                public byte[] Collect()
                {
                    return result;
                }

                public int Collect(byte[] destination, int offset)
                {
                    Array.Copy(result, 0, destination, offset, result.Length);

                    return result.Length;
                }
            }
        }
    }

}