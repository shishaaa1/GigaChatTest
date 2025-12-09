using Newtonsoft.Json;
using System.Net.Security;
using System.Text.Json.Serialization;

namespace GigaChatTest
{
    internal class Program
    {
        /// <summary>
        /// Клиент ID
        /// </summary>
        static string ClientId = "019b038f-be32-7728-9ba8-86b03afb5efc";
        /// <summary>
        /// Код авторизации
        /// </summary>
        static string AuthorizationKey = "MDE5YjAzOGYtYmUzMi03NzI4LTliYTgtODZiMDNhZmI1ZWZjOjdlZTk5Y2MzLTFlMGEtNGFjMC1hMjI0LWMxY2ZiZjY1ZmNiMA==";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
        /// <summary>
        /// Метод получения токена пользователя
        /// </summary>
        /// <param name="RqUID"></param>
        /// <param name="Bearer"></param>
        /// <returns>Токен для выполнения запросов</returns>
        public static async Task<string> GetToken(string rqUID, string bearer)
        {
            string ReturnToken = null;
            string Url = "https://ngw.devices.sberbank.ru:9443/api/v2/oauth";
            using (HttpClientHandler Handler = new HttpClientHandler())
            {
                Handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyError) => true;
                using (HttpClient client = new HttpClient(Handler))
                {
                    HttpRequestMessage Request = new HttpRequestMessage(HttpMethod.Post, Url);
                    Request.Headers.Add("Accept", "application/json");
                    Request.Headers.Add("RqUID", rqUID);
                    Request.Headers.Add("Authorization", $"Bearer {bearer}");
                    var Data = new List<KeyValuePair<string, string>>
                    {
                       new KeyValuePair<string, string>("scope", "GIGACHAT_API_PERS")
                    };
                    Request.Content = new FormUrlEncodedContent(Data);
                    HttpResponseMessage Response = await client.SendAsync(Request);
                    if (Response.IsSuccessStatusCode)
                    {
                        string ResponseContent = await Response.Content.ReadAsStringAsync();
                        ResponseToken Token = JsonConvert.DeserializeObject<string>(ResponseContent);
                        ReturnToken = Token.access_token;
                    }

                }
            }
            return ReturnToken;
        }
    }
}
