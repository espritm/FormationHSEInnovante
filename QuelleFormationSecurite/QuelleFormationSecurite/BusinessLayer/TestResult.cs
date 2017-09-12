

using System;
using System.Collections.Generic;
using System.Text;

namespace QuelleFormationSecurite.BusinessLayer
{
    public class TestResult
    {
        public DateTime m_dateAnswer { get; set; }
        public List<Outil> m_lsOutil { get; set; }
        public Dictionary<string, string> m_dicQuestionReponse {get;set;}


        public TestResult()
        {
            m_dateAnswer = DateTime.Now;
            m_lsOutil = new List<Outil>();
            m_dicQuestionReponse = new Dictionary<string, string>();
        }

        public string ToStringForEmail()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Date du questionnaire : " + m_dateAnswer.ToString("dd-MM-yyyy HH:mm"));
            foreach (KeyValuePair<string, string> questionReponse in m_dicQuestionReponse)
            {
                sb.AppendLine(questionReponse.Value);
                sb.AppendLine("\t" + questionReponse.Key);
            }

            return sb.ToString();
        }
    }
}
