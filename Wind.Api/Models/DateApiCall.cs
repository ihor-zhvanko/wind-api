using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.Api.Models
{
    [Table("date_api_call")]
    public class DateApiCall
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("total")]
        public int Total { get; set; }
    }
}
