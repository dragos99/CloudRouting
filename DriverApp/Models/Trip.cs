using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int DriverId { get; set; }
        public string AvailableFromTime { get; set; }
        public string AvailableTillTime { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public float TotalDistanceInKm { get; set; }
        public int TotalDurationInSec { get; set; }

        /*public Trip()
        {
            Id = 0;
            AccountId = 0;
            DriverId = 0;
            AvailableFromTime = "0";
            AvailableTillTime = "0";
            StartTime = "0";
            FinishTime = "0";
            TotalDistanceInKm = 0f;
            TotalDurationInSec = 0;
        }*/
    }
}
