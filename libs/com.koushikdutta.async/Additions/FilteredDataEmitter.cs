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

namespace Com.Koushikdutta.Async
{
    public partial class FilteredDataEmitter
    {
        void IDataTrackingEmitter.SetDataEmitter(global::Com.Koushikdutta.Async.IDataEmitter p0)
            => DataEmitter = p0;
    }
}