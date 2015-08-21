using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Notification.Service.Models;

namespace Notification.Service.Controllers {
    public class NotificationsController : ApiController {
        private readonly string _baseApiUri;
        private readonly string _authorizationToken;

        public NotificationsController() {
            // Get Web API Uri
            _baseApiUri = ConfigurationManager.AppSettings.Get("WebApiUri");
            _authorizationToken = "dk";
        }

        public async Task<bool> Get(int notifyPersonCode, string notifyPersonType, string mobileNumber) {
            bool sentNotify = false;        
            
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(_baseApiUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Basic", "YWRtaW46YWRtaW4=");

                HttpResponseMessage response;
                if (notifyPersonType == "patient") {
                    response = await client.GetAsync(_baseApiUri + "Patients/" + notifyPersonCode);

                    if (response.IsSuccessStatusCode) {
                        var patient = await response.Content.ReadAsAsync<Patient>();

                        //Send Notification
                        if (true) {
                            sentNotify = true;
                        }
                    }
                }
                else if (notifyPersonType == "doctor") {
                    response = await client.GetAsync(_baseApiUri + "Doctors/" + notifyPersonCode);

                    if (response.IsSuccessStatusCode) {
                        var doctor = await response.Content.ReadAsAsync<Doctor>();

                        //Send Notification
                        //if (sent) {
                        //    sentNotify = true;
                        //}
                    }
                }
            }
            return sentNotify;
        }
    }
}
