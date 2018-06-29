using DarkSky.Api;
using DarkSky.Api.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Wind.Api.Models;
using Wind.Api.Services;

namespace Wind.Api.Workers
{
  public interface IDarkApiWorker : IWorker
  { }

  public class DarkApiWorker : BaseWorker, IDarkApiWorker
  {
    private ITimeMashineService _timeMashineService;

    private IApiCallsTrackerService _apiCallsTrackerService;

    private IPointService _pointService;
    private IWindDayService _windDayService;
    private IWindHourService _windHourService;

    private static readonly DateTime START_FROM_DATE = new DateTime(2018, 03, 30, 0, 0, 0, DateTimeKind.Utc);
    private static readonly TimeSpan ONE_DAY = new TimeSpan(24, 0, 0);

    public DarkApiWorker(
        ITimeMashineService timeMashineService,
        IApiCallsTrackerService apiCallsTrackerService,
        IPointService pointService,
        IWindDayService windDayService,
        IWindHourService windHourService
    )
    {
      _timeMashineService = timeMashineService;
      _apiCallsTrackerService = apiCallsTrackerService;

      _pointService = pointService;
      _windDayService = windDayService;
      _windHourService = windHourService;
    }

    protected override async Task Main()
    {
      var canMakeApi = await _apiCallsTrackerService.CanTodayMakeApiCall();

      if (!canMakeApi)
      {
        await Task.Delay(ONE_DAY.Milliseconds);
      }

      var points = await _pointService.GetAll();
      var lastWindDay = await _windDayService.GetLastOrNull();
      var lastWindDayDate = lastWindDay?.Date ?? START_FROM_DATE.Date;

      var pointsToSkip = 0;
      if (lastWindDay != null)
      {
        var lastWindDayPoint = points.First(x => x.Id == lastWindDay.PointId);
        pointsToSkip = points.IndexOf(lastWindDayPoint) + 1;
      }

      if (pointsToSkip == points.Count)
      {
        pointsToSkip = 0;
        lastWindDayDate = lastWindDayDate.Subtract(ONE_DAY);
      }

      foreach (var point in points.Skip(pointsToSkip))
      {
        canMakeApi = await _apiCallsTrackerService.CanTodayMakeApiCall();

        if (!canMakeApi) break;

        var historicalData = await _timeMashineService.GetHistoricalWeather(point.Latitude, point.Longitude, lastWindDayDate);
        await _apiCallsTrackerService.TrackApiCall();

        var windDay = MapWindDay(historicalData.Currently, lastWindDayDate, point);
        windDay = await _windDayService.Add(windDay);

        var windHours = historicalData.Hourly.Data.Select(x => MapWindHour(x, windDay)).ToList();
        await _windHourService.AddMany(windHours);
      }
    }

    protected WindDay MapWindDay(DataPoint dataPoint, DateTime forDate, Point p)
    {
      return new WindDay
      {
        Date = forDate,
        Humidity = dataPoint.Humidity,
        PointId = p.Id,
        Pressure = dataPoint.Pressure,
        Temperature = dataPoint.Temperature,
        WindBearing = dataPoint.WindBearing,
        WindGust = dataPoint.WindGust,
        WindSpeed = dataPoint.WindSpeed,
      };
    }

    protected WindHour MapWindHour(DataPoint dataPoint, WindDay forDay)
    {
      return new WindHour
      {
        Date = DateTimeOffset.FromUnixTimeSeconds(dataPoint.Time).LocalDateTime,
        Humidity = dataPoint.Humidity,
        Pressure = dataPoint.Pressure,
        Temperature = dataPoint.Temperature,
        WindBearing = dataPoint.WindBearing,
        WindGust = dataPoint.WindGust,
        WindSpeed = dataPoint.WindSpeed,
        WindDayId = forDay.Id
      };
    }
  }
}
