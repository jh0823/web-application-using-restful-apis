using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace weatherService
{
    /// <summary>
    ///  GetWeather is one elective service and GetSolar is another elective service. As we are unable to upload more than 3 services to the Webstrar server,
    ///  I am combining these into one service project with unique endpoints and try it pages in the web application
    /// </summary>
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebGet(UriTemplate = "/getweather/{address}/{city}/{state}", ResponseFormat = WebMessageFormat.Json)]
        returnWeatherObject GetWeather(string address, string city, string state);

        [OperationContract]
        [WebGet(UriTemplate = "/getsolardata/{city1}/{state1}/{city2}/{state2}", ResponseFormat = WebMessageFormat.Json)]
        returnSolarObject GetSolar(string city1, string state1, string city2, string state2);



     
    }

    [DataContract]
    public class returnSolarObject
    {
        [DataMember]
        public string[] solarValues1 { get; set; }
        [DataMember]
        public string[] solarValues2 { get; set; }
        [DataMember]
        public string greaterAnnual { get; set; }
    }

    [DataContract]
    public class returnWeatherObject
    {
        [DataMember]
        public string[] weather5DayString { get; set; }
    }

    #region Data Contracts for the Google Geocoder API
    [DataContract]
    public class AddressComponent
    {
        [DataMember]
        public string long_name { get; set; }
        [DataMember]
        public string short_name { get; set; }

        [DataMember]
        public List<string> types { get; set; }
    }

    [DataContract]
    public class Location
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
    }

    [DataContract]
    public class Northeast
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
    }
    [DataContract]
    public class Southwest
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
    }
    [DataContract]
    public class Viewport
    {
        [DataMember]
        public Northeast northeast { get; set; }
        [DataMember]
        public Southwest southwest { get; set; }
    }
    [DataContract]
    public class Geometry
    {
        [DataMember]
        public Location location { get; set; }

        [DataMember]
        public string location_type { get; set; }

        [DataMember]
        public Viewport viewport { get; set; }
    }

    [DataContract]
    public class PlusCode
    {
        [DataMember]
        public string compound_code { get; set; }
        [DataMember]
        public string global_code { get; set; }
    }
    [DataContract]
    public class Result
    {
        [DataMember]
        public List<AddressComponent> address_components { get; set; }
        [DataMember]
        public string formatted_address { get; set; }
        [DataMember]
        public Geometry geometry { get; set; }
        [DataMember]
        public string place_id { get; set; }
        [DataMember]
        public PlusCode plus_code { get; set; }
        [DataMember]
        public List<string> types { get; set; }

    }
    [DataContract]
    public class GRootObject
    {
        [DataMember]
        public List<Result> results { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    #endregion


 
    #region Data Contracts for the Weather API

    [DataContract]
    public class Main
    {
        [DataMember]
        public double temp { get; set; }
        [DataMember]
        public double feels_like { get; set; }
        [DataMember]
        public double temp_min { get; set; }
        [DataMember]
        public double temp_max { get; set; }
        [DataMember]
        public int pressure { get; set; }
        [DataMember]
        public int sea_level { get; set; }
        [DataMember]
        public int grnd_level { get; set; }
        [DataMember]
        public int humidity { get; set; }
        [DataMember]
        public double temp_kf { get; set; }
    }

    [DataContract]
    public class Weather
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string main { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string icon { get; set; }
    }

    [DataContract]
    public class Clouds
    {
        [DataMember]
        public int all { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember]
        public double speed { get; set; }
        [DataMember]
        public int deg { get; set; }
    }
    [DataContract]
    public class Sys
    {
        [DataMember]
        public string pod { get; set; }
    }
    [DataContract]
    public class Rain
    {
        [DataMember]
        public double __invalid_name__3h { get; set; }
    }

    [DataContract]
    public class Snow
    {
        [DataMember]
        public double snowfall { get; set; }
    }

    [DataContract]
    public class List
    {
        [DataMember]
        public int dt { get; set; }
        [DataMember]
        public Main main { get; set; }
        [DataMember]
        public List<Weather> weather { get; set; }
        [DataMember]
        public Clouds clouds { get; set; }
        [DataMember]
        public Wind wind { get; set; }
        [DataMember]
        public Sys sys { get; set; }
        [DataMember]
        public string dt_txt { get; set; }
        [DataMember]
        public Rain rain { get; set; }
        [DataMember]
        public Snow snow { get; set; }
    }
    [DataContract]
    public class Coord
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lon { get; set; }
    }
    [DataContract]
    public class City
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public Coord coord { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public int population { get; set; }
        [DataMember]
        public int timezone { get; set; }
        [DataMember]
        public int sunrise { get; set; }
        [DataMember]
        public int sunset { get; set; }
    }
    [DataContract]
    public class WRootObject
    {
        [DataMember]
        public string cod { get; set; }
        [DataMember]
        public int message { get; set; }
        [DataMember]
        public int cnt { get; set; }
        [DataMember]
        public List<List> list { get; set; }
        [DataMember]
        public City city { get; set; }
    }
    #endregion


    #region Data Contracts for Solar API
    // START OF SOLAR API OBJECTS
    [DataContract]
    public class Metadata
    {
        [DataMember]
        public List<string> sources { get; set; }
    }
    [DataContract]
    public class Inputs
    {
        [DataMember]
        public string lat { get; set; }
        [DataMember]
        public string lon { get; set; }
    }
    [DataContract]
    public class Monthly
    {
        [DataMember]
        public double jan { get; set; }
        [DataMember]
        public double feb { get; set; }
        [DataMember]
        public double mar { get; set; }
        [DataMember]
        public double apr { get; set; }
        [DataMember]
        public double may { get; set; }
        [DataMember]
        public double jun { get; set; }
        [DataMember]
        public double jul { get; set; }
        [DataMember]
        public double aug { get; set; }
        [DataMember]
        public double sep { get; set; }
        [DataMember]
        public double oct { get; set; }
        [DataMember]
        public double nov { get; set; }
        [DataMember]
        public double dec { get; set; }
    }
    [DataContract]
    public class AvgDni
    {
        [DataMember]
        public double annual { get; set; }
        [DataMember]
        public Monthly monthly { get; set; }
    }
    [DataContract]
    public class Monthly2
    {
        [DataMember]
        public double jan { get; set; }
        [DataMember]
        public double feb { get; set; }
        [DataMember]
        public double mar { get; set; }
        [DataMember]
        public double apr { get; set; }
        [DataMember]
        public double may { get; set; }
        [DataMember]
        public double jun { get; set; }
        [DataMember]
        public double jul { get; set; }
        [DataMember]
        public double aug { get; set; }
        [DataMember]
        public double sep { get; set; }
        [DataMember]
        public double oct { get; set; }
        [DataMember]
        public double nov { get; set; }
        [DataMember]
        public double dec { get; set; }
    }
    [DataContract]
    public class AvgGhi
    {
        [DataMember]
        public double annual { get; set; }
        [DataMember]
        public Monthly2 monthly { get; set; }
    }
    [DataContract]
    public class Monthly3
    {
        [DataMember]
        public double jan { get; set; }
        [DataMember]
        public double feb { get; set; }
        [DataMember]
        public double mar { get; set; }
        [DataMember]
        public double apr { get; set; }
        [DataMember]
        public double may { get; set; }
        [DataMember]
        public double jun { get; set; }
        [DataMember]
        public double jul { get; set; }
        [DataMember]
        public double aug { get; set; }
        [DataMember]
        public double sep { get; set; }
        [DataMember]
        public double oct { get; set; }
        [DataMember]
        public double nov { get; set; }
        [DataMember]
        public double dec { get; set; }
    }
    [DataContract]
    public class AvgLatTilt
    {
        [DataMember]
        public double annual { get; set; }
        [DataMember]
        public Monthly3 monthly { get; set; }
    }
    [DataContract]
    public class Outputs
    {
        [DataMember]
        public AvgDni avg_dni { get; set; }
        [DataMember]
        public AvgGhi avg_ghi { get; set; }
        [DataMember]
        public AvgLatTilt avg_lat_tilt { get; set; }
    }
    [DataContract]
    public class SRootObject
    {
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public List<object> warnings { get; set; }
        [DataMember]
        public List<object> errors { get; set; }
        [DataMember]
        public Metadata metadata { get; set; }
        [DataMember]
        public Inputs inputs { get; set; }
        [DataMember]
        public Outputs outputs { get; set; }
    }
    #endregion

}
