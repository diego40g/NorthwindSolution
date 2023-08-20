﻿namespace Northwind.WebApi.Models
{
    public class CorsSettings
    {
        public string[] AllowedOrigins { get; set; }
        public string[] AllowedHeaders { get; set; }
        public string[] AllowedMethods { get; set; }
    }
}
