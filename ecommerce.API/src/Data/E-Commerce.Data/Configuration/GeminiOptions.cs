namespace E_Commerce.Data.Configuration
{
    public class GeminiOptions
    {

        public const string SectionName = "Gemini"; // Matches the section in appsettings.json
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
    }
}
