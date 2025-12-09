using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace YandexGptTest
{
    internal class Program
    {
        static string ApiKey = "***";
        static string FolderId = "***";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<Message> history = new List<Message>();

            Console.WriteLine("YandexGPT чат. Введите 'exit' для выхода.\n");

            while (true)
            {
                Console.Write("Вы: ");
                string input = Console.ReadLine();

                if (input == "exit")
                    break;

                history.Add(new Message { role = "user", text = input });

                string reply = await SendToYandexGPT(history);

                if (reply != null)
                {
                    Console.WriteLine("Бот: " + reply);
                    history.Add(new Message { role = "assistant", text = reply });
                }
                else
                {
                    Console.WriteLine("Ошибка получения ответа.\n");
                }
            }
        }

        static async Task<string> SendToYandexGPT(List<Message> history)
        {
            var url = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Api-Key", ApiKey);

                var request = new YandexRequest
                {
                    modelUri = $"gpt://{FolderId}/yandexgpt-lite",
                    completionOptions = new CompletionOptions { stream = false },
                    messages = history
                };

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Ошибка API: {response.StatusCode}");
                    Console.WriteLine(responseText);
                    return null;
                }

                var result = JsonConvert.DeserializeObject<YandexResponse>(responseText);
                return result?.result?.alternatives?[0]?.message?.text;
            }
        }
    }

    public class Message
    {
        public string role { get; set; }
        public string text { get; set; }
    }

    public class CompletionOptions
    {
        public bool stream { get; set; }
    }

    public class YandexRequest
    {
        public string modelUri { get; set; }
        public CompletionOptions completionOptions { get; set; }
        public List<Message> messages { get; set; }
    }

    public class YandexResponse
    {
        public ResponseResult result { get; set; }
    }

    public class ResponseResult
    {
        public List<Alternative> alternatives { get; set; }
    }

    public class Alternative
    {
        public Message message { get; set; }
    }
}
