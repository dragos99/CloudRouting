using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Models
{
    public class Driver
    {
        [Key]
        public string DriverId { get; set; }
        public Manager Manager { get; set; }
        public bool Done { get; set; }
    }
}
