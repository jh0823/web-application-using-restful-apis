using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace createMapService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebGet(UriTemplate = "/createmap/{address}/{city}/{state}", ResponseFormat = WebMessageFormat.Json)] // Returns JSON Object instead of default XML
        returnMapObject CreateMap(string address, string city, string state);

      

        
    }
    // RETURN MAPOBJECT FOR JSON
    [DataContract]
    public class returnMapObject
    {
        [DataMember]
        public string mapURL;
    }

    #region DataContracts for Google Geocoder API
    // START OF GOOGLE GEOCODER API OBJECTS
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
    public class RootObject
    {
        [DataMember]
        public List<Result> results { get; set; }
        [DataMember]
        public string status { get; set; }
    }
    // END OF GOOGLE GEOCODER API OBJECTS
    #endregion



}
