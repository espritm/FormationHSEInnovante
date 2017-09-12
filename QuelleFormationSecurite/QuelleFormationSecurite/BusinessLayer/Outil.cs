using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuelleFormationSecurite.BusinessLayer
{
    public class Outil
    {
        public int m_iID { get; set; }
        public string m_sTitre { get; set; }
        public string m_sObjectif { get; set; }
        public string m_sPourquoi { get; set; }
        public string m_sPublic { get; set; }
        public string m_sParticipantDescription { get; set; }
        public string m_sTypeEntreprise { get; set; }
        public string m_sMoyenTechniques { get; set; }
        public string m_sMoyenHumain { get; set; }
        public string m_sAttention { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Identifiant : " + m_iID);
            sb.AppendLine("Titre : " + m_sTitre);
            sb.AppendLine("Objectif : " + m_sObjectif);
            sb.AppendLine("Pourquoi : " + m_sPourquoi);
            sb.AppendLine("Quel public : " + m_sPublic);
            sb.AppendLine("Participants : " + m_sParticipantDescription);
            sb.AppendLine("Type entreprise : " + m_sTypeEntreprise);
            sb.AppendLine("Moyens Techniques : " + m_sMoyenTechniques);
            sb.AppendLine("Moyens humains : " + m_sMoyenHumain);
            sb.AppendLine("Points de vigilance : " + m_sAttention);

            return sb.ToString();
        }
    }
}
