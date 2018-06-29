using DarkSky.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wind.Api.Models;

namespace Wind.Api.Services
{
    public interface IApiCallsTrackerService
    {
        Task TrackApiCall();
        Task<bool> CanTodayMakeApiCall();
    }

    public class ApiCallsTrackerService : BaseService, IApiCallsTrackerService
    {
        private int _maxApiCalls;

        public ApiCallsTrackerService(WindDbContext dbContext, IOptions<DarkSkySettings> darkSkySettings):
            base(dbContext)
        {
            _maxApiCalls = darkSkySettings.Value.MaxRequestPerDay;
        }

        public async Task TrackApiCall()
        {
            var todayApiCalls = _dbContext.DateApiCall.Where(x => x.Date == DateTime.Now.Date).FirstOrDefault();
            if(todayApiCalls == null)
            {
                todayApiCalls = new DateApiCall()
                {
                    Date = DateTime.Now.Date,
                    Total = 0
                };
                todayApiCalls.Total += 1;
                await _dbContext.AddAsync(todayApiCalls);
            } else
            {
                if(todayApiCalls.Total >= _maxApiCalls)
                {
                    throw new Exception("Top range of calls exceeded");
                }

                todayApiCalls.Total += 1;
                _dbContext.Update(todayApiCalls);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CanTodayMakeApiCall()
        {
            return await Task.Run(() => {
                var total = _dbContext.DateApiCall.Where(x => x.Date == DateTime.Now.Date).FirstOrDefault()?.Total ?? 0;
                return total < _maxApiCalls;
            });
        }
    }
}
