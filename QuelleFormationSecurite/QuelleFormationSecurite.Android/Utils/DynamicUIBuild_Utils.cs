
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Views.InputMethods;

namespace QuelleFormationSecurite.Droid.Utils
{
    public static class DynamicUIBuild_Utils
    {
        public static void HideKeyboard(Context context, View v)
        {
            InputMethodManager imm = (InputMethodManager)context.GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(v.WindowToken, 0);
        }

        internal static void ShowSnackBar(Context context, View view, string sText, int iSnackbarLength)
        {
            Snackbar bar = Snackbar.Make(view, sText, iSnackbarLength);

            bar.View.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(context, Resource.Color.primary_dark)));

            bar.Show();
        }

        internal static void ShowSnackBar(Context context, View view, int iTextResourceID, int iSnackbarLength)
        {
            Snackbar bar = Snackbar.Make(view, iTextResourceID, iSnackbarLength);

            bar.View.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(context, Resource.Color.primary_dark)));

            bar.Show();
        }

        internal static void ShowSnackBar_WithOKButtonToClose(Context context, View view, string sText)
        {
            Snackbar bar = Snackbar.Make(view, sText, Snackbar.LengthIndefinite);

            bar.View.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(context, Resource.Color.primary_dark)));

            bar.SetAction("OK", (v) => { });
            bar.SetActionTextColor(new Android.Graphics.Color(ContextCompat.GetColor(context, Resource.Color.primary_light)));

            bar.Show();
        }

        internal static void ShowSnackBar_WithOKButtonToClose(Context context, View view, int iTextResourceID)
        {
            Snackbar bar = Snackbar.Make(view, iTextResourceID, Snackbar.LengthIndefinite);

            bar.View.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(context, Resource.Color.primary_dark)));

            bar.SetAction("OK", (v) => { });
            bar.SetActionTextColor(new Android.Graphics.Color(ContextCompat.GetColor(context, Resource.Color.primary_light)));

            bar.Show();
        }
    }
}