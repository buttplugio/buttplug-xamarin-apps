using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Security;

namespace ButtplugApp.Android.Utils
{
    public static class KeyStoreManager
    {
        internal static readonly char[] KeyStorePassword = null;//"123456789".ToCharArray();

        public static KeyStore GetKeyStore(Context context)
        {
            var keystoreFilename = context.GetString(Resource.String.keystore);
            var initialiseKeyStore = false;


            var keyStore = KeyStore.GetInstance("AndroidKeyStore");
            keyStore.Load(null, null);

            //try
            //{
            //    if (context.GetFileStreamPath(keystoreFilename)?.Exists() ?? false)
            //    {
            //        using (var s = context.OpenFileInput(keystoreFilename))
            //            keyStore.Load(s, KeyStorePassword);
            //    }
            //    else
            //    {
            //        keyStore.Load(null, KeyStorePassword);
            //        initialiseKeyStore = true;
            //    }
            //}
            //catch (Java.IO.IOException ex)
            //{
            //    if (ex.Message == "KeyStore integrity check failed.")
            //    {
            //        // Start from fresh
            //        keyStore.Load(null, KeyStorePassword);
            //        initialiseKeyStore = true;
            //    }
            //}

            //if (initialiseKeyStore)
            //    SaveKeyStore(context, keyStore);

            return keyStore;
        }

        public static void SaveKeyStore(Context context, KeyStore keyStore)
        {
            //var keystoreFilename = context.GetString(Resource.String.keystore);

            //using (var s = context.OpenFileOutput(keystoreFilename, FileCreationMode.Private))
            //    keyStore.Store(s, KeyStorePassword);
        }
    }
}