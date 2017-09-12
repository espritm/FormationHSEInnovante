using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;

namespace QuelleFormationSecurite.Droid.ViewHolder
{
    public class ViewHolder_Display_TestResult : RecyclerView.ViewHolder
    {
        public CardView m_cardview { get; set; }
        public ProgressBar m_progressBar { get; set; }
        public TextView m_textviewTitle { get; set; }
        public TextView m_textviewDescription { get; set; }

        public ViewHolder_Display_TestResult(View itemView, Action<int> onCardClickListener)
            :base (itemView)
        {
            m_cardview = itemView.FindViewById<CardView>(Resource.Id.item_display_testResult_cardview);
            m_progressBar = itemView.FindViewById<ProgressBar>(Resource.Id.item_display_testResult_progressbar); 
            m_textviewTitle = itemView.FindViewById<TextView>(Resource.Id.item_display_testResult_textview_title);
            m_textviewDescription = itemView.FindViewById<TextView>(Resource.Id.item_display_testResult_textview_description);
            //TODO : compléter avec les éléments graphiques supplémentaires

            m_cardview.Click += (sender, e) => onCardClickListener(base.AdapterPosition);
        }
    }
}