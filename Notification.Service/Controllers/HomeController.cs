using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Notification.Service.Models;

namespace Notification.Service.Controllers {
    public class HomeController : Controller {
        private readonly string _baseApiUri;
        private readonly string _authorizationToken;

        public HomeController() {
            // Get Web API Uri
            _baseApiUri = ConfigurationManager.AppSettings.Get("WebApiUri");
            _authorizationToken = "dk";
        }
        public ActionResult Index() {

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Test(int notifyPersonCode, string notifyPersonType, string mobileNumber) {
            bool sentNotify = false;    
            using (var client = new HttpClient()) { 
                client.BaseAddress = new Uri(_baseApiUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic YWRtaW46YWRtaW4=");

                HttpResponseMessage response;
                if (notifyPersonType == "patient") {
                    //response = await client.GetAsync(_baseApiUri + "Patients/7f726729-dc5f-4508-8d28-d10b9cd2a335" + notifyPersonCode);
                    //response = await client.GetAsync(_baseApiUri + "Patients/3b12ae0b-8dab-46e3-847f-0642ac1a47a7");
                    response = await client.GetAsync("https://52.68.122.167:9443/governance/patients/3b12ae0b-8dab-46e3-847f-0642ac1a47a7");

                    if (response.IsSuccessStatusCode) {
                        var patient = await response.Content.ReadAsAsync<Patient>();

                        //Send Notification
                        if (true) {
                            sentNotify = true;
                        }
                    }
                }
                else if (notifyPersonType == "doctor") {
                    response = await client.GetAsync(_baseApiUri + "Doctor/Doctors/" + notifyPersonCode);

                    if (response.IsSuccessStatusCode) {
                        var doctor = await response.Content.ReadAsAsync<Doctor>();

                        //Send Notification
                        //if (sent) {
                        //    sentNotify = true;
                        //}
                    }
                }
            }
           // return sentNotify;

            return Json(new { ActionStatus = sentNotify }, JsonRequestBehavior.AllowGet);
        }
    }
}
