namespace WebApplication1.ParserCore.Rozetka
{
    class RozetkaParserSettings : IParserSettings
    {
        public string BaseUrl { get; set; }

        public RozetkaParserSettings(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
    }
}