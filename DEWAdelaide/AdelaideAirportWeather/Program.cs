using CommonHelper;

namespace AdealaideAirportArea
{
    public class Program
    {
        public string AverageTemperature()
        {
            string avgtemp = "";
            string WMO = "94672";
            try
            {
                JsonCaller caller = new JsonCaller("", "GET", WMO);
                caller.SendRequest();
                if (caller != null && caller.ResponseData != null)
                {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                    WeatherData weatherData = (WeatherData)caller.ResponseData;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
                    if (weatherData != null && weatherData.observations != null && weatherData.observations.data != null && weatherData.observations.header != null)
                    {
                        List<Datum> dataObservation = new List<Datum>();
                        DateTime timestamp72HoursAgo = DateTime.Now.AddHours(-72);
                        dataObservation = weatherData.observations.data.Where(x => DateFormat(x.local_date_time_full) >= timestamp72HoursAgo).ToList();
                        if (dataObservation != null && dataObservation.Count > 0)
                        {
                            double sum = dataObservation.Sum(reading => reading.air_temp);
                            avgtemp = Convert.ToString(sum / dataObservation.Count);
                        }
                    }
                    else
                    {
                        avgtemp = "Recieved empty response from weather station";
                    }
                }
            }
            catch
            {
                throw new Exception("Recieved error response from the weather station");
            }
            return avgtemp;
        }
        protected DateTime DateFormat(string dateString)
        {
            // Define the format of the input string
            string format = "yyyyMMddHHmmss";
            DateTime dataresult = DateTime.Now;
            // Parse the string to a DateTime object using the specified format
            if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                dataresult = result;
            }
            return dataresult;
        }
               
        static void Main(string[] args)
        {
            var temperature = new Program().AverageTemperature();
            Console.WriteLine("Adelaide airport average temperature for the previous 72 hours "+temperature);
        }
    }
}
    
