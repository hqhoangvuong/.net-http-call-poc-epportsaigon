using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Net;
using System.Text;

namespace WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await ProcessRepositories();
        }

        private static async Task ProcessRepositories()
        {
            //client.DefaultRequestHeaders.Accept.Clear();

            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");


            //var httpRequest = client.GetAsync("https://eport.saigonnewport.com.vn");
            //var response = await httpRequest;
            //IEnumerable<String> cookies;
            //response.Headers.TryGetValues("Set-Cookie", out cookies);

            //foreach (string cookie in cookies)
            //{
            //    Console.WriteLine(cookie.Split(";")[0]);
            //}

            await shit();
        }

        private static CookieContainer makeCookie()
        {
            CookieContainer cookieContainer = new CookieContainer();
            var baseAddress = new Uri("https://eport.saigonnewport.com.vn");
            Cookie sessionId = new Cookie("ASP.NET_SessionId", "p3k1v11yp55scyrmnipytg3q");
            Cookie verificationToken = new Cookie("__RequestVerificationToken", "3OXZhPO5X_xdFJYOtf9qd-bKmrZ9x_A0NiQHFfjIW1CRQR794ZYhqqIUSuNHgjM3I6jirRoCFMxLj3HEXJPvOTHW8fkUI4B7_tqpBxz2-P01");
            Cookie bigIpServerHttp = new Cookie("BIGipServerHTTP-iApp_ePort-Web_443.app~HTTP-iApp_ePort-Web_443_pool", "!OUOD Itqcipynd3qX/Ld0TZxi Xlv5H7fR vCFv4k0cqrXrdvHKg8s0KXb8xBeGzDXVXKBtwPnQZ2HU=");
            Cookie aspxAuth = new Cookie(".ASPXAUTH", "95CC8F72D2366C2FF727298D8D8FEEF1092B14F25A0B080020083EA18DF8BABDD9EDB0218AAFA7AE7F58C18382B8EB593B849F76B6EA5119066B742A9D37C4917C9CB02B969AAF3F11CA0C66E2A327ED4E26502ECE11C0740BFE62F054737ACD");

            cookieContainer.Add(baseAddress, sessionId);
            cookieContainer.Add(baseAddress, verificationToken);
            cookieContainer.Add(baseAddress, bigIpServerHttp);
            cookieContainer.Add(baseAddress, aspxAuth);

            return cookieContainer;
        }

        private static void postRequest()
        {

            var baseAddress = new Uri("https://eport.saigonnewport.com.vn");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://eport.saigonnewport.com.vn/ContainerInformation/FindContInfo");
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.CookieContainer = makeCookie();

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ 'viewModel':{ 'SITE_ID':'CTL','SearchContainerNos':'12345','IsSearchByInYard':true,'IsSearchByBatch':false}}";
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }

        private static async Task shit()
        {
            var baseAddress = new Uri("https://eport.saigonnewport.com.vn"); using (var handler = new HttpClientHandler() { CookieContainer = makeCookie() })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                string json = "{ 'viewModel':{ 'SITE_ID':'CTL','SearchContainerNos':'12345','IsSearchByInYard':true,'IsSearchByBatch':false}}";
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/ContainerInformation/FindContInfo", httpContent);
                var contents = await result.Content.ReadAsStringAsync();
                result.EnsureSuccessStatusCode();
                Console.WriteLine(contents);
            }
        }
    }
}