using back_end.DTOs;
using Newtonsoft.Json;

namespace back_end.Services
{
    public class WeatherService
	{
        private readonly IConfiguration _config;

        public WeatherService(IConfiguration config)
		{
            _config = config;
        }

		//example city Auckland
        //https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/Auckland?unitGroup=metric&key=TWMFF6ZF67CJHQXZXBZLZR2XJ&contentType=json
        public async Task<WeatherDto?> GetWeatherByCity(string cityName)
		{
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{cityName}?unitGroup=metric&key={_config["WeaterKey"]}&contentType=json");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode(); // Throw an exception if error

            var body = await response.Content.ReadAsStringAsync();

            var weather = JsonConvert.DeserializeObject<WeatherDto>(body);

            return weather;
        }
	}
}

