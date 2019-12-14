using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebJobVink
{
    public class SendNotifications
    {
        #region Singleton
        private static SendNotifications _instance = null;
        public static SendNotifications Instance
        {
            get
            {
                if (_instance == null) { _instance = new SendNotifications(); }
                return _instance;
            }
        }
        #endregion

        public void NotifyUser(string message)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                                    + "\"app_id\": \"" + "c2df8f3d-8733-47b2-87b3-9787310cecc3" + "\","
                                                    + "\"contents\": {\"en\": \"" + message + "\"},"
                                                    + "\"include_player_ids\": [\"db00a68e-0adf-40c1-8f6b-0dd0b4ee9606\"]}");

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }
    }
}
