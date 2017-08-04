using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Dtos.CloudDtos
{
	public class TriggerRequest
	{
		public int RequestReference;
		public List<RequestParameter> RequestParameters;
		public RequestData Data;
	}

	public class RequestParameter
	{
		public string Name;
		public string Value;
	}

	public class RequestData
	{
		public List<Address> Addresses;
		public List<Depot> Depots;
		public List<RequestOrder> Orders;
		public List<Route> Routes;

		public RequestData()
		{
			Addresses = new List<Address>();
			Depots = new List<Depot>();
			Orders = new List<RequestOrder>();
			Routes = new List<Route>();
		}
	}

	public class Address
	{
		public float Lat;
		public float Long;
		public string Id;
	}

	public class Depot
	{
		public string AddressId;
		public int Id;
	}

	public class RequestOrder
	{
		public string TimeWindowTill;
		public string TimeWindowFrom;
		public int FixedDurationInSec;
		public string AddressId;
		public string Type;
		public int Id;
	}

	public class TriggerResponse
	{
		public int RequestReference;
		public TrackingData TrackingData;
		public OutputPlan OutputPlan;
		public NotPlannedOrders NotPlannedOrders;
	}

	public class TrackingData
	{
		public string ServerTrackingId;
	}

	public class OutputPlan
	{
        public List<Route> Routes;
	}

	public class NotPlannedOrders
	{
		public int NOfOrders;
		public List<NotPlannedOrder> Order;
	}

	public class NotPlannedOrder {
		public int Id;
	}

	public class Route
	{
		public List<Stop> Stops;
		public int Id;
		public string StartDateTime;
		public string FinishDateTime;
		public int DurationInSec;
		public int DrivingTimeInSec;
		public Decimal Distance;
		public int WaitingTimeInSec;
		public int NofStops;
	}

	public class Stop
	{
		public string AddressId;
		public int StopSequence;
		public string Type;
		public int DrivingTimeInSec;
		public float Distance;
		public int WaitingTimeInsec;
		public int DurationInSec;
		public string ArrivalDateTime;
		public string DepartureDateTime;
	}
}

