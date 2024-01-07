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
                _logger.LogError("Recieved empty response from the weather station");
            }
            return weatherData;
        }
        public enum StringOptions
        {
            temp,
            apptemp,
            dewpoint
        }
        [HttpGet("GetSpecificStateWeather")]
        public string GetSpecificWeatherData(string WMO, [SwaggerParameter("Dropdown parameter", Required = true)]
            [EnumDataType(typeof(StringOptions), ErrorMessage = "Invalid option.")] string stringValue)
        {
            WeatherData weatherData = new WeatherData();
            JsonCaller caller = new JsonCaller("", "GET", WMO);
            caller.SendRequest();
            if (caller != null && caller.ResponseData != null)
            {
                weatherData = (WeatherData)caller.ResponseData;
                var filteredObservations = weatherData?.observations?.data?.FirstOrDefault();
                if (filteredObservations != null && !string.IsNullOrEmpty(stringValue))
                {
                    switch (stringValue)
                    {
                        case "temp":
                            return  Convert.ToString(filteredObservations.air_temp);
                        case "apptemp":
                            return Convert.ToString(filteredObservations.apparent_t);
                        case "dewpoint":
                            return Convert.ToString(filteredObservations.dewpt);
                        default:
                            return string.Empty;
                    }
                }
                else
                {
                    return "Recieved empty response from the weather station";
                }
            }
            else
            {
                _logger.LogError("Recieved empty response from the weather station");
                return "Recieved empty response from the weather station";
            }
        }
    }
}