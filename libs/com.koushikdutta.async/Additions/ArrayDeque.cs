using System;
using Android.Runtime;

namespace Com.Koushikdutta.Async
{
    public partial class ArrayDeque
    {
        // Explicit proxy method because there's a nexted class also named DescendingIterator
        unsafe global::Java.Util.IIterator IDeque.DescendingIterator()
        => InvokeDescendingIterator();

        public partial class DeqIterator
        {
            static Delegate cb_next;
#pragma warning disable 0169
            static Delegate GetNextHandler()
            {
                if (cb_next == null)
                    cb_next = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr>)n_Next);
                return cb_next;
            }

            static IntPtr n_Next(IntPtr jnienv, IntPtr native__this)
            {
                global::Com.Koushikdutta.Async.ArrayDeque.DeqIterator __this = global::Java.Lang.Object.GetObject<global::Com.Koushikdutta.Async.ArrayDeque.DeqIterator>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
                return JNIEnv.ToLocalJniHandle(__this.Next());
            }
#pragma warning restore 0169

            static IntPtr id_next;
            // Metadata.xml XPath method reference: path="/api/package[@name='com.koushikdutta.async']/class[@name='ArrayDeque']/method[@name='peek' and count(parameter)=0]"
            [Register("next", "()Ljava/lang/Object;", "GetNextHandler")]
            public virtual unsafe global::Java.Lang.Object Next()
            {
                if (id_next == IntPtr.Zero)
                    id_next = JNIEnv.GetMethodID(class_ref, "next", "()Ljava/lang/Object;");
                try
                {

                    if (((object)this).GetType() == ThresholdType)
                        return (Java.Lang.Object)global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(JNIEnv.CallObjectMethod(((global::Java.Lang.Object)this).Handle, id_next), JniHandleOwnership.TransferLocalRef);
                    else
                        return (Java.Lang.Object)global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(JNIEnv.CallNonvirtualObjectMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "next", "()Ljava/lang/Object;")), JniHandleOwnership.TransferLocalRef);
                }
                finally
                {
                }
            }
        }

        
        public partial class DescendingIterator
        {
            static Delegate cb_next;
#pragma warning disable 0169
            static Delegate GetNextHandler()
            {
                if (cb_next == null)
                    cb_next = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr>)n_Next);
                return cb_next;
            }

            static IntPtr n_Next(IntPtr jnienv, IntPtr native__this)
            {
                global::Com.Koushikdutta.Async.ArrayDeque.DescendingIterator __this = global::Java.Lang.Object.GetObject<global::Com.Koushikdutta.Async.ArrayDeque.DescendingIterator>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
                return JNIEnv.ToLocalJniHandle(__this.Next());
            }
#pragma warning restore 0169

            static IntPtr id_next;
            // Metadata.xml XPath method reference: path="/api/package[@name='com.koushikdutta.async']/class[@name='ArrayDeque']/method[@name='peek' and count(parameter)=0]"
            [Register("next", "()Ljava/lang/Object;", "GetNextHandler")]
            public virtual unsafe global::Java.Lang.Object Next()
            {
                if (id_next == IntPtr.Zero)
                    id_next = JNIEnv.GetMethodID(class_ref, "next", "()Ljava/lang/Object;");
                try
                {

                    if (((object)this).GetType() == ThresholdType)
                        return (Java.Lang.Object)global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(JNIEnv.CallObjectMethod(((global::Java.Lang.Object)this).Handle, id_next), JniHandleOwnership.TransferLocalRef);
                    else
                        return (Java.Lang.Object)global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(JNIEnv.CallNonvirtualObjectMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "next", "()Ljava/lang/Object;")), JniHandleOwnership.TransferLocalRef);
                }
                finally
                {
                }
            }
        }
    }
}