using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using QuelleFormationSecurite.BusinessLayer;
using QuelleFormationSecurite.Droid.Adapters;
using QuelleFormationSecurite.Droid.Utils;
using QuelleFormationSecurite.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuelleFormationSecurite.Droid.Fragments
{
    public class Fragment_List_Results : Fragment
    {
        private SwipeRefreshLayout m_refresher;
        private RecyclerView m_recyclerView;
        private TextView m_textviewEmptyList;
        private RecyclerviewAdapter_Display_Results m_adapter;
        public int m_iCount;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = null;

            try
            {
                view = inflater.Inflate(Resource.Layout.Fragment_List_Results, null);

                m_refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.fragmentListResult_refresher);
                m_recyclerView = view.FindViewById<RecyclerView>(Resource.Id.fragmentListResult_recyclerView);
                m_textviewEmptyList = view.FindViewById<TextView>(Resource.Id.fragmentListResult_textview_emptyList);

                //TestResults list
                m_recyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
                m_adapter = new RecyclerviewAdapter_Display_Results(Activity, this);
                m_recyclerView.SetAdapter(m_adapter);

                m_refresher.Refresh += M_refresher_Refresh;

                m_refresher.Post(async () =>
                {
                    m_refresher.Refreshing = true;

                    await RefreshTestResultList();
                });
            }
            catch (System.Exception e)
            {
                if (view != null)
                    DynamicUIBuild_Utils.ShowSnackBar(Activity, view, Resource.String.unknownError, Snackbar.LengthLong);
            }

            return view;
        }

        private async void M_refresher_Refresh(object sender, System.EventArgs e)
        {
            await RefreshTestResultList();
        }

        public async Task RefreshTestResultList()
        {
            m_refresher.Refreshing = true;

            //TODO : get the list in the database
            List<TestResult> lsResultsToShow  = JSON.DeserializeObject<List<TestResult>>(Settings.lsJsonResults, "");

            if (lsResultsToShow == null)
                lsResultsToShow = new List<TestResult>();

            m_adapter.UpdateListeMarches(lsResultsToShow);

            if (lsResultsToShow.Count == 0)
                m_textviewEmptyList.Visibility = ViewStates.Visible;
            else
                m_textviewEmptyList.Visibility = ViewStates.Gone;

            m_refresher.Refreshing = false;
        }
    }
}