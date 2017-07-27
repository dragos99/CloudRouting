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
		public float GivenY;
		public float GeoX;
		public float GeoY;
    }
}
