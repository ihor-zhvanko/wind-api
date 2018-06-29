using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using DarkSky.Api.Models;
using Microsoft.Extensions.Options;

namespace DarkSky.Api
{
	public interface ITimeMashineService
	{
		Task<TimeMashineResponse> GetHistoricalWeather(double latitude, double longitude, DateTime time);
	}

	public class TimeMashineService : BaseService, ITimeMashineService
	{
		public TimeMashineService(IOptions<DarkSkySettings> emailSettings) : base(emailSettings)
		{
		}

        protected long Unix(DateTime time)
        {
            return ((DateTimeOffset)time).ToUnixTimeSeconds();
        }

		public async Task<TimeMashineResponse> GetHistoricalWeather(double latitude, double longitude, DateTime time)
		{
			var valueCol = new NameValueCollection();
			var res = Get<TimeMashineResponse>($"{latitude},{longitude},{Unix(time)}", valueCol);
			return await res;
		}
	}
}
