using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.Api.Models
{
    [Table("point")]
    public class Point
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("lat")]
        public double Latitude { get; set; }

        [Column("lng")]
        public double Longitude { get; set; }

        public List<WindDay> WindDays { get; set; }
    }
}
