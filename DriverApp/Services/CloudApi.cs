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
	}
}
