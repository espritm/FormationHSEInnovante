using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Microsoft.Azure.Mobile.Analytics;
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
             * question 4 : "Un animateur de formation" et "Une enveloppe budgétaire pour d'éventuels éléments de jeux (téléphones plastiques, etc...)"
             */

            //Outil 3 : 
            /*
             * question 1 : "Entre 1 et 5" ou "Entre 6 et 10"
             * question 4 : "Un animateur de formation" et 
             *      ( "Une enveloppe budgétaire pour d'éventuels éléments de jeux (téléphones plastiques, etc...)" ou 
             *        "Une ou plusieurs tablettes" ou 
             *        "Les personnes formées disposent de smartphone" )
             */

            //Outil 4 : 
            /*
             * question 4 : "Un ordinateur et un vidéo-projecteur"
             */

            //Outil 5 : 
            /*
             * question 4 : "Accès internet (wifi ou autre) dans l'établissement"  et "Une enveloppe budgétaire pour d'éventuels éléments de jeux (téléphones plastiques, etc...)"
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
                q4.m_lsAnswer.Find(a => a.m_sTitle == "Une enveloppe budgétaire pour d'éventuels éléments de jeux (téléphones plastiques, etc...)").m_bIsSelected)
            {
                m_Result.m_lsOutil.Add(m_lsOutil.Find(o => o.m_iID == 2));
            }

            //Outil 3
            if ((q1.m_lsAnswer.Find(a => a.m_sTitle == "Entre 1 et 5").m_bIsSelected || q1.m_lsAnswer.Find(a => a.m_sTitle == "Entre 6 et 10").m_bIsSelected) &&
                q4.m_lsAnswer.Find(a => a.m_sTitle == "Un animateur de formation").m_bIsSelected &&
                ( q4.m_lsAnswer.Find(a => a.m_sTitle == "Une enveloppe budgétaire pour d'éventuels éléments de jeux (téléphones plastiques, etc...)").m_bIsSelected ||
                  q4.m_lsAnswer.Find(a => a.m_sTitle == "Une ou plusieurs tablette(s)").m_bIsSelected ||
                  q4.m_lsAnswer.Find(a => a.m_sTitle == "Les personnes formées disposent de smartphone").m_bIsSelected))
            {
                m_Result.m_lsOutil.Add(m_lsOutil.Find(o => o.m_iID == 3));
            }

            //Outil 4
            if (q4.m_lsAnswer.Find(a => a.m_sTitle == "Un ordinateur et un vidéo-projecteur").m_bIsSelected)
            {
                m_Result.m_lsOutil.Add(m_lsOutil.Find(o => o.m_iID == 4));
            }

            //Outil 5
            if (q4.m_lsAnswer.Find(a => a.m_sTitle == "Accès internet (wifi ou autre) dans l'établissement").m_bIsSelected &&
                q4.m_lsAnswer.Find(a => a.m_sTitle == "Une enveloppe budgétaire pour d'éventuels éléments de jeux (téléphones plastiques, etc...)").m_bIsSelected)
            {
                m_Result.m_lsOutil.Add(m_lsOutil.Find(o => o.m_iID == 5));
            }

            //Vérifie qu'un moins 1 outil est proposé
            if (m_Result.m_lsOutil.Count == 0)
            {
                Android.Support.V7.App.AlertDialog dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                   .SetTitle("Aucun outil de formation ne correspond")
                   .SetMessage("Au vu de vos réponses, le plus simple serait de vous munir d'un vidéo-projecteur.\nCochez la case \"vidéo - projecteur\" à la question 4 !")
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

            //Track event
            Analytics.TrackEvent("Questionnaire répondu", m_Result.m_dicQuestionReponse);

            this.Finish();
        }

        private async Task InitQuestions()
        {
            m_refresher.Refreshing = true;

            m_lsQuestions = new List<TestQuestion>();

            TestQuestion q1 = new TestQuestion(1, "Quel est le nombre de personnes à former ?", EnumAnswerType.Radiobutton);
            q1.AddAnswer(new QuestionAnswer("Entre 1 et 5"));
            q1.AddAnswer(new QuestionAnswer("Entre 6 et 10"));
            q1.AddAnswer(new QuestionAnswer("Plus de 11"));
            m_lsQuestions.Add(q1);

            TestQuestion q2 = new TestQuestion(2, "Quelle est la moyenne d'âge des personnes à former ?", EnumAnswerType.Radiobutton);
            q2.AddAnswer(new QuestionAnswer("Entre 14 et 18 ans"));
            q2.AddAnswer(new QuestionAnswer("Entre 19 et 28 ans"));
            q2.AddAnswer(new QuestionAnswer("Entre 29 et 39 ans"));
            q2.AddAnswer(new QuestionAnswer("Plus de 39 ans"));
            m_lsQuestions.Add(q2);

            TestQuestion q3 = new TestQuestion(3, "Les personnes à former ont-elles déjà travaillé dans un environnement similaire à votre entreprise ?", EnumAnswerType.Radiobutton);
            q3.AddAnswer(new QuestionAnswer("Non, jamais"));
            q3.AddAnswer(new QuestionAnswer("Oui, moins de 6 mois"));
            q3.AddAnswer(new QuestionAnswer("Oui, plus de 6 mois"));
            q3.AddAnswer(new QuestionAnswer("C'est mitigé : certaines personnes oui, d'autres non"));
            m_lsQuestions.Add(q3);

            TestQuestion q4 = new TestQuestion(4, "Quels sont les moyens à votre disposition pour la formation ?", EnumAnswerType.Checkbox);
            q4.AddAnswer(new QuestionAnswer("Un animateur de formation"));
            q4.AddAnswer(new QuestionAnswer("Une salle de formation"));
            q4.AddAnswer(new QuestionAnswer("Un ordinateur et un vidéo-projecteur"));
            q4.AddAnswer(new QuestionAnswer("Un tableau blanc"));
            q4.AddAnswer(new QuestionAnswer("Une ou plusieurs tablette(s)"));
            q4.AddAnswer(new QuestionAnswer("Une ou plusieurs lunette(s) de réalité virtuelle (VR)"));
            q4.AddAnswer(new QuestionAnswer("Une enveloppe budgétaire pour d'éventuels éléments de jeux (téléphones plastiques, etc...)"));
            q4.AddAnswer(new QuestionAnswer("Les personnes formées disposent de smartphone"));
            q4.AddAnswer(new QuestionAnswer("Accès internet (wifi ou autre) dans l'établissement"));
            m_lsQuestions.Add(q4);

            TestQuestion q5 = new TestQuestion(5, "Dans quel(s) milieu(x) vont travailler les personnes à former ?", EnumAnswerType.Checkbox);
            q5.AddAnswer(new QuestionAnswer("Bureau (tertiaire)"));
            q5.AddAnswer(new QuestionAnswer("Atelier (production)"));
            q5.AddAnswer(new QuestionAnswer("Logistique (transport & logistique)"));
            q5.AddAnswer(new QuestionAnswer("Chantier (BTP ou autre)"));
            q5.AddAnswer(new QuestionAnswer("Magasin (Commerce, distribution ou autre)"));
            q5.AddAnswer(new QuestionAnswer("Laboratoire"));
            q5.AddAnswer(new QuestionAnswer("Autre"));
            m_lsQuestions.Add(q5);

            TestQuestion q6 = new TestQuestion(6, "Quels sont les risques majeurs de votre entreprise ?", EnumAnswerType.Checkbox);
            q6.AddAnswer(new QuestionAnswer("Risques de chutes de plain-pied & chutes en hauteur"));
            q6.AddAnswer(new QuestionAnswer("Risques de manutention manuelle"));
            q6.AddAnswer(new QuestionAnswer("Risques de manutention mécanisée"));
            q6.AddAnswer(new QuestionAnswer("Risques de circulations & déplacements"));
            q6.AddAnswer(new QuestionAnswer("Risques deffondrements & chutes dobjets"));
            q6.AddAnswer(new QuestionAnswer("Risques de toxicité"));
            q6.AddAnswer(new QuestionAnswer("Risques dincendies & explosions"));
            q6.AddAnswer(new QuestionAnswer("Risques biologiques"));
            q6.AddAnswer(new QuestionAnswer("Risques délectricité"));
            q6.AddAnswer(new QuestionAnswer("Risques de manque dhygiène"));
            q6.AddAnswer(new QuestionAnswer("Risques de bruits ou Risques de vibrations"));
            q6.AddAnswer(new QuestionAnswer("Risques dambiances thermiques ou Risques dambiances lumineuses"));
            q6.AddAnswer(new QuestionAnswer("Risques de rayonnements"));
            q6.AddAnswer(new QuestionAnswer("Risques de machines & outils"));
            q6.AddAnswer(new QuestionAnswer("Risques dinterventions en entreprises extérieures"));
            q6.AddAnswer(new QuestionAnswer("Risques dorganisation du travail & Stress"));
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
                m_sTitre = "Jeux questions/réponses sur une thématique risque en particulier",
                m_iImageResource = Resource.Drawable.ic_jeux_question_reponse,
                m_sObjectif = "Faire un focus particulier sur un danger et le risque présent dans lentreprise en impliquant les personnes dans la connaissance précise de ce risque spécifique. Cest un outil stratégique de démarche de prévention.",
                m_sPourquoi = "Si l’entreprise a des problématiques et/ou une accidentologie plus forte sur un risque particulier, l’intérêt de cet outil est d’impliquer le personnel pour favoriser son apprentissage sur les bons gestes à effectuer/respecter pour éviter l’exposition à ce danger. C’est un outil stratégique de démarche de prévention.",
                m_sPublic = "Toute personne devant effectuer la formation générale à la sécurité. De 14 à 99 ans !",
                m_sParticipantDescription = "Entre 1 à 5 personnes pour que le jeu reste dynamique",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "le support du jeu et une salle de préférence (facultatif)",
                m_sMoyenHumain = "1 animateur",
                m_sAttention = ""
            };
            m_lsOutil.Add(r1);

            Outil r2 = new Outil
            {
                m_iID = 2,
                m_sTitre = "Jeux de rôle sur une situation particulière dangereuse dans lentreprise",
                m_iImageResource = Resource.Drawable.ic_jeux_de_role,
                m_sObjectif = "Faire un focus particulier sur une situation dangereuse présente dans lentreprise en impliquant les personnes à tenir des rôles différents pour apprendre à réagir correctement face à la situation dangereuse. Cest un outil stratégique de démarche de prévention.",
                m_sPourquoi = "Si lentreprise a des problématiques et/ou une accidentologie plus forte sur une situation dangereuse particulière, lintérêt de cet outil est dimpliquer le personnel pour favoriser son apprentissage sur les bons gestes à effectuer/respecter pour éviter lexposition à ce danger.",
                m_sPublic = "Toute personne devant effectuer la formation générale à la sécurité. De 14 à 99 ans !",
                m_sParticipantDescription = "Entre 1 à 10 personnes",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "éléments en fonction de lenjeu du jeux de rôle (ex : téléphone plastique, etc.)",
                m_sMoyenHumain = "1 animateur",
                m_sAttention = ""
            };
            m_lsOutil.Add(r2);

            Outil r3 = new Outil
            {
                m_iID = 3,
                m_sTitre = "Visite entreprise + rallye photo sur les informations générales",
                m_iImageResource = Resource.Drawable.ic_rallye_photo,
                m_sObjectif = "Faire une visite dentreprise pour repérer visuellement les dangers/risques et les moyens de prévention mis en place de lenvironnement de travail. Puis laisser la personne formée évoluer sur le site dans un temps donné pour retrouver les informations demandées par le rallye photo. Cela facilitera son apprentissage pour retenir les éléments sécurité du site. Cest un outil stratégique de démarche de prévention.",
                m_sPourquoi = "Si lentreprise a des spécificités métiers avec des consignes sécurité particulières ou quelle souhaite impliquer davantage les personnes formées dans leur acquisition de connaissances sur les dangers/risques de lentreprise, lintérêt de cet outil est de laisser autonome la personne formée dans son apprentissage des bons gestes à effectuer/respecter pour éviter lexposition à ce danger.",
                m_sPublic = "Toute personne devant effectuer la formation générale à la sécurité. De 14 à 99 ans !",
                m_sParticipantDescription = "Entre 1 à 9 personnes/animateur",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "il faut que les personnes formées aient un téléphone avec photo ou envisager un prêt dappareil photo/tablette par un groupe de personnes formées.",
                m_sMoyenHumain = "1 animateur de visite puis pour la restitution.",
                m_sAttention = "Être vigilant sur : \n- Les autorisations daccès ne permettant pas aux personnes formées de se déplacer seules,\n-Lautorisation de prendre des photographies dans létablissement."
            };
            m_lsOutil.Add(r3);

            Outil r4 = new Outil
            {
                m_iID = 4,
                m_sTitre = "Support vidéo + interaction avec les formés",
                m_sObjectif = "La personne formée voit au travers de vidéos l’environnement de travail. Elle assimile plus facilement les dangers/risques. Son apprentissage est renforcé avec l’interaction faite par l’animateur. Cela facilitera son apprentissage pour retenir les éléments sécurité du site.",
                m_sPourquoi = "Si l’entreprise a de multiples spécificités métiers avec des consignes sécurité particulières, la vidéo permet de faire une présentation des différents corps de métiers et d’axer sur les bons gestes à effectuer/respecter pour éviter l’exposition à ce danger. L’animateur peut cibler des vidéos spécifiques aux corps de métiers des personnes dans la salle. C’est un outil stratégique de démarche de prévention.",
                m_sPublic = "Toute personne devant effectuer la formation générale à la sécurité. De 14 à 99 ans !",
                m_sParticipantDescription = "Entre 1 à X personnes",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "une salle et un vidéoprojecteur.",
                m_sMoyenHumain = "1 animateur ",
                m_sAttention = ""
            };
            m_lsOutil.Add(r4);

            Outil r5 = new Outil
            {
                m_iID = 5,
                m_sTitre = "Jeux de piste avec FlashCode",
                m_sObjectif = "Rendre la personne actrice de sa formation sécurité et/ou de la visite des locaux, favorisant lapprentissage des risques et des gestes de préventions associés à des lieux de lentreprise. Cest un outil de prévention des risques coûteux à mettre en place, mais permettant une autonomie complète de la part des formés. De plus, en dehors de la formation à la sécurité, chaque salarié pourra à nouveau flasher un FlashCode pour revoir les instructions associées, à tout moment.",
                m_sPourquoi = "Si lentreprise a de multiples spécificités métiers avec des consignes sécurité particulières, quelle souhaite impliquer davantage les personnes formées dans leur acquisition de connaissances sur les dangers/risques de lentreprise, quelle souhaite une autonomie complète de la part des personnes formées, ou quelle souhaite laisser la possibilité aux personnes formées de revoir à tout moment et en autonomie une partie de la formation sécurité, lintérêt de cet outil est de laisser autonome la personne formée dans son apprentissage des bons gestes à effectuer/respecter pour éviter lexposition à ce danger.",
                m_sPublic = "Toute personne devant effectuer la formation générale à la sécurité. De 14 à 99 ans !",
                m_sParticipantDescription = "Entre 1 à X personnes",
                m_sTypeEntreprise = "TPE/PME",
                m_sMoyenTechniques = "Connexion wifi dans les locaux, étiquettes avec FlashCode",
                m_sMoyenHumain = "",
                m_sAttention = "Être vigilant sur : \n- Les autorisations daccès ne permettant pas aux personnes formées de se déplacer seules."
            };
            m_lsOutil.Add(r5);
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
