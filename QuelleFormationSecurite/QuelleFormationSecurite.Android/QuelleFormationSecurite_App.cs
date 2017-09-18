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
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

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

            MobileCenter.Start("a78ae0b8-b0e3-4f66-ba05-674ef185c031", typeof(Analytics), typeof(Crashes));

            Analytics.SetEnabledAsync(true);
            Crashes.SetEnabledAsync(true);
        }
    }
}