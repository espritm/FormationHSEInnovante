using System;
using Android.Views;
using Android.Widget;
using QuelleFormationSecurite.BusinessLayer;
using Android.App;
using System.Collections.Generic;
using Android.Content;

namespace QuelleFormationSecurite.Droid.Adapters
{
    public class ListviewAdapter_Display_Outil : BaseAdapter<Outil>
    {
        Activity m_context;
        TestResult m_Result;

        public ListviewAdapter_Display_Outil(Activity context)
        {
            m_context = context;
        }

        public void UpdateListeOutils(TestResult result)
        {
            m_Result = result;
            
            NotifyDataSetChanged();
        }

        public override Outil this[int position]
        {
            get { return m_Result == null ? null : m_Result.m_lsOutil[position]; }
        }

        public override int Count
        {
            get { return m_Result == null ? 0 : m_Result.m_lsOutil.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = null;
            Outil currentOutil = null;
            TextView textviewTitle = null;
            TextView textviewObjectif = null;
            TextView textviewPourquoi = null;
            TextView textviewPublic = null;
            TextView textviewParticipants = null;
            TextView textviewEntreprise = null;
            TextView textviewTechnique = null;
            TextView textviewHumain = null;
            LinearLayout layoutAttention = null;
            TextView textviewAttention = null;
            TextView btnContactUs = null;

            try
            {
                currentOutil = this[position];
                view = m_context.LayoutInflater.Inflate(Resource.Layout.Item_Display_Outil, null);
                textviewTitle = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Title);
                textviewObjectif = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Objectif);
                textviewPourquoi = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Pourquoi);
                textviewPublic = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Public);
                textviewParticipants = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Participants);
                textviewEntreprise = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Entreprise);
                textviewTechnique = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Technique);
                textviewHumain = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Humain);
                layoutAttention = view.FindViewById<LinearLayout>(Resource.Id.Item_Display_Outil_Layout_Attention);
                textviewAttention = view.FindViewById<TextView>(Resource.Id.Item_Display_Outil_Textview_Attention);
                btnContactUs = view.FindViewById<Button>(Resource.Id.Item_Display_Outil_Button_ContactUs);

                textviewTitle.Text = currentOutil.m_sTitre;
                textviewObjectif.Text = currentOutil.m_sObjectif;
                textviewPourquoi.Text = currentOutil.m_sPourquoi;
                textviewPublic.Text = currentOutil.m_sPublic;
                textviewParticipants.Text = currentOutil.m_sParticipantDescription;
                textviewEntreprise.Text = currentOutil.m_sTypeEntreprise;
                textviewTechnique.Text = currentOutil.m_sMoyenTechniques;
                textviewHumain.Text = currentOutil.m_sMoyenHumain;

                if (currentOutil.m_sAttention == "")
                    layoutAttention.Visibility = ViewStates.Gone;
                else
                {
                    layoutAttention.Visibility = ViewStates.Visible;
                    textviewAttention.Text = currentOutil.m_sAttention;
                }

                btnContactUs.Click += (sender, args) =>
                {
                    Intent emailIntent = new Intent(Intent.ActionSendto);
                    emailIntent.SetType("text/plain");
                    emailIntent.SetData(Android.Net.Uri.Parse("mailto:camille.sartori@live.fr"));
                    emailIntent.PutExtra(Intent.ExtraSubject, "Formation HSE Innovante - Demande de scénario spécifique");
                    emailIntent.PutExtra(Intent.ExtraText, "Bonjour,\nJe souhaite un scénario personnalisé vis à vis de mes réponses ci-dessous svp :) \n\n" + m_Result.ToStringForEmail() + "\n\n" + currentOutil.ToString());

                    m_context.StartActivity(Intent.CreateChooser(emailIntent, "Demander un scénario personnalisé"));
                };
            }
            catch (Exception)
            {
            }

            return view;
        }
    }
}