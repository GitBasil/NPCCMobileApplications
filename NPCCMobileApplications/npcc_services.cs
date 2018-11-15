using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;
using Newtonsoft.Json;
using Xamarin.Essentials;
using static NPCCMobileApplications.Library.npcc_types;

namespace NPCCMobileApplications.Library
{
    public static class npcc_services
    {
        public static async Task<WebServiceResault> inf_CallWebServiceAsync<WebServiceResault, post_data>(inf_method method, string url, post_data data = default(post_data))
        {

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

        public static async void inf_mobile_exception_managerAsync(string ex)
        {
            var oauthToken = await SecureStorage.GetAsync("oauth_token");
            if (oauthToken != null)
                await inf_CallWebServiceAsync<bool, bool>(inf_method.Get, "https://webapps.npcc.ae/ApplicationWebServices/api/Common/MobileExceptionManager?exception=" + ex);
        }

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public static string npcc_SizeSuffix(long value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + npcc_SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }


        public static long npcc_GetFileSize(string Path)
        {
            Uri uri = new Uri(Path);
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                return long.Parse(fileSize);
            }
        }

        public static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;
                webClient.DownloadDataAsync(new Uri(url));
            }

            return imageBitmap;
        }

        static void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            Bitmap imageBitmap = null;
            if (e.Result != null && e.Result.Length > 0)
            {
                imageBitmap = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);
            }
        }

    }
}
