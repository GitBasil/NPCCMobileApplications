using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Library
{
    public static class npcc_services
    {
        public static async Task<WebServiceResault> inf_CallWebServiceAsync<WebServiceResault, post_data>(inf_method method, string url, post_data data = default(post_data)){

            var oauthToken = await SecureStorage.GetAsync("oauth_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", oauthToken);
            client.BaseAddress = new Uri(url);

            try
            {
                HttpResponseMessage response;
                switch (method)
                {
                    case inf_method.Get:
                        response = await client.GetAsync(client.BaseAddress);
                       
                        break;   
                    case inf_method.Post:
                        string json = JsonConvert.SerializeObject(data);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(client.BaseAddress, content);


                        break;            
                    default:
                        response = null;
                        break;
                }

                response.EnsureSuccessStatusCode();
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                WebServiceResault result = JsonConvert.DeserializeObject<WebServiceResault>(jsonResult);

                return result;
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e);

                inf_mobile_exception_managerAsync(e.Message);

                return default(WebServiceResault);
            }
        }

        public static async void inf_mobile_exception_managerAsync(string ex){
            var oauthToken = await SecureStorage.GetAsync("oauth_token");
            if(oauthToken != null)
            await inf_CallWebServiceAsync<bool,bool>(inf_method.Get, "https://webapps.npcc.ae/ApplicationWebServices/api/Common/MobileExceptionManager?exception=" + ex);
        }
    }
}
