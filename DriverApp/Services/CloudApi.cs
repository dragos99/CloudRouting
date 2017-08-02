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

		public async Task GetRouteAsync()
		{
			try
			{
				StringContent content = new StringContent("{\"message\": \"Hello Ortec\"}", Encoding.UTF8, "application/json");
				var response = await _client.PostAsync($"/api/v1/routing?key={_key}&profile={_routingProfile}&async=false", content);
				response.EnsureSuccessStatusCode();

				var stringResult = await response.Content.ReadAsStringAsync();
				_logger.LogInformation(stringResult);
			}
			catch (HttpRequestException e)
			{
				_logger.LogInformation($"Error getting route from OrtecCloud: {e.Message}");
			}
		}
	}
}
