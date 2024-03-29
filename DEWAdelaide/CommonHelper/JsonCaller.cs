﻿
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace CommonHelper
{
    public class JsonCaller
    {
        #region Properties
        public string URL { get; set; }
        public object ResponseData { set; get; }
        public string Method { get; set; }
        public string WMO { get; set; }
        #endregion

        #region Constructors

        public JsonCaller(string url, string method, string wMO)
        {
            this.URL = url;
            this.Method = method;
            this.WMO = wMO;
        }
        #endregion

        #region Public Methods
        public void SendRequest()
        {
            try
            {
                URL = "http://www.bom.gov.au/fwo/IDS60901/IDS60901." +WMO+".json";

#pragma warning disable IDE0063 // Use simple 'using' statement
                using (WebClient client = new WebClient())
                {
                    var response = client.DownloadString(URL);
                    if (response != null)
                    {
#pragma warning disable CS8601 // Possible null reference assignment.
                        ResponseData = JsonConvert.DeserializeObject<WeatherData>(response);
#pragma warning restore CS8601 // Possible null reference assignment.
                    }
                }
#pragma warning restore IDE0063 // Use simple 'using' statement
            }
            catch (Exception)
            {
                throw new Exception("Error happened while processing the request");
            }
        }


        //public void GetRequest()
        //{
        //    try
        //    {

        //        using (WebClient client = new WebClient())
        //        {
        //            client.Headers.Add("Content-type", "application/json; charset=utf-8");

        //            var response = client.DownloadString(this.URL);
        //            if (response != null)
        //            {
        //                ResponseData = JsonConvert.DeserializeObject<object>(response);
        //            }
        //            else
        //            {
        //                ResponseData = "Received empty or null response while retrieving the data";
        //            }
        //        }

        //    }
        //    catch (WebException ex)
        //    {
        //        ResponseData = null;
        //    }
        //}

        #endregion
    }
}