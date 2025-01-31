﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Configuration
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt"; // Matches the section in appsettings.json

        public string Secret { get; set; } // Secret key for signing the token
        public string Issuer { get; set; } // Token issuer
        public string Audience { get; set; } // Token audience
        public int ExpiryMinutes { get; set; } // Token expiration time
    }
}
