using back_end.DTOs;
using back_end.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetWeaterByCity(string cityName="auckland")
        {
            var weather = await _weatherService.GetWeatherByCity(cityName);

            return Ok(weather);
        }
    }
}

