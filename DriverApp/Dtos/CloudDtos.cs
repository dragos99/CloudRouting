using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Dtos.CloudDtos
{
	public class CloudRoutingRequest
	{
		public string RequestReference;
		public Object[] RequestParameters = new Object[1] { new { Name = "command", Value = "singleRoute" } };
		public CloudRoutingRequestData Data;
		public Object[] Depots = new Object[1] { new { AddressId = "depot", Id = "depot1" } };
		public Order[] Orders;

	}

	public struct CloudRoutingRequestData
	{
		public Address[] Addresses;
	}

	public struct Address
	{
		public float Lat;
		public float Long;
		public int Id;
	}

	public struct Order
	{
		public string TimeWindowTill;
		public string TimeWindowFrom;
		public int FixedDurationInSec;
		public int AddressId;
		public string Type;
		public int Id;
	}
}
