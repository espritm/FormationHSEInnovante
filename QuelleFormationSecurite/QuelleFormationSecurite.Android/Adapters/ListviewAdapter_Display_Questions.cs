using Android.App;
using Android.Widget;
using QuelleFormationSecurite.BusinessLayer;
using System.Collections.Generic;
using System;
using Android.Views;

namespace QuelleFormationSecurite.Droid.Adapters
{
    public class ListviewAdapter_Display_Questions : BaseAdapter<TestQuestion>
    {
        Activity m_context;
        List<TestQuestion> m_listQuestions;
        int m_iHeightOfitem;

        public event AnswerSelectedEventHandler AnswerSelected;

        public ListviewAdapter_Display_Questions(Activity context)
        {
            m_context = context;
            m_listQuestions = new List<TestQuestion>();
        }

        public void RefreshListQuestions(List<TestQuestion> listQuestions, int iHeightOfitem)
        {
            m_listQuestions = listQuestions;

            m_iHeightOfitem = iHeightOfitem;

            NotifyDataSetChanged();
        }

        public override TestQuestion this[int position]
        {
            get { return m_listQuestions[position]; }
        }

        public override int Count
        {
            get { return m_listQuestions.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = null;
            TestQuestion currentQuestion = null;
            LinearLayout layout = null;
            TextView textviewtitle = null;
            LinearLayout layoutAnswer = null;
            RadioGroup radiogroup = null;

            try
            {
                currentQuestion = this[position];
                view = m_context.LayoutInflater.Inflate(Resource.Layout.Item_Display_Questions, null);
                layout = view.FindViewById<LinearLayout>(Resource.Id.Item_Display_Questions_layout);
                textviewtitle = view.FindViewById<TextView>(Resource.Id.Item_Display_Questions_textview_title);
                layoutAnswer = view.FindViewById<LinearLayout>(Resource.Id.Item_Display_Questions_layout_answers);

                //Size of the item
                layout.SetMinimumHeight(m_iHeightOfitem);

                //Question title
                textviewtitle.Text = currentQuestion.m_sTitle;                

                //Init Radio Group if needed
                if (currentQuestion.m_AnswerType == EnumAnswerType.Radiobutton)
                {
                    radiogroup = new RadioGroup(m_context);
                    radiogroup.Orientation = Orientation.Vertical;
                }

                //Create programmatically answers' views
                foreach (QuestionAnswer answer in currentQuestion.m_lsAnswer)
                {
                    switch (currentQuestion.m_AnswerType)
                    {
                        case EnumAnswerType.Checkbox:
                            CheckBox checkbox = new CheckBox(m_context);
                            checkbox.Text = answer.m_sTitle;
                            checkbox.Checked = answer.m_bIsSelected;
                            checkbox.CheckedChange += (e, s) =>
                            {
                                answer.m_bIsSelected = checkbox.Checked;
                            };
                            checkbox.Click += (e, s) => 
                            {
                                this.AnswerSelected(s, new AnswerSelectedEventArgs { answerSelected = answer });
                            };

                            layoutAnswer.AddView(checkbox);
                            break;

                        case EnumAnswerType.Radiobutton:
                            RadioButton radiobutton = new RadioButton(m_context);
                            radiobutton.Text = answer.m_sTitle;
                            radiobutton.Checked = answer.m_bIsSelected;
                            radiobutton.CheckedChange += (e, s) =>
                            {
                                answer.m_bIsSelected = radiobutton.Checked;
                            };
                            radiobutton.Click += (e, s) =>
                            {
                                this.AnswerSelected(s, new AnswerSelectedEventArgs { answerSelected = answer });
                            };

                            radiogroup.AddView(radiobutton);
                            break;

                        case EnumAnswerType.Text:
                            //TODO later if needed
                            break;

                        case EnumAnswerType.Combolist:
                            //TODO later if needed
                            break;
                    }
                }

                //Finalize Radio Group if needed
                if (currentQuestion.m_AnswerType == EnumAnswerType.Radiobutton)
                    layoutAnswer.AddView(radiogroup);
            }
            catch (Exception)
            {
            }

            return view;
        }

        public class AnswerSelectedEventArgs : EventArgs
        {
            public QuestionAnswer answerSelected { get; set; }
        }

        public delegate void AnswerSelectedEventHandler(object sender, AnswerSelectedEventArgs args);
    }
}