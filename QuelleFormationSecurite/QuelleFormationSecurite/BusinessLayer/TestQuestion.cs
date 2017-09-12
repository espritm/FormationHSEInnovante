using System;
using System.Collections.Generic;

namespace QuelleFormationSecurite.BusinessLayer
{
    public class TestQuestion
    {
        public int m_iID { get; set; }
        public string m_sTitle { get; set; }
        public EnumAnswerType m_AnswerType { get; set; }
        public List<QuestionAnswer> m_lsAnswer { get; private set; }


        public TestQuestion(int iID, string sTitle, EnumAnswerType answerType)
        {
            m_iID = iID;
            m_sTitle = sTitle;
            m_AnswerType = answerType;
            m_lsAnswer = new List<QuestionAnswer>();
        }

        public void AddAnswer(QuestionAnswer answer)
        {
            m_lsAnswer.Add(answer);
            answer.m_Question = this;
        }

        public bool IsAnswered()
        {
            return m_lsAnswer.Find(a => a.m_bIsSelected) != null;
        }

        public bool ShouldGoToNextQuestionAutomatically()
        {
            return m_AnswerType == EnumAnswerType.Radiobutton;
        }
    }
}
