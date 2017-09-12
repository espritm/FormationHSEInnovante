using Android.App;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using QuelleFormationSecurite.BusinessLayer;
using QuelleFormationSecurite.Droid.Activities;
using QuelleFormationSecurite.Droid.ViewHolder;
using QuelleFormationSecurite.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuelleFormationSecurite.Droid.Adapters
{
    public class RecyclerviewAdapter_Display_Results : RecyclerView.Adapter
    {
        Activity m_context;
        Android.Support.V4.App.Fragment m_Parent;
        List<TestResult> m_lsResults = new List<TestResult>();

        public RecyclerviewAdapter_Display_Results(Activity context, Android.Support.V4.App.Fragment parent)
        {
            m_context = context;
            m_Parent = parent;
        }

        public void UpdateListeMarches(List<TestResult> lsResults)
        {
            m_lsResults = lsResults;
            NotifyDataSetChanged();
        }

        public override int ItemCount
        {
            get { return m_lsResults == null ? 0 : m_lsResults.Count; }
        }

        private void OnCardClicked(int position)
        {
            TestResult resultClicked = null;
            try
            {
                resultClicked = m_lsResults[position];
            }
            catch (Exception)
            {
            }

            if (resultClicked != null)
            {
                //Go to Activity_Result
                Intent intent = new Intent(m_context, typeof(Activity_Result));
                intent.PutExtra("JsonResult", JSON.SerializeObject<TestResult>(resultClicked));
                m_context.StartActivity(intent);
            }
        }
        
        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ViewHolder_Display_TestResult viewHolder = (ViewHolder_Display_TestResult)holder;

            TestResult currentResult = m_lsResults[position];

            await SetViewAsync(currentResult, viewHolder);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = m_context.LayoutInflater.Inflate(Resource.Layout.Item_display_testResult, parent, false);

            ViewHolder_Display_TestResult viewHolder = new ViewHolder_Display_TestResult(itemView, OnCardClicked);

            return viewHolder;
        }

        private async Task SetViewAsync(TestResult currentResult, ViewHolder_Display_TestResult viewHolder)
        {
            await Task.Factory.StartNew(() =>
            {
                //Clean cardview and show progressbar
                if (m_context != null)
                {
                    m_context.RunOnUiThread(() =>
                    {
                        //TODO : Activate refresher and empty view
                        viewHolder.m_progressBar.Visibility = ViewStates.Visible;
                    });
                }

                //TODO : Get values to show..
                //Title
                string sTextviewTitleText = "Résultat du " + currentResult.m_dateAnswer.ToString("dd-MM-yyyy HH:mm");
                string sTextviewDescriptionText = currentResult.m_lsOutil.Count.ToString() + " outil" + (currentResult.m_lsOutil.Count > 1 ? "s" : "") + " de formation proposé" + (currentResult.m_lsOutil.Count > 1 ? "s" : "") + ".";

                if (m_context == null || !m_Parent.IsAdded)
                    return;

                //Update cardview with values
                m_context.RunOnUiThread(() =>
                {
                    //TODO : update view and dismiss refresher
                    viewHolder.m_textviewTitle.Text = sTextviewTitleText;
                    viewHolder.m_textviewDescription.Text = sTextviewDescriptionText;

                    viewHolder.m_progressBar.Visibility = ViewStates.Gone;
                });
            });
        }
    }
}