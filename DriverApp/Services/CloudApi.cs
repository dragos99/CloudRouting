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

		public CloudApi(ILoggerFactory fact)
		{
			_logger = fact.CreateLogger("CloudApiLogger");
		}

		public async Task GetRouteAsync()
		{
			var client = new HttpClient();
			try
			{
				StringContent content = new StringContent("{\"message\": \"Hello Ortec\"}", Encoding.UTF8, "application/json");

				client.BaseAddress = new Uri("https://test.orteccloudservices.com");
				var response = await client.PostAsync($"/api/v1/routing?key={_key}&profile={_routingProfile}&async=false", content);
				response.EnsureSuccessStatusCode();

				var stringResult = await response.Content.ReadAsStringAsync();
				_logger.LogInformation(stringResult);
			}
			catch (HttpRequestException httpRequestException)
			{
				_logger.LogInformation($"Error getting route from OrtecCloud: {httpRequestException.Message}");
			}
		}
        public string TriggerRouting(TriggerRequest triggerRequest)
        {
            var result = xTriggerRouting(triggerRequest).Result;
            _logger.LogInformation("The result is {0}", result);
            return result;
        }
        public async Task<string> xTriggerRouting(TriggerRequest triggerRequest)
        {
            var client = new HttpClient();
            string stringResult = null;
            try
            {
                //StringContent content = new StringContent("{ \"requestReference\": \"13243\", \"requestParameters\": [{ \"name\": \"command\", \"value\": \"single-route\" }], \"data\": { \"addresses\": [{ \"lat\": 50.140964, \"long\": 8.662275, \"id\": \"886627\" }, { \"lat\": 50.167130, \"long\": 8.679496, \"id\": \"1867807\" }, { \"lat\": 50.133628, \"long\": 8.671028, \"id\": \"3038836\" }, { \"lat\": 50.118740, \"long\": 8.699856, \"id\": \"depot\" }], \"depots\": [{ \"addressId\": \"depot\", \"id\": \"depot1\" }], \"orders\": [{ \"timeWindowTill\": \"2015-04-14T15:00:00\", \"timeWindowFrom\": \"2015-04-14T11:00:00\", \"fixedDurationInSec\": 300, \"addressId\": \"886627\", \"type\": \"delivery\", \"id\": \"9436340\" }, { \"timeWindowTill\": \"2015-04-14T15:00:00\", \"timeWindowFrom\": \"2015-04-14T11:00:00\", \"fixedDurationInSec\": 600, \"addressId\": \"1867807\", \"type\": \"delivery\", \"id\": \"9436343\" }, { \"timeWindowTill\": \"2015-04-14T15:00:00\", \"timeWindowFrom\": \"2015-04-14T11:00:00\", \"fixedDurationInSec\": 300, \"addressId\": \"3038836\", \"type\": \"delivery\", \"id\": \"9436347\" }], \"routes\": [{ \"id\": \"302901\" }] } }", Encoding.UTF8, "application/json");
                var json = JsonConvert.SerializeObject(triggerRequest);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                _logger.LogInformation("The serialized object is {0}", JsonConvert.SerializeObject(triggerRequest));

                client.BaseAddress = new Uri("https://test.orteccloudservices.com");
                var response = await client.PostAsync($"/api/v1/routing?key={_key}&profile={_routingProfile}&async=false", content);
                //response.EnsureSuccessStatusCode();

                stringResult = response.Content.ReadAsStringAsync().Result;
                _logger.LogInformation(stringResult);
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogInformation($"Error getting route from OrtecCloud: {httpRequestException.Message}");
            }
            return stringResult;
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
            public double Lat { get; set; }
            public double Long { get; set; }
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
            public string Id { get; set; }
        }
    }
}
