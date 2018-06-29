using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.Api.Models
{
    public class WindDbContext : DbContext
    {
        public DbSet<Point> Points { get; set; }

        public DbSet<WindDay> WindDay { get; set; }

        public DbSet<WindHour> WindHour { get; set; }

        public DbSet<DateApiCall> DateApiCall { get; set; }

        public WindDbContext(DbContextOptions options) : base(options) { }
        
    }
}
