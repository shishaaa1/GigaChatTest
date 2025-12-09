namespace YandexGptTest.Response
{
    public class YandexResponse
    {
        public ResponseResult result { get; set; }

        public class ResponseResult
        {
            public Alternative[] alternatives { get; set; }
        }

        public class Alternative
        {
            public string text { get; set; }
            public string status { get; set; }
        }
    }
}
