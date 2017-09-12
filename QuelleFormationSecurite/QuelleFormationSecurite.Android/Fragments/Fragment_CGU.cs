using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Webkit;
using QuelleFormationSecurite.Droid.Utils;

namespace QuelleFormationSecurite.Droid.Fragments
{
    public class Fragment_CGU : Fragment
    {
        private WebView m_webview { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = null;

            try
            {
                view = inflater.Inflate(Resource.Layout.Fragment_CGU, null);

                m_webview = view.FindViewById<WebView>(Resource.Id.fragmentCGU_webview);

                m_webview.Settings.JavaScriptEnabled = true;
                m_webview.Settings.AllowFileAccessFromFileURLs = true;
                m_webview.Settings.AllowFileAccessFromFileURLs = true;
                m_webview.Settings.BuiltInZoomControls = false;
                m_webview.SetWebChromeClient(new WebChromeClient());
                m_webview.LoadUrl("file:///android_asset/pdfjs/web/viewer.html?file=///android_asset/CGU.pdf"); //https://github.com/mozilla/pdf.js/wiki/Viewer-options  :  + "#page=3" + "#nameddest=bookmarks" + "#zoom=40"
            }
            catch (System.Exception e)
            {
                if (view != null)
                    DynamicUIBuild_Utils.ShowSnackBar(Activity, view, Resource.String.unknownError, Snackbar.LengthLong);
            }

            return view;
        }
    }
}