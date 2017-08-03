using DriverApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DriverApp.Services
{
    public class CloudApi
    {
		private ILogger _logger;
		private string _key = "311A1B20704D4EA797EE6E9B1D618999";
		private string _routingProfile = "R-EUR-001";
		private HttpClient _client;

		public CloudApi(ILoggerFactory fact)
		{
			_logger = fact.CreateLogger("CloudApiLogger");
			_client = new HttpClient();
			_client.BaseAddress = new Uri("https://test.orteccloudservices.com");
		}
        public async Task<TriggerResponse> TriggerRouting(IEnumerable<Order> orders)
        {
            var client = new HttpClient();
            TriggerResponse trip = null;
            try
            {
                client.BaseAddress = new Uri("https://test.orteccloudservices.com");
                /*        */
                // Creating the Request
                TriggerRequest triggerRequest = new TriggerRequest();
                triggerRequest.RequestReference = 1;
                triggerRequest.RequestParameters.Add(new Parameter { Name = "command", Value = "single-route" });
                triggerRequest.Data.Addresses.Add(new Address { Lat = 44.0121f, Long = 23.1393f, Id = "depot" });
                triggerRequest.Data.Depots.Add(new Depot { AddressId = "depot", Id = "depot1" });
                triggerRequest.Data.Routes.Add(new Route { Id = "1" });
                foreach (var order in orders)
                {
                    triggerRequest.Data.Addresses.Add(new Address { Lat = order.GeoX, Long = order.GeoY, Id = "" + order.Id });
                    triggerRequest.Data.Orders.Add(new RequestOrder { TimeWindowTill = order.TimeWindowTill, TimeWindowFrom = order.TimeWindowFrom, FixedDurationInSec = order.FixedDurationInSec, AddressId = "" + order.Id, Type = order.OrderType, Id = Int32.Parse(order.OrderNumber) });
                }
                /*        */

                StringContent content = new StringContent(JsonConvert.SerializeObject(triggerRequest), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"/api/v1/routing?key={_key}&profile={_routingProfile}&async=false", content);
                trip = JsonConvert.DeserializeObject<TriggerResponse>(response.Content.ReadAsStringAsync().Result);
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogInformation($"Error getting route from OrtecCloud: {httpRequestException.Message}");
            }
            return trip;
        }
        public class TriggerRequest
        {
            public int RequestReference { get; set; }
            public List<Parameter> RequestParameters { get; set; }
            public RequestData Data { get; set; }

            public TriggerRequest()
            {
                RequestParameters = new List<Parameter>();
                Data = new RequestData();
            }
        }
        public class Parameter
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
        }
        public class Tracking
        {
            public string ServerTrackingId { get; set; }
        }
        public class Output
        {
            public List<Route> Routes { get; set; }
            public NotPlanned NotPlannedOrders { get; set; }

            public Output()
            {
                Routes = new List<Route>();
            }
        }
        public class NotPlanned
        {
            public int NofOrders { get; set; }
        }
    }
}
