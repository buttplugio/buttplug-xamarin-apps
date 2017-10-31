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

namespace Com.Koushikdutta.Async.Http
{
    public partial class AsyncHttpClient
    {
        public abstract partial class RequestCallbackBase
        {
            static Delegate cb_onCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_;
#pragma warning disable 0169
            static Delegate GetOnCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_Handler()
            {
                if (cb_onCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_ == null)
                    cb_onCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_ = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr>)n_OnCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_);
                return cb_onCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_;
            }

            static void n_OnCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_(IntPtr jnienv, IntPtr native__this, IntPtr native_p0, IntPtr native_p1, IntPtr native_p2)
            {
                global::Com.Koushikdutta.Async.Http.AsyncHttpClient.RequestCallbackBase __this = global::Java.Lang.Object.GetObject<global::Com.Koushikdutta.Async.Http.AsyncHttpClient.RequestCallbackBase>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
                global::Java.Lang.Exception p0 = (global::Java.Lang.Exception)global::Java.Lang.Object.GetObject<global::Java.Lang.Exception>(native_p0, JniHandleOwnership.DoNotTransfer);
                global::Java.Lang.Object p1 = (global::Java.Lang.Object)global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(native_p1, JniHandleOwnership.DoNotTransfer);
                global::Java.Lang.Object p2 = (global::Java.Lang.Object)global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(native_p2, JniHandleOwnership.DoNotTransfer);
                __this.OnCompleted(p0,p1,p2);
            }
#pragma warning restore 0169

            static IntPtr id_onCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_;
            // Metadata.xml XPath method reference: path="/api/package[@name='com.koushikdutta.async.http']/class[@name='AsyncHttpClient.RequestCallbackBase']/method[@name='onCompleted' and count(parameter)=1 and parameter[1][@type='com.koushikdutta.async.http.AsyncHttpResponse']]"
            [Register("onCompleted", "(Lcom/koushikdutta/async/http/AsyncHttpResponse;)V", "GetOnCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_Handler")]
            
            public virtual unsafe void OnCompleted(global::Java.Lang.Exception p0, global::Java.Lang.Object p1, global::Java.Lang.Object p2)
            {
                if (id_onCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_ == IntPtr.Zero)
                    id_onCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_ = JNIEnv.GetMethodID(class_ref, "onCompleted", "(Lcom/koushikdutta/async/http/AsyncHttpResponse;)V");
                try
                {
                    JValue* __args = stackalloc JValue[3];
                    __args[0] = new JValue(p0);
                    __args[1] = new JValue(p1);
                    __args[2] = new JValue(p2);

                    if (((object)this).GetType() == ThresholdType)
                        JNIEnv.CallVoidMethod(((global::Java.Lang.Object)this).Handle, id_onCompleted_Lcom_koushikdutta_async_http_AsyncHttpResponse_, __args);
                    else
                        JNIEnv.CallNonvirtualVoidMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "onCompleted", "(Lcom/koushikdutta/async/http/AsyncHttpResponse;)V"), __args);
                }
                finally
                {
                }
            }
        }
    }
}