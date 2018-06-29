using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wind.Api.Services;
using Wind.Api.Helpers;
using Wind.Api.Helpers.Models;

namespace Wind.Api.Models
{
  public class WindBearingStats
  {
    public int Degree { get; set; }
    public int Count { get; set; }
  }

  public class WindDayStats
  {
    public double AvgTemperature { get; set; }

    public double MinTemperature { get; set; }

    public double MaxTemperature { get; set; }

    public double AvgHumidity { get; set; }

    public double AvgPressure { get; set; }

    public IList<WindBearingStats> WindRose { get; set; }

    public double AvgWindSpeed { get; set; }

    public static async Task<WindDayStats> Create(IList<IAsyncGrouping<DateTime, WindDay>> groupedWindDays, Point[] points, Point target)
    {
      var interpolatedWindDays = await Task.WhenAll(groupedWindDays.Select(async x =>
      {
        if (await x.Count() != 4)
        {
          return null;
          //throw new Exception("Should be four points for interpolation");
        }

        var windVector = await InterpolateWind(x, points, target);
        return new WindDay
        {
          Date = x.Key,
          Temperature = await InterpolateTemperature(x, points, target),
          Humidity = await InterpolateHumidity(x, points, target),
          WindSpeed = windVector.Length,
          WindBearing = (int?)Math.Round(windVector.Angle * 180 / Math.PI),
          Pressure = await InterpolatePressure(x, points, target)
        };
      }));

      interpolatedWindDays = interpolatedWindDays.Where(x => x != null).ToArray();

      var windRose = interpolatedWindDays
        .Where(x => x.WindSpeed != 0)
        .GroupBy(x => x.WindBearing)
        .Select(x => new WindBearingStats
        {
          Degree = x.Key ?? 0,
          Count = x.Count()
        }).ToList();

      return new WindDayStats
      {
        AvgHumidity = interpolatedWindDays.Average(x => x.Humidity) ?? 0,
        AvgPressure = interpolatedWindDays.Average(x => x.Pressure) ?? 0,
        AvgTemperature = interpolatedWindDays.Average(x => x.Temperature) ?? 0,
        MinTemperature = interpolatedWindDays.Min(x => x.Temperature) ?? 0,
        MaxTemperature = interpolatedWindDays.Max(x => x.Temperature) ?? 0,
        AvgWindSpeed = interpolatedWindDays.Average(x => x.WindSpeed) ?? 0,
        WindRose = windRose
      };
    }

    private static async Task<Vector> InterpolateWind(IAsyncGrouping<DateTime, WindDay> windDays, Point[] points, Point target)
    {
      var windVectors = await windDays.Select(x => new InterpolationNode<Vector>
      {
        Arg = points.First(p => p.Id == x.PointId),
        Value = new Vector(x.WindBearing ?? 0 + 180).Multiply(x.WindSpeed ?? 0)
      }).ToArray();

      return InterpolationHelper.BilinearInterpolation(windVectors, target);
    }

    private static async Task<double> InterpolateTemperature(IAsyncGrouping<DateTime, WindDay> windDays, Point[] points, Point target)
    {
      var temps = await windDays.Select(x => new InterpolationNode<double>
      {
        Arg = points.First(p => p.Id == x.PointId),
        Value = x.Temperature ?? 0
      }).ToArray();

      return InterpolationHelper.BilinearInterpolation(temps, target);
    }

    private static async Task<double> InterpolateHumidity(IAsyncGrouping<DateTime, WindDay> windDays, Point[] points, Point target)
    {
      var humidities = await windDays.Select(x => new InterpolationNode<double>
      {
        Arg = points.First(p => p.Id == x.PointId),
        Value = x.Humidity ?? 0
      }).ToArray();

      return InterpolationHelper.BilinearInterpolation(humidities, target);
    }

    private static async Task<double> InterpolatePressure(IAsyncGrouping<DateTime, WindDay> windDays, Point[] points, Point target)
    {
      var pressures = await windDays.Select(x => new InterpolationNode<double>
      {
        Arg = points.First(p => p.Id == x.PointId),
        Value = x.Pressure ?? 0
      }).ToArray();

      return InterpolationHelper.BilinearInterpolation(pressures, target);
    }
  }


}