using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Configuration
{
    public class GeminiOptions
    {

        public const string SectionName = "Gemini"; // Matches the section in appsettings.json
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
    }
}
