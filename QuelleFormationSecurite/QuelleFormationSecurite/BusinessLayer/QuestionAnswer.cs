namespace QuelleFormationSecurite.BusinessLayer
{
    public class QuestionAnswer
    {
        public string m_sTitle { get; set; }
        public TestQuestion m_Question { get; set; }
        public bool m_bIsSelected { get; set; } //Add m_sAnswer if an answer need to selected text 

        public QuestionAnswer(string sTitle)
        {
            m_sTitle = sTitle;
        }
    }
}
