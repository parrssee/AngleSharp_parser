namespace WebApplication1.ParserCore.Comfy
{
    class ComfyParserSettings : IParserSettings
    {
        public string BaseUrl { get; set; }

        public ComfyParserSettings(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
    }
}