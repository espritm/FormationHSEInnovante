using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuelleFormationSecurite.Helpers
{
    public class JSON
    {
        public static Task<T> DeserializeObjectAsync<T>(string value, string sMoreInfoForExceptionThrowned)
        {
            return Task.Factory.StartNew(() => DeserializeObject<T>(value, sMoreInfoForExceptionThrowned));
        }

        public static Task<string> SerializeObjectAsync<T>(T item, string sMoreInfoForExceptionThrowned = "")
        {
            return Task.Factory.StartNew(() => SerializeObject<T>(item, sMoreInfoForExceptionThrowned));
        }

        public static T DeserializeObject<T>(string value, string sMoreInfoForExceptionThrowned)
        {
            T res = default(T);

            try
            {
                res = JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception e)
            {
            }

            return res;
        }

        public static string SerializeObject<T>(T item, string sMoreInfoForExceptionThrowned = "")
        {
            string res = "";

            try
            {
                res = JsonConvert.SerializeObject(item);
            }
            catch (Exception e)
            {
            }

            return res;
        }
    }
}
