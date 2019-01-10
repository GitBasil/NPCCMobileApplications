using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Library
{
    public class npcc_authentication
    {
        public bool IsBusy { get; set; }

        public static String GetMacAddress()
        {
            string mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        mac = nic.GetPhysicalAddress().ToString();
                    }
                }
            }

            return mac;
        }

        public  async Task<inf_login_info> Login(string username, string password)
        {
            if (IsBusy)
                return null;
            
            try
            {
                string url = "https://webapps.npcc.ae/ApplicationWebServices/api/Authentication/LoginValidator";

                inf_credentials objCredentials = new inf_credentials();
                objCredentials.username = username;
                objCredentials.password = password;

                var Login_Info = await npcc_services.inf_CallWebServiceAsync<inf_login_info, inf_credentials>(inf_method.Post, url, objCredentials);   
                return Login_Info;
            }
            catch (HttpRequestException e)
            {
                npcc_services.inf_mobile_exception_managerAsync(e.Message);
                return null;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public static async Task<string> getTokenAsync()
        {
            var oauthToken = await SecureStorage.GetAsync("oauth_token");

            return oauthToken;
        }

        //public static async Task<HttpClient> GetAuthenticatedHttpClientAsync()
        //{
        //    var oauthToken = await SecureStorage.GetAsync("oauth_token");
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oauthToken);
        //    return client;
        //}

        public static async Task<HttpClient> GetAuthenticatedHttpClientAsync()
        {
            string oauthToken = await SecureStorage.GetAsync("oauth_token");
            Console.WriteLine(oauthToken);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authentication", "Bearer " + oauthToken);
            return client;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                
            string url = "https://webapps.npcc.ae/ApplicationWebServices/api/Authentication/IsAuthenticated";
                bool isAuthenticated = await npcc_services.inf_CallWebServiceAsync<bool,string>(inf_method.Get, url);

            if(!isAuthenticated) SecureStorage.Remove("oauth_token");
            return isAuthenticated;

            }
            catch (HttpRequestException e)
            {
                npcc_services.inf_mobile_exception_managerAsync(e.Message);
                SecureStorage.Remove("oauth_token");
                return false;
            }
        }
    }
}