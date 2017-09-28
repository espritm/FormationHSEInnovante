using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using QuelleFormationSecurite.BusinessLayer;
using QuelleFormationSecurite.Droid.Adapters;
using QuelleFormationSecurite.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuelleFormationSecurite.Droid.Activities
{
    [Activity(Icon = "@drawable/icon")]
    public class Activity_Result : AppCompatActivity
    {
        public TestResult m_Result;
        public ListView m_listviewResult;
        public SwipeRefreshLayout m_refresher;
        public TextView m_textviewEmptyList;
        public ListviewAdapter_Display_Outil m_adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string sJsonResult = Intent.Extras.GetString("JsonResult");
            m_Result = JSON.DeserializeObject<TestResult>(sJsonResult, "");

            Title = m_Result.m_dateAnswer.ToString("dd-MM-yyyy HH:mm");

            SetContentView(Resource.Layout.Activity_Result);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            m_listviewResult = FindViewById<ListView>(Resource.Id.Activity_Result_listivew);
            m_refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.Activity_Result_refresher);
            m_textviewEmptyList = FindViewById<TextView>(Resource.Id.Activity_Result_Textview_EmptyList);

            m_adapter = new ListviewAdapter_Display_Outil(this);
            m_listviewResult.Adapter = m_adapter;
            
            m_refresher.Post(async () =>
            {
                await InitResults();
            });
        }

        public async Task InitResults()
        {
            m_refresher.Enabled = false;

            m_adapter.UpdateListeOutils(m_Result);

            if (m_adapter.Count == 0)
                m_textviewEmptyList.Visibility = ViewStates.Visible;
            else
                m_textviewEmptyList.Visibility = ViewStates.Gone;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutString("JsonResult", JSON.SerializeObject<TestResult>(m_Result));
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);

            string sJsonResult = Intent.Extras.GetString("JsonResult");
            m_Result = JSON.DeserializeObject<TestResult>(sJsonResult, "");
        }
 
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
