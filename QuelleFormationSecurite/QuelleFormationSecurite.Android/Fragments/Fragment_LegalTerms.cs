using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using QuelleFormationSecurite.Droid.Utils;

namespace QuelleFormationSecurite.Droid.Fragments
{
    public class Fragment_LegalTerms : Fragment
    {
        TextView m_textviewDescEditor { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = null;

            try
            {
            //mon commentaire
                view = inflater.Inflate(Resource.Layout.Fragment_LegalTerms, null);

                m_textviewDescEditor = view.FindViewById<TextView>(Resource.Id.fragmentLegalTerms_textview_DescEditor);

                m_textviewDescEditor.MovementMethod = new Android.Text.Method.LinkMovementMethod();
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
