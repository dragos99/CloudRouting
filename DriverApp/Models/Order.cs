using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Models
{
    public class Order
    {
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public string StreetName { get; set; }
		public string StreetNumber { get; set; }
		public string CityName { get; set; }
		public string CountryCode { get; set; }
		public float GivenX { get; set; }
		public float GivenY { get; set; }
		public float GeoX { get; set; }
		public float GeoY { get; set; }
        public string OrderType { get; set; }
        public int FixedDurationInSec { get; set; }
        public string TimeWindowFrom { get; set; }
        public string TimeWindowTo { get; set; }
        public int AccountId { get; set; }
        public int DriverId { get; set; }
        public int TripId { get; set; }
        public string StopStartTime { get; set; }
        public string StopFinishTime { get; set; }
        public int StopDurationInSec { get; set; }
        public bool Complete { get; set; }
        public string Comment { get; set; }
    }
}
