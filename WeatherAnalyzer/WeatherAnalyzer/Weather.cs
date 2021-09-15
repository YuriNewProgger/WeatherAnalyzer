using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalyzer
{
    [JsonObject(MemberSerialization.OptIn)]
    class Weather
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("temp")]
        public double temp { get; set; }
        //[JsonProperty("temp_feels_like")]
        //public double temp_feels_like { get; set; }
        [JsonProperty("temp_min")]
        public double temp_min { get; set; }
        [JsonProperty("temp_max")]
        public double temp_max { get; set; }
        //[JsonProperty("pressure")]
        //public int pressure { get; set; }
        [JsonProperty("humidity")]
        public double humidity { get; set; }
        [JsonProperty("wind_speed")]
        public double wind_speed { get; set; }
        //[JsonProperty("wind_deg")]
        //public double wind_deg { get; set; }
        //[JsonProperty("wind_gust")]
        //public double wind_gust { get; set; }
        //[JsonProperty("createdAt")]
        //public double createdAt { get; set; }
        //[JsonProperty("updatedAt")]
        //public double updatedAt { get; set; }
        [JsonProperty("city")]
        public string city { get; set; }
    }
}
//"id":1676,
//"temp":7.89,
//"temp_feels_like":5.57,
//"temp_min":6.13,
//"temp_max":9.75,
//"pressure":1014,
//"humidity":60,
//"wind_speed":3.69,
//"wind_deg":314,
//"wind_gust":6.78,
//"createdAt":"2021-09-15T18:00:01.176Z",
//"updatedAt":"2021-09-15T18:00:01.176Z",
//"city":"moscow"}
