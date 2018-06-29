using System;
namespace DarkSky.Api.Models
{
	public class TimeMashineResponse
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DataPoint Currently { get; set; }
		public DataBlock Hourly { get; set; }
	}
}
