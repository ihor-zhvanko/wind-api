using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wind.Api.Models;

namespace Wind.Api.Services
{
    public class BaseService
    {
        protected WindDbContext _dbContext;

        public BaseService(WindDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
