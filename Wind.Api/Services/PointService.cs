using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wind.Api.Models;

namespace Wind.Api.Services
{
    public interface IPointService
    {
        Task<IList<Point>> GetAll();
    }

    public class PointService : BaseService, IPointService
    {
        public PointService(WindDbContext dbContext)
            :base(dbContext) { }

        public async Task<IList<Point>> GetAll()
        {
            return await _dbContext.Points.ToAsyncEnumerable().ToList();
        }
    }
}
