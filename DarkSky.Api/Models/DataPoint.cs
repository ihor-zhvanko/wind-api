using System;
namespace DarkSky.Api.Models
{
	public class DataPoint
	{
		public double Temperature { get; set; }
		public double Humidity { get; set; }
		public double Pressure { get; set; }
		public int Time { get; set; }
		public int WindBearing { get; set; }
		public double WindGust { get; set; }
		public double WindSpeed { get; set; }
	}
}
