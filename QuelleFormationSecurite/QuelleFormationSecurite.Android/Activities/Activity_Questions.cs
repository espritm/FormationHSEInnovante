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
    public class Activity_Questions : AppCompatActivity
    {
        public List<TestQuestion> m_lsQuestions;
        public List<Outil> m_lsOutil;
        public TestResult m_Result;
        public ListView m_listviewQuestions;
        public SwipeRefreshLayout m_refresher;
        public Button m_btnValid;
        public ListviewAdapter_Display_Questions m_adapter;
        private int m_iCurrentPosition = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Title = "Question";

            SetContentView(Resource.Layout.Activity_Questions);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            m_listviewQuestions = FindViewById<ListView>(Resource.Id.Activity_Questions_listivew);
            m_refresher = FindViewById<SwipeRefreshLayout>(Resource.Id.Activity_Questions_refresher);
            m_btnValid = FindViewById<Button>(Resource.Id.Activity_Questions_button);

            m_adapter = new ListviewAdapter_Display_Questions(this);
            m_adapter.AnswerSelected += M_adapter_AnswerSelected;

            m_listviewQuestions.Adapter = m_adapter;
            m_listviewQuestions.Divider = null;
            m_listviewQuestions.DividerHeight = 0;
            m_listviewQuestions.Enabled = false;
            m_listviewQuestions.Scroll += M_listviewQuestions_Scroll;

            m_btnValid.Visibility = ViewStates.Gone;
            m_btnValid.Click += M_btnValid_Click;

            m_refresher.Post(async () =>
            {
                await InitQuestions();
            });
        }

        private void M_listviewQuestions_Scroll(object sender, AbsListView.ScrollEventArgs e)
        {
            UpdateButtonValidVisibility();
        }

        public void UpdateButtonValidVisibility()
        {
            if (m_lsQuestions == null)
                return;

            TestQuestion currentQuestion = m_lsQuestions[m_iCurrentPosition];

            //Update button title
            if (m_iCurrentPosition + 1 < m_adapter.Count)
            {
                m_btnValid.Text = Resources.GetString(Resource.String.Activity_Questions_buttonNext);
            }
            else
            {
                m_btnValid.Text = Resources.GetString(Resource.String.Activity_Questions_buttonValid);
                m_btnValid.Visibility = ViewStates.Visible;
            }

            if (currentQuestion.IsAnswered())
                m_btnValid.Visibility = ViewStates.Visible;
            else
                m_btnValid.Visibility = ViewStates.Gone;
        }

        private void M_btnValid_Click(object sender, System.EventArgs e)
        {
            if (m_btnValid.Text == Resources.GetString(Resource.String.Activity_Questions_buttonValid))
                ComputeAnswerAndFinishActivity();
            else
            {
                m_iCurrentPosition++;
                ScrollListviewToCurrentPosition(true, true);
            }
        }
        
        private async Task ComputeAnswerAndFinishActivity()
        {
            await InitOutils();

            m_Result = new TestResult();

            //Outil 1 : 
            /*
             * question 1 : "Entre 1 et 5"
             * question 4 : "Un animateur de formation", 
             */

            //Outil 2 : 
            /*
             * question 1 : "Entre 1 et 5" ou "Entre 6 et 10"
             * question 4 : "Un animateur de formation" et "Une enveloppe budg�taire pour d'�ventuels �l�ments de jeux (t�l�phones plastiques, etc...)"
             */

            //Outil 3 : 
            /*
             * question 1 : "Entre 1 et 5" ou "Entre 6 et 10"
             * question 4 : "Un animateur de formation" et 
             *      ( "Une enveloppe budg�taire pour d'�ventuels �l�ments de jeux (t�l�phones plastiques, etc...)" ou 
             *        "Une ou plusieurs tablettes" ou 
             *        "Les personnes form�es disposent de smartphone" )
             */

            //Outil 4 : 
            /*
             * question 4 : "Un ordinateur et un vid�o-projecteur"
             */

            TestQuestion q1 = m_lsQuestions.Find(q => q.m_iID == 1);
            TestQuestion q4 = m_lsQuestions.Find(q => q.m_iID == 4);

            //Outil 1
            if (q1.m_lsAnswer.Find(a => a.m_sTitle == "Entre 1 et 5").m_bIsSelected &&
                q4.m_lsAnswer.Find(a => a.m_sTitle == "Un animateur de formation").m_bIsSelected)
            {
                m_Result.m_lsOutil.Add(m_lsOutil.Find(o => o.m_iID == 1));
            }

            //Outil 2
            if ((q1.m_lsAnswer.Find(a => a.m_sTitle == "Entre 1 et 5").m_bIsSelected || q1.m_lsAnswer.Find(a => a.m_sTitle == "Entre 6 et 10").m_bIsSelected) &&
                q4.m_lsAnswer.Find(a => a.m_sTitle == "Un animateur de formation").m_bIsSelected &&
                q4.m_lsAnswer.Find(a => a.m_sTitle == "Une enveloppe budg�taire pour d'�ventuels �l�ments de jeux (t�l�phones plastiques, etc...)").m_bIsSelected)
            {
                m_Result.m_lsOutil.Add(m_lsOutil.Find(o => o.m_iID == 2));
            }

            //Outil 3
            if ((q1.m_lsAnswer.Find(a => a.m_sTitle == "Entre 1 et 5").m_bIsSelected || q1.m_lsAnswer.Find(a => a.m_sTitle == "Entre 6 et 10").m_bIsSelected) &&
                q4.m_lsAnswer.Find(a => a.m_sTitle == "Un animateur de formation").m_bIsSelected &&
                ( q4.m_lsAnswer.Find(a => a.m_sTitle == "Une enveloppe budg�taire pour d'�ventuels �l�ments de jeux (t�l�phones plastiques, etc...)").m_bIsSelected ||
                  q4.m_lsAnswer.Find(a => a.m_sTitle == "Une ou plusieurs tablette(s)").m_bIsSelected ||
                  q4.m_lsAnswer.Find(a => a.m_sTitle == "Les personnes form�es disposent de smartphone").m_bIsSelected))
            {
                m_Result.m_lsOutil.Add(m_lsOutil.Find(o => o.m_iID == 3));
            }

            //Outil 4
            if (q4.m_lsAnswer.Find(a => a.m_sTitle == "Un ordinateur et un vid�o-projecteur").m_bIsSelected)
            {
                m_Result.m_lsOutil.Add(m_lsOutil.Find(o => o.m_iID == 4));
            }

            //V�rifie qu'un moins 1 outil est propos�
            if (m_Result.m_lsOutil.Count == 0)
            {
                Android.Support.V7.App.AlertDialog dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                   .SetTitle("Aucun outil de formation ne correspond")
                   .SetMessage("Au vu de vos r�ponses, le plus simple serait de vous munir d'un vid�o-projecteur.\nCochez la case \"vid�o - projecteur\" � la question 4 !")
                   .SetPositiveButton("OK", delegate { })
                   .SetIcon(Resource.Drawable.Icon)
                   .SetCancelable(true)
                   .Show();

                return;
            }

            //Record the answers into the result object in order to get them in the email
            foreach (TestQuestion q in m_lsQuestions)
                foreach (QuestionAnswer a in q.m_lsAnswer)
                    if (a.m_bIsSelected)
                        m_Result.m_dicQuestionReponse.Add(a.m_sTitle, q.m_sTitle);

            //Enregistre dans les settings
            List<TestResult> lsResultsSettings = JSON.DeserializeObject<List<TestResult>>(Settings.lsJsonResults, "");
            if (lsResultsSettings == null)
                lsResultsSettings = new List<TestResult>();

            lsResultsSettings.Add(m_Result);
            Settings.lsJsonResults = JSON.SerializeObject<List<TestResult>>(lsResultsSettings);

            this.Finish();
        }

        private async Task InitQuestions()
        {
            m_refresher.Refreshing = true;

            m_lsQuestions = new List<TestQuestion>();

            TestQuestion q1 = new TestQuestion(1, "Quel est le nombre de personnes � former ?", EnumAnswerType.Radiobutton);
            q1.AddAnswer(new QuestionAnswer("Entre 1 et 5"));
            q1.AddAnswer(new QuestionAnswer("Entre 6 et 10"));
            q1.AddAnswer(new QuestionAnswer("Plus de 11"));
            m_lsQuestions.Add(q1);

            TestQuestion q2 = new TestQuestion(2, "Quelle est la moyenne d'�ge des personnes � former ?", EnumAnswerType.Radiobutton);
            q2.AddAnswer(new QuestionAnswer("Entre 14 et 18 ans"));
            q2.AddAnswer(new QuestionAnswer("Entre 19 et 28 ans"));
            q2.AddAnswer(new QuestionAnswer("Entre 29 et 39 ans"));
            q2.AddAnswer(new QuestionAnswer("Plus de 39 ans"));
            m_lsQuestions.Add(q2);

            TestQuestion q3 = new TestQuestion(3, "Les personnes � former ont-elles d�j� travaill� dans un environnement similaire � votre entreprise ?", EnumAnswerType.Radiobutton);
            q3.AddAnswer(new QuestionAnswer("Non, jamais"));
            q3.AddAnswer(new QuestionAnswer("Oui, moins de 6 mois"));
            q3.AddAnswer(new QuestionAnswer("Oui, plus de 6 mois"));
            q3.AddAnswer(new QuestionAnswer("C'est mitig� : certaines personnes oui, d'autres non"));
            m_lsQuestions.Add(q3);

            TestQuestion q4 = new TestQuestion(4, "Quels sont les moyens � votre disposition pour la formation ?", EnumAnswerType.Checkbox);
            q4.AddAnswer(new QuestionAnswer("Un animateur de formation"));
            q4.AddAnswer(new QuestionAnswer("Une salle de formation"));
            q4.AddAnswer(new QuestionAnswer("Un ordinateur et un vid�o-projecteur"));
            q4.AddAnswer(new QuestionAnswer("Un tableau blanc"));
            q4.AddAnswer(new QuestionAnswer("Une ou plusieurs tablette(s)"));
            q4.AddAnswer(new QuestionAnswer("Une ou plusieurs lunette(s) de r�alit� virtuelle (VR)"));
            q4.AddAnswer(new QuestionAnswer("Une enveloppe budg�taire pour d'�ventuels �l�ments de jeux (t�l�phones plastiques, etc...)"));
            q4.AddAnswer(new QuestionAnswer("Les personnes form�es disposent de smartphone"));
            m_lsQuestions.Add(q4);

            TestQuestion q5 = new TestQuestion(5, "Dans quels milieux vont travailler les personnes � former ?", EnumAnswerType.Checkbox);
            q5.AddAnswer(new QuestionAnswer("Bureaux (terci�re)"));
            q5.AddAnswer(new QuestionAnswer("Ateliers (production)"));
            q5.AddAnswer(new QuestionAnswer("Logistique (transport & logistique)"));
            q5.AddAnswer(new QuestionAnswer("Chantier (BTP ou autre)"));
            q5.AddAnswer(new QuestionAnswer("Magasin (Commerce, distribution ou autre)"));
            q5.AddAnswer(new QuestionAnswer("Laboratoire"));
            q5.AddAnswer(new QuestionAnswer("Autre"));
            m_lsQuestions.Add(q5);

            TestQuestion q6 = new TestQuestion(6, "Y a-t-il des risques majeurs concernant votre entreprise qu'il faudrait inclure dans la formation ?", EnumAnswerType.Checkbox);
            q6.AddAnswer(new QuestionAnswer("Risques de chutes de plain-pied & chutes en hauteur"));
            q6.AddAnswer(new QuestionAnswer("Risques de manutention manuelle"));
            q6.AddAnswer(new QuestionAnswer("Risques de manutention m�canis�e"));
            q6.AddAnswer(new QuestionAnswer("Risques de circulations & d�placements"));
            q6.AddAnswer(new QuestionAnswer("Risques d�effondrements & chutes d�objets"));
            q6.AddAnswer(new QuestionAnswer("Risques de toxicit�"));
            q6.AddAnswer(new QuestionAnswer("Risques d�incendies & explosions"));
            q6.AddAnswer(new QuestionAnswer("Risques biologiques"));
            q6.AddAnswer(new QuestionAnswer("Risques d��lectricit�"));
            q6.AddAnswer(new QuestionAnswer("Risques de manque d�hygi�ne"));
            q6.AddAnswer(new QuestionAnswer("Risques de bruits ou Risques de vibrations"));
            q6.AddAnswer(new QuestionAnswer("Risques d�ambiances thermiques ou Risques d�ambiances lumineuses"));
            q6.AddAnswer(new QuestionAnswer("Risques de rayonnements"));
            q6.AddAnswer(new QuestionAnswer("Risques de machines & outils"));
            q6.AddAnswer(new QuestionAnswer("Risques d�interventions en entreprises ext�rieures"));
            q6.AddAnswer(new QuestionAnswer("Risques d�organisation du travail & Stress"));
            m_lsQuestions.Add(q6);

            m_adapter.RefreshListQuestions(m_lsQuestions, m_refresher.Height);

            m_refresher.Refreshing = false;
            m_refresher.Enabled = false;

            //Update activity title
            Title = string.Format(Resources.GetString(Resource.String.activity_question_title), "1", m_adapter.Count.ToString());
        }

        private async Task InitOutils()
        {
            m_lsOutil = new List<Outil>();

            Outil r1 = new Outil
            {
                m_iID = 1,
                m_sTitre = "Jeux question/r�ponse sur une th�matique risque en particulier",
                m_sObjectif = "Faire un focus particulier sur un danger et le risque pr�sent dans l�entreprise en impliquant les personnes dans la connaissance pr�cise de ce risque sp�cifique. C�est un outil strat�gique de d�marche de pr�vention.",
                m_sPourquoi = "Si l�entreprise a des probl�matiques et/ou une accidentologie plus forte sur un risque particulier, l�int�r�t de cet outil est d�impliquer le personnel pour favoriser son apprentissage sur les bons gestes � effectuer/respecter pour �viter l�exposition � ce danger.",
                m_sPublic = "Toute personne devant effectuer la formation g�n�rale � la s�curit�. De 14 � 99 ans !",
                m_sParticipantDescription = "Entre 1 � 5 personnes pour que le jeu reste dynamique",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "le support du jeu et une salle de pr�f�rence (facultatif)",
                m_sMoyenHumain = "1 animateur",
                m_sAttention = ""
            };
            m_lsOutil.Add(r1);

            Outil r2 = new Outil
            {
                m_iID = 2,
                m_sTitre = "Jeux de r�le sur une situation particuli�re dangereuse dans l�entreprise",
                m_sObjectif = "Faire un focus particulier sur une situation dangereuse pr�sente dans l�entreprise en impliquant les personnes � tenir des r�les diff�rents pour apprendre � r�agir correctement face � la situation dangereuse. C�est un outil strat�gique de d�marche de pr�vention.",
                m_sPourquoi = "Si l�entreprise a des probl�matiques et/ou une accidentologie plus forte sur une situation dangereuse particuli�re, l�int�r�t de cet outil est d�impliquer le personnel pour favoriser son apprentissage sur les bons gestes � effectuer/respecter pour �viter l�exposition � ce danger.",
                m_sPublic = "Toute personne devant effectuer la formation g�n�rale � la s�curit�. De 14 � 99 ans !",
                m_sParticipantDescription = "Entre 1 � 10 personnes",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "�l�ments en fonction de l�enjeu du jeux de r�le (ex : t�l�phone plastique, etc.)",
                m_sMoyenHumain = "1 animateur",
                m_sAttention = ""
            };
            m_lsOutil.Add(r2);

            Outil r3 = new Outil
            {
                m_iID = 3,
                m_sTitre = "Visite entreprise + rallye photo sur les informations g�n�rales",
                m_sObjectif = "Faire une visite d�entreprise pour rep�rer visuellement les dangers/risques et les moyens de pr�vention mis en place de l�environnement de travail. Puis laisser la personne form�e �voluer sur le site dans un temps donn� pour retrouver les informations demand�es par le rallye photo. Cela facilitera son apprentissage pour retenir les �l�ments s�curit� du site. C�est un outil strat�gique de d�marche de pr�vention.",
                m_sPourquoi = "Si l�entreprise a des sp�cificit�s m�tiers avec des consignes s�curit� particuli�res ou qu�elle souhaite impliquer davantage les personnes form�es dans leur acquisition de connaissances sur les dangers/risques de l�entreprise, l�int�r�t de cet outil est de laisser autonome la personne form�e dans son apprentissage des bons gestes � effectuer/respecter pour �viter l�exposition � ce danger.",
                m_sPublic = "Toute personne devant effectuer la formation g�n�rale � la s�curit�. De 14 � 99 ans !",
                m_sParticipantDescription = "Entre 1 � 9 personnes/animateur",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "il faut que les personnes form�es aient un t�l�phone avec photo ou envisager un pr�t d�appareil photo/tablette par un groupe de personnes form�es.",
                m_sMoyenHumain = "1 animateur de visite puis pour la restitution.",
                m_sAttention = "�tre vigilant sur : \n- Les autorisations d�acc�s ne permettant pas aux personnes form�es de se d�placer seules,\n-L�autorisation de prendre des photographies dans l��tablissement."
            };
            m_lsOutil.Add(r3);

            Outil r4 = new Outil
            {
                m_iID = 4,
                m_sTitre = "Support vid�o + interaction avec les form�s",
                m_sObjectif = "C�est un outil strat�gique de d�marche de pr�vention. La personne form�e voit au travers de vid�os l�environnement de travail, et permet d�assimiler plus facilement les dangers/risques et les moyens de pr�vention mis en place dans l��tablissement. Son apprentissage est renforc� avec l�interaction faite par l�animateur. Cela facilitera son apprentissage pour retenir les �l�ments s�curit� du site.",
                m_sPourquoi = "Si l�entreprise a de multiples sp�cificit�s m�tiers avec des consignes s�curit� particuli�res, la vid�o permet de faire une pr�sentation des diff�rents corps de m�tiers et d�axer sur les bons gestes � effectuer/respecter pour �viter l�exposition � ce danger. L�animateur peut cibler des vid�os sp�cifiques aux corps de m�tiers des personnes dans la salle. ",
                m_sPublic = "Toute personne devant effectuer la formation g�n�rale � la s�curit�. De 14 � 99 ans !",
                m_sParticipantDescription = "Entre 1 � X personnes",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "une salle et un vid�oprojecteur.",
                m_sMoyenHumain = "1 animateur ",
                m_sAttention = ""
            };
            m_lsOutil.Add(r4);
        }

        private void M_adapter_AnswerSelected(object sender, ListviewAdapter_Display_Questions.AnswerSelectedEventArgs args)
        {
            bool bShowNextButton = false;
            bool bForceScroll = args.answerSelected.m_Question.ShouldGoToNextQuestionAutomatically();

            if (bForceScroll)
                m_iCurrentPosition++;
            else
            {
                if (args.answerSelected.m_Question.IsAnswered())
                    bShowNextButton = true;
                else
                    m_btnValid.Visibility = ViewStates.Gone;
            }

            ScrollListviewToCurrentPosition(bShowNextButton, bForceScroll);
        }

        private void ScrollListviewToCurrentPosition(bool bShowNextButton = false, bool bForceScroll = false)
        {
            //Update activity title
            Title = string.Format(Resources.GetString(Resource.String.activity_question_title), m_iCurrentPosition + 1, m_adapter.Count.ToString());
            
            if (bShowNextButton)
                UpdateButtonValidVisibility();

            if (bForceScroll)
                m_listviewQuestions.SmoothScrollToPositionFromTop(m_iCurrentPosition, 0);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_questions, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    //TODO : Confirm user wants to leave before the end of the questions
                    this.Finish();
                    break;

                case Resource.Id.menu_question_previous:
                    if (m_iCurrentPosition > 0)
                    {
                        m_iCurrentPosition--;
                        ScrollListviewToCurrentPosition(true, true);
                    }
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}