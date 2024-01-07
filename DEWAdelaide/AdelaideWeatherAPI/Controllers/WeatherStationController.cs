using CommonHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AdelaideWeatherAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherStationController : ControllerBase
    {
        private readonly ILogger<WeatherStationController> _logger;

        public WeatherStationController(ILogger<WeatherStationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetStateWeatherByWMO")]
        public WeatherData GetStateWeather(string WMO)
        {
            WeatherData weatherData = new WeatherData();
            JsonCaller caller = new JsonCaller("", "GET", WMO);
            caller.SendRequest();
            if(caller != null && caller.ResponseData != null)
            {
                 weatherData = (WeatherData)caller.ResponseData;
            }
            else
            {
                _logger.LogError("Received empty response from the weather station");
            }
            return weatherData;
        }
       
        [HttpGet("GetSpecificStateWeather")]
        public string GetSpecificWeatherData(string WMO, [SwaggerParameter("Dropdown parameter", Required = true)]
            [EnumDataType(typeof(DataOptions), ErrorMessage = "Invalid option.")] string specificData)
        {
            WeatherData weatherData = new WeatherData();
            JsonCaller caller = new JsonCaller("", "GET", WMO);
            caller.SendRequest();
            if (caller != null && caller.ResponseData != null)
            {
                weatherData = (WeatherData)caller.ResponseData;
                var dataObservations = weatherData?.observations?.data?.FirstOrDefault();
                if (dataObservations != null && !string.IsNullOrEmpty(specificData))
                {
                    switch (specificData)
                    {
                        case "temp":
                            return  Convert.ToString(dataObservations.air_temp);
                        case "apptemp":
                            return Convert.ToString(dataObservations.apparent_t);
                        case "dewpoint":
                            return Convert.ToString(dataObservations.dewpt);
                        default:
                            return string.Empty;
                    }
                }
                else
                {
                    return "Received empty response from the weather station";
                }
            }
            else
            {
                _logger.LogError("Received empty response from the weather station");
                return "Received empty response from the weather station";
            }
        }
    }
}