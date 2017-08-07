using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Models
{
    public class Manager
    {
        [Key]
        [MaxLength(4)]
        public string CustomerKey { get; set; }

        public ICollection<Driver> Drivers { get; set; }
    }
}
