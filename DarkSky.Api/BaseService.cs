using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DarkSky.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DarkSky.Api
{
	public class BaseService
	{
		private DarkSkySettings _settings;

		public BaseService(IOptions<DarkSkySettings> emailSettings)
		{
			_settings = emailSettings.Value;
		}

		protected void CreateWebClient()
		{

		}

		protected string BuildUrl(string uri)
		{
			return $"{string.Format(_settings.BaseUrlTemplate, _settings.SecretKey)}/{uri}";
		}

		public async Task<TReturn> Get<TReturn>(string uri, NameValueCollection query)
		{
			var webClient = new WebClient();
			webClient.Encoding = Encoding.UTF8;
			webClient.Headers.Add("Content-Type", "application/json");
			webClient.QueryString = query;
            
			var url = BuildUrl(uri);
			var result = await webClient.DownloadStringTaskAsync(url);

			return JsonConvert.DeserializeObject<TReturn>(result);
		}
	}
}
