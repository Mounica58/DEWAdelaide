using CommonHelper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AdelaideWeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherByWMO")]
        public WeatherData Get(string WMO,string data="")
        {
            WeatherData weatherData = new WeatherData();
            JsonCaller caller = new JsonCaller("", "GET", WMO);
            caller.SendRequest();
            if(caller != null && caller.ResponseData != null)
            {
                 weatherData = (WeatherData)caller.ResponseData;
                if (!string.IsNullOrEmpty(data))
                {
                    List<double> dewpoint = weatherData.observations.data.Select(x => x.dewpt).ToList();
                    var filteredObservations = weatherData.observations.data.Select(x => new Datum
                    {
                        dewpt = x.dewpt,
                    });
                }   
            }
            return weatherData;
        }
    }
}