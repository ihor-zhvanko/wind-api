using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wind.Api.Models;
using Wind.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wind.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/stats")]
  public class StatsController : Controller
  {
    private IWindDayService _windDayService;

    public StatsController(
        IWindDayService windDayService
    )
    {
      _windDayService = windDayService;
    }
    // GET: api/<controller>
    [HttpGet]
    public async Task<WindDayStats> Get(double lat, double lng, DateTime from, DateTime to)
    {
      return await _windDayService.GetWindDayStats(lat, lng, from, to);
    }
  }
}
