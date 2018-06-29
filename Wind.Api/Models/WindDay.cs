using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.Api.Models
{
    [Table("wind_day")]
    public class WindDay
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("point_id")]
        public int PointId { get; set; }

        [Column("temperature")]
        public double? Temperature { get; set; }

        [Column("humidity")]
        public double? Humidity { get; set; }

        [Column("pressure")]
        public double? Pressure { get; set; }

        [Column("wind_bearing")]
        public int? WindBearing { get; set; }

        [Column("wind_gust")]
        public double? WindGust { get; set; }

        [Column("wind_speed")]
        public double? WindSpeed { get; set; }
        
        public Point Point { get; set; }

        public List<WindHour> WindHours { get; set; }
    }
}
