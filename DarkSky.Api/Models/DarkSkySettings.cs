using System;
namespace DarkSky.Api.Models
{
	public class DarkSkySettings
	{
		public string BaseUrlTemplate { get; set; }
		public string SecretKey { get; set; }
        public int MaxRequestPerDay { get; set; }
    }
}
