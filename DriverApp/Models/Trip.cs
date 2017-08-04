using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string DriverId { get; set; }
        public DateTime AvailableFromTime { get; set; }
        public DateTime AvailableTillTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public Decimal TotalDistanceInKm { get; set; }
        public int TotalDurationInSec { get; set; }
    }
}
