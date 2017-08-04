using DriverApp.Dtos.CloudDtos;
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

			TriggerResponse trip = null;

			try
			{
				TriggerRequest triggerRequest = new TriggerRequest
				{
					RequestReference = 1,
					RequestParameters = new List<RequestParameter> { new RequestParameter { Name = "command", Value = "single-route" } },
					Data = new RequestData
					{
						Addresses = new List<Address> { new Address { Lat = 44.0121f, Long = 23.1393f, Id = "depot" } },
						Depots = new List<Depot> { new Depot { AddressId = "depot", Id = 1 } },
						Routes = new List<Route> { new Route { Id = 1 } }
					}
				};

				foreach (var order in orders)
				{
					triggerRequest.Data.Addresses.Add(new Address { Lat = order.GeoX, Long = order.GeoY, Id = order.Id.ToString() });
					triggerRequest.Data.Orders.Add(new RequestOrder
					{
						TimeWindowTill = order.TimeWindowTill,
						TimeWindowFrom = order.TimeWindowFrom,
						FixedDurationInSec = order.FixedDurationInSec,
						AddressId = order.Id.ToString(),
						Type = order.OrderType,
						Id = Int32.Parse(order.OrderNumber)
					});
				}

				StringContent content = new StringContent(JsonConvert.SerializeObject(triggerRequest), Encoding.UTF8, "application/json");
				var response = await _client.PostAsync($"/api/v1/routing?key={_key}&profile={_routingProfile}&async=false", content);
				trip = JsonConvert.DeserializeObject<TriggerResponse>(response.Content.ReadAsStringAsync().Result);
			}
			catch (HttpRequestException httpRequestException)
			{
				_logger.LogInformation($"Error getting route from OrtecCloud: {httpRequestException.Message}");
			}

			return trip;
		}

	}
        
}
