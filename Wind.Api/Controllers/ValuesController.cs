using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkSky.Api;
using Microsoft.AspNetCore.Mvc;
using Wind.Api.Models;
using Wind.Api.Services;

namespace Wind.Api.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    private ITimeMashineService _service;
    private IApiCallsTrackerService _apiCallsTrackerService;
    private IWindDayService _windDayService;

    public ValuesController(
        ITimeMashineService service,
        IApiCallsTrackerService apiCallsTrackerService,
        IWindDayService windDayService
    )
    {
      _service = service;
      _apiCallsTrackerService = apiCallsTrackerService;
      _windDayService = windDayService;
    }

    // GET api/values
    [HttpGet]
    public async Task<WindDayStats> Get()
    {
      await Task.Delay(0);

      return await _windDayService.GetWindDayStats(49.834939, 24.014399, new DateTime(2018, 01, 01), DateTime.Now);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
