using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DriverApp.Dtos.CloudDtos
{
	public class TriggerRequest
	{
		public int RequestReference { get; set; }
		public List<RequestParameter> RequestParameters { get; set; }
		public RequestData Data { get; set; }
	}

	public class RequestParameter
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class RequestData
	{
		public List<Address> Addresses { get; set; }
		public List<Depot> Depots { get; set; }
		public List<RequestOrder> Orders { get; set; }
		public List<Route> Routes { get; set; }

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
		public float Lat { get; set; }
		public float Long { get; set; }
		public string Id { get; set; }
	}

	public class Depot
	{
		public string AddressId { get; set; }
		public string Id { get; set; }
	}

	public class RequestOrder
	{
		public string TimeWindowTill { get; set; }
		public string TimeWindowFrom { get; set; }
		public int FixedDurationInSec { get; set; }
		public string AddressId { get; set; }
		public string Type { get; set; }
		public int Id { get; set; }
	}

	public class Route
	{
		public List<Stop> Stops { get; set; }
		public string Id { get; set; }
		public string StartDateTime { get; set; }
		public string FinishDateTime { get; set; }
		public int DurationInSec { get; set; }
		public int DrivingTimeInSec { get; set; }
		public float Distance { get; set; }
		public int WaitingTimeInSec { get; set; }
		public int NofStops { get; set; }

		public Route()
		{
			Stops = new List<Stop>();
		}
	}

	public class Stop
	{
		public string AddressId { get; set; }
		public int StopSequence { get; set; }
		public string Type { get; set; }
		public int DrivingTimeInSec { get; set; }
		public float Distance { get; set; }
		public int WaitingTimeInsec { get; set; }
		public int DurationInSec { get; set; }
		public string ArrivalDateTime { get; set; }
		public string DepartureDateTime { get; set; }
	}

	public class TriggerResponse
	{
		public int RequestReference { get; set; }
		public Tracking TrackingData { get; set; }
		public Output OutputPlan { get; set; }
		public NotPlanned NotPlannedOrders { get; set; }
	}

	public class Tracking
	{
		public string ServerTrackingId { get; set; }
	}

	public class Output
	{
		public List<Route> Routes { get; set; }
	}

	public class NotPlanned
	{
		public int NofOrders { get; set; }
	}
}

