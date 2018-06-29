using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wind.Api.Models;
using Wind.Api.Extenstions;

namespace Wind.Api.Services
{
  public interface IWindDayService
  {
    Task<IList<WindDay>> GetAll();

    Task<WindDay> GetLastOrNull();

    Task<WindDay> Add(WindDay windDay);

    Task<WindDayStats> GetWindDayStats(double lat, double lng, DateTime from, DateTime to);

    Task<Point[]> GetNeighbours(double lat, double lng);
  }

  public class WindDayService : BaseService, IWindDayService
  {
    private IPointService _pointService;

    public WindDayService(
        WindDbContext dbContext,
        IPointService pointService
    ) : base(dbContext)
    {
      _pointService = pointService;
    }

    public async Task<WindDay> Add(WindDay windDay)
    {
      await _dbContext.AddAsync(windDay);

      await _dbContext.SaveChangesAsync();

      return windDay;
    }

    public async Task<WindDay> GetLastOrNull()
    {
      return await _dbContext.WindDay.ToAsyncEnumerable().OrderBy(x => x.Id).LastOrDefault();
    }

    public async Task<IList<WindDay>> GetAll()
    {
      return await _dbContext.WindDay.ToAsyncEnumerable().ToList();
    }

    public async Task<WindDayStats> GetWindDayStats(double lat, double lng, DateTime from, DateTime to)
    {
      var neighbours = await GetNeighbours(lat, lng);
      if (neighbours.Contains(null))
      {
        throw new ArgumentException("Out of bounding set");
      }

      var windDays = await _dbContext.WindDay.ToAsyncEnumerable()
        .Where(x => x.Date >= from && x.Date <= to)
        .Where(x => neighbours.Any(y => x.PointId == y.Id))
        .GroupBy(x => x.Date)
        .ToList();

      return await WindDayStats.Create(windDays, neighbours, new Point { Latitude = lat, Longitude = lng });
    }

    public async Task<Point[]> GetNeighbours(double lat, double lng)
    {
      var points = await _pointService.GetAll();

      var firstGroup = points.Where(x => x.Latitude >= lat && x.Longitude > lng).ToList();
      var secondGroup = points.Where(x => x.Latitude > lat && x.Longitude <= lng).ToList();
      var thirdGroup = points.Where(x => x.Latitude <= lat && x.Longitude < lng).ToList();
      var forthGroup = points.Where(x => x.Latitude < lat && x.Longitude >= lng).ToList();

      var nearestPoints = new[] {
        NearestPoint(lat, lng, firstGroup),
        NearestPoint(lat, lng, secondGroup),
        NearestPoint(lat, lng, thirdGroup),
        NearestPoint(lat, lng, forthGroup)
      };

      return nearestPoints;
    }

    private Point NearestPoint(double lat, double lng, IList<Point> points)
    {
      if (points.Count == 0)
      {
        return null;
      }

      var minDistance = double.MaxValue;
      Point minPoint = null;
      foreach (var point in points)
      {
        var distance = Distance(lat, lng, point);
        if (distance <= minDistance)
        {
          minDistance = distance;
          minPoint = point;
        }
      }

      return minPoint;
    }

    private double Distance(double lat, double lng, Point point)
    {
      return Math.Sqrt(
        (lat - point.Latitude) * (lat - point.Latitude) +
        (lng - point.Longitude) * (lng - point.Longitude)
      );
    }
  }
}
