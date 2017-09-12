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

namespace QuelleFormationSecurite.Droid
{
#if DEBUG
    [Application(Theme = "@style/QuelleFormationSecurite", Debuggable = true)]
#else
    [Application(Theme = "@style/QuelleFormationSecurite", Debuggable = false)]
#endif
    public class QuelleFormationSecurite_App : Application
    {
        public QuelleFormationSecurite_App(IntPtr handle, global::Android.Runtime.JniHandleOwnership transer)
            : base(handle, transer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}