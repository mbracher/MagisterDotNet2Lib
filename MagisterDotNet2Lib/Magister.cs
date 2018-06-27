using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace MagisterDotNet2Lib
{
    public class Magister
    {
        private string baseUrl;
        private CookedWebClient wc;

        Session session;
        private string sessionId;

		private string kidId;
		private string aanmeldingId;

        private Account account;
        private List<Rol> rollen;
        private Aanmeldingen aanmeldingen;
		private Kinderen kinderen;

		public Magister(string username, string password, string baseUrl)
        {
            wc = new CookedWebClient();
            wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
            wc.Headers.Add("X-API-Client-ID", "12D8");
            sessionId = "";
			this.baseUrl = baseUrl;

            login(username, password);
        }


        public Dictionary<String, Vak> getVakken()
        {
            Dictionary<String, Vak> vakken = new Dictionary<string, Vak>();           

            wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
            string response = wc.DownloadString(baseUrl + "/api/personen/" + kidId + "/aanmeldingen/" + aanmeldingId + "/cijfers/cijferoverzichtvooraanmelding?actievePerioden=false&alleenBerekendeKolommen=false&alleenPTAKolommen=false");

            //Console.WriteLine(response);
            JObject r = JObject.Parse(response);
            foreach(var item in r["Items"]) {
                if (item["CijferKolom"]["KolomSoort"].Value<int>() == 1
                    && item["Inhalen"].Value<bool>() == false
                    && item["TeltMee"].Value<bool>() == true
                    && item["VakVrijstelling"].Value<bool>() == false
                    && item["CijferStr"].Value<string>().CompareTo("Vr") != 0)
                {
                    Cijfer cijfer = GetCijfer(item["CijferStr"].Value<string>(), item["CijferKolom"]["Id"].Value<string>());
                    //Console.Write(item["CijferId"]);
                    //Console.Write(item["Vak"]["Afkorting"]);
                    //Console.Write("\t");
                    //Console.Write(item["CijferStr"]);
                    //Console.WriteLine();

                    Vak vak;
                    if( vakken.ContainsKey(item["Vak"]["Afkorting"].Value<string>())){
                        vak = vakken[item["Vak"]["Afkorting"].Value<string>()];
                    }
                    //Vak vak = vakken[item["Vak"]["Afkorting"].Value<string>()];
                    //if(vak == null) {
                    else {
                        vak = new Vak(item["Vak"]["Omschrijving"].Value<string>());
                        vakken[item["Vak"]["Afkorting"].Value<string>()] = vak;
                    }
                    vak.AddCijfer(cijfer);
                }
            }

            return vakken;

        }

        public Cijfer GetCijfer(string CijferStr, string cijferId) {
            wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
            string response = wc.DownloadString(baseUrl + "/api/personen/" + kidId + "/aanmeldingen/" + aanmeldingId + "/cijfers/extracijferkolominfo/" + cijferId);

            //Console.WriteLine(response);
            JObject r = JObject.Parse(response);

            decimal waarde = decimal.Parse(CijferStr, NumberStyles.Any, CultureInfo.GetCultureInfo("nl-NL"));
            decimal weging = decimal.Parse(r["Weging"].Value<string>(), NumberStyles.Any, CultureInfo.InvariantCulture);

            Cijfer cijfer = new Cijfer(waarde, weging);

            return cijfer;
        }

        private bool login(string username, string password) {
            string response;
            wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
            response = wc.UploadString(baseUrl + "/api/sessies/huidige", "DELETE", "");
            //Console.WriteLine(response);
            wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
            response = wc.DownloadString(baseUrl + "/api/versie");
            //Console.WriteLine(response);
            string payload = "{\"Gebruikersnaam\":\"" + username + "\",\"Wachtwoord\":\"" + password + "\",\"IngelogdBlijven\":\"false\"}";
            //Console.WriteLine(payload);
            wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");

            try
            {
                response = wc.UploadString(baseUrl + "/api/sessies", "POST", payload);
            }
            catch(Exception e) {
                //Console.WriteLine(e);
                return false;
            }
            //Console.WriteLine(response);
            session = JsonConvert.DeserializeObject<Session>(response);
            sessionId = wc.Cookies.GetCookies(new Uri(baseUrl+"/api/"))["SESSION_ID"].Value;

            //Console.WriteLine("sessionId: " + sessionId);
           
            account = getTypeData<Account>(session?.links?.account?.href);
            rollen = getTypeData<List<Rol>>(session?.links?.rollen?.href);
            Account2 loggedInAccount = getTypeData<Account2>("/api/account?noCache=0");

			if (rollen == null || rollen.Count == 0 || loggedInAccount == null)
			{
				return false;
			}

			if (rollen[0].id == 1)
			{ //kind
				kidId = loggedInAccount.Persoon.Id.ToString();
			}
			else if (rollen[0].id == 2)
			{ //ouder
				kinderen = getTypeData<Kinderen>("/api/personen/" + loggedInAccount.Persoon.Id.ToString() + "/kinderen?openData=%27%27");
				kidId = kinderen.Items[0].Id.ToString();
			}

			aanmeldingen = getTypeData<Aanmeldingen>("/api/personen/" + kidId + "/aanmeldingen?geenToekomstige=false");
			aanmeldingId = aanmeldingen.Items[aanmeldingen.Items.Count - 1].id.ToString();

			return true;
        }

        private string getData(string apiUrl){
            if (string.IsNullOrWhiteSpace(apiUrl)) {
                return null;
            }
            string response = wc.DownloadString(baseUrl + apiUrl);

            //Console.WriteLine(response);
            return response;
        }

        private T getTypeData<T>(string apiUrl) {
            return JsonConvert.DeserializeObject<T>(getData(apiUrl));
        }

        private class CookedWebClient : WebClient
        {

            CookieContainer cookies = new CookieContainer();

            public CookieContainer Cookies { get { return cookies; } }

            protected override WebRequest GetWebRequest(Uri address)
            {

                WebRequest request = base.GetWebRequest(address);

                if (request.GetType() == typeof(HttpWebRequest))
                    ((HttpWebRequest)request).CookieContainer = cookies;

                return request;

            }

        }
            
    }
}
