using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.Api.Models
{
    [Table("wind_hour")]
    public class WindHour
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("wind_day_id")]
        public int WindDayId { get; set; }

        [Column("temperature")]
        public double? Temperature { get; set; }

        [Column("humidity")]
        public double Humidity { get; set; }

        [Column("pressure")]
        public double Pressure { get; set; }

        [Column("wind_bearing")]
        public int WindBearing { get; set; }

        [Column("wind_gust")]
        public double WindGust { get; set; }

        [Column("wind_speed")]
        public double WindSpeed { get; set; }
        
        public WindDay WindDay { get; set; }
    }
}
