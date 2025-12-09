namespace YandexGptTest.Models
{
    public class YandexRequest
    {
        public string modelUri { get; set; }
        public CompletionOptions completionOptions { get; set; }
        public List<Message> messages { get; set; }

        public class Message
        {
            public string role { get; set; }
            public string text { get; set; }
        }

        public class CompletionOptions
        {
            public bool stream { get; set; }
            public double temperature { get; set; }
            public int maxTokens { get; set; }
        }
    }
}
