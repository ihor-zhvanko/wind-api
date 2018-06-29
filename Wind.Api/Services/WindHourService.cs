using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wind.Api.Models;

namespace Wind.Api.Services
{
    public interface IWindHourService
    {
        Task AddMany(IList<WindHour> windHours);
    }

    public class WindHourService : BaseService, IWindHourService
    {
        public WindHourService(WindDbContext dbContext) : base(dbContext)
        {

        }

        public async Task AddMany(IList<WindHour> windHours)
        {
            await _dbContext.AddRangeAsync(windHours);
            await _dbContext.SaveChangesAsync();
        }
    }
}
