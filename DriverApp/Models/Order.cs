
namespace DriverApp.Models
{
    public class Order
    {
		public int Id { get; set; }
		public string AccountId { get; set; }
		public string DriverId { get; set; }
		public int TripId { get; set; }
		public int FixedDurationInSec { get; set; }
		public int StopDurationInSec { get; set; }
		public int StopSequence { get; set; }
		public float GivenX { get; set; }
		public float GivenY { get; set; }
		public float GeoX { get; set; }
		public float GeoY { get; set; }
		public float Distance { get; set; }
		public string OrderNumber { get; set; }
		public string StreetName { get; set; }
		public string StreetNumber { get; set; }
		public string CityName { get; set; }
		public string CountryCode { get; set; }
        public string OrderType { get; set; }
        public string TimeWindowFrom { get; set; }
        public string TimeWindowTill { get; set; }
        public string StopStartTime { get; set; }
        public string StopFinishTime { get; set; }
        public string Comment { get; set; }
		public string ArrivalDateTime { get; set; }
		public string DepartureDateTime { get; set; }
		public bool	Complete { get; set; }
    }
}
