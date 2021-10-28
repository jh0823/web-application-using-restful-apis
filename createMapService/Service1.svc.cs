using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace createMapService
{
         /// <summary>
         /// createMapService is the first elective service for Part3.1
         /// The Google Static Map API takes in latitude and longitude; however the user's input is an address, city and state. Therefore the Google's Geocoder API was used to convert the user's input into
         /// latitude and longitude coordinates so that the Static Map API can be used
         /// 
         /// The createMapService has two parts to it:
         /// The first part, takes in one address, city and state OR it can just take in a city and state if the user does not wish to put an address and returns a static map image provided
         /// through Google's Static Map API. This static image will have a marker at the user's inputted address and the map will be centered around that marker
         /// 
         /// The second part, takes in 2 sets of addresses, cities, and states. Simiarily to the first part, the user has the choice to not input an address and only input a city and state
         /// With the two sets of inputs, a new map will be created where a marker will be placed at both addresses, the static map will adjust the zoom and center the map around the midpoint between 
         /// the two markers so that both markers are visible to the user. 
         /// </summary>
    public class Service1 : IService1
    {
    
        public returnMapObject CreateMap(string address, string city, string state)
        {
            
            /// The user's address could have spaces in it (house number, street name, etc)
            /// The user's city could also have spaces in it ( San Diego, Los Angeles, etc)
            /// In order to create the url correctly, need to add '+' in the place of each space and a ',' at the end of the address and city

            // Create string[] to store the address and city into
            string[] addressArray = address.Split(',');
            string[] cityArray = city.Split(',');
            string[] stateArray = state.Split(',');


            // Create strings to build the url
            string homeAddressTemp = "";
            string homeCityTemp = "";
            string homeStateTemp = "";

            string markerAddressTemp = "";
            string markerCityTemp = "";
            string markerStateTemp = "";



            // If the addressArray contains any elements, add a '+' after each one and concatenate onto string
            // if its greater than 1, than theres a marker attached to the address (need to parse through this)
            /// addressArray[0] = contains home address
            /// cityArray[0] = contains home city
            /// stateArray[0] = contains home state
            /// 
            /// addressArray[1] = contains marker address
            /// cityArray[1] = contains marker city
            /// stateArray[1] = contains marker state
            /// 
            // Check to see if there is more than one address in the array
            if (addressArray.Length > 1)
            {
                // Split the first address (home address) into a string[] by spaces to be able to build the Geocoder API url
                string[] homeAddTemp = addressArray[0].Split(' ');
                // Split the second address (marker address) into a string[] by spaces to be able to build the URL
                string[] markerAddTemp = addressArray[1].Split(' ');

                // For all words inside the home address, concatenate a '+' to the end of it
                for (int j = 0; j < homeAddTemp.Length; j++)
                {
                    homeAddressTemp += homeAddTemp[j] + "+";
                }
                // Do the same for the marker address
                for (int i = 0; i < markerAddTemp.Length; i++)
                {
                    markerAddressTemp += markerAddTemp[i] + "+";
                }
                //The strings will have an extra '+', this will remove the last character in the strings
                homeAddressTemp = homeAddressTemp.Remove(homeAddressTemp.Length - 1);
                markerAddressTemp = markerAddressTemp.Remove(markerAddressTemp.Length - 1);
                // Append a ',' after the address when completed
                homeAddressTemp += ",";
                markerAddressTemp += ",";

            }
            // if there is only the home address, then parse like normal
            else
            {
                string[] homeAddTemp = addressArray[0].Split(' ');
                for (int i = 0; i < homeAddTemp.Length; i++)
                {
                    homeAddressTemp += homeAddTemp[i] + "+";
                }
                //The string will have an extra '+', this will remove the last character in the string
                homeAddressTemp = homeAddressTemp.Remove(homeAddressTemp.Length - 1);
                // Append a ',' after the address when completed
                homeAddressTemp += ",";
            }


            // Similar to how the address was parsed through, if the city has any element in the string[], add '+' after each element and build the string
            /// This if statement will check to see if there is a marker city attached, if so, parse through both cities by:
            /// First: Spliting the cities by any spaces
            /// Second: Concatenating the words together with a '+' 
            /// Third: Remove the extra '+'
            /// Finally: Add a ',' at the end

            if (cityArray.Length > 1)
            {
                string[] homeCTemp = cityArray[0].Split(' ');
                string[] markerCTemp = cityArray[1].Split(' ');
                for (int j = 0; j < homeCTemp.Length; j++)
                {
                    homeCityTemp += homeCTemp[j] + "+";
                }
                for (int i = 0; i < markerCTemp.Length; i++)
                {
                    markerCityTemp += markerCTemp[i] + "+";
                }

                homeCityTemp = homeCityTemp.Remove(homeCityTemp.Length - 1);
                markerCityTemp = markerCityTemp.Remove(markerCityTemp.Length - 1);

                homeCityTemp += ",";
                markerCityTemp += ",";
                
            }
            // Same process above, except only to the home address as there is no marker to be added
            else
            {
                string[] homeCTemp = cityArray[0].Split(' ');
                for (int i = 0; i < homeCTemp.Length; i++)
                {
                    homeCityTemp += homeCTemp[i] + "+";
                }
                // Removes the extra '+' 
                homeCityTemp = homeCityTemp.Remove(homeCityTemp.Length - 1);
                // Adds a ',' at the end of the cityTemp string
                homeCityTemp += ",";
            }

            /// The stateArray is processed the same way as the addressArray and the cityArray 
            if (stateArray.Length > 1)
            {
                // Splits the states by spaces 
                string[] homeSTemp = stateArray[0].Split(' ');
                string[] markerSTemp = stateArray[1].Split(' ');
                // Add the '+'
                for (int j = 0; j < homeSTemp.Length; j++)
                {
                    homeStateTemp += homeSTemp[j] + "+";
                }
                for (int i = 0; i < markerSTemp.Length; i++)
                {
                    markerStateTemp += markerSTemp[i] + "+";
                }

                // Remove the extra '+'
                homeStateTemp = homeStateTemp.Remove(homeStateTemp.Length - 1);
                markerStateTemp = markerStateTemp.Remove(markerStateTemp.Length - 1);
        

            }
            else
            {
                string[] homeSTemp = stateArray[0].Split(' ');
                for (int i = 0; i < homeSTemp.Length; i++)
                {
                    homeStateTemp += homeSTemp[i] + "+";
                }
                // Removes the extra '+' 
                homeStateTemp = homeStateTemp.Remove(homeStateTemp.Length - 1);
                
                
            }


            // This googeAPI will use the strings above to be able to retrieve the latitude and longitude that will be used to build the map url
            string googlehomeAPI = @"https://maps.googleapis.com/maps/api/geocode/json?address=" + homeAddressTemp + "+" + homeCityTemp + "+" + homeStateTemp + "&key=AIzaSyC4sRiQHF9Axdz7C6gaHxypBi_g7sY4WqM";

            // Make the HTTPWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(googlehomeAPI);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();

            // The Google Geocoder returns a Json Object, create the rootobject and then deserialize it
            RootObject geocoderObject = JsonConvert.DeserializeObject<RootObject>(responsereader);

            //Get the latitude and longitude from the object
            double latitude = geocoderObject.results[0].geometry.location.lat;
            double longitude = geocoderObject.results[0].geometry.location.lng;
            //Convert the doubles into strings to build the Static Map API URL
            string latString = latitude.ToString();
            string lngString = longitude.ToString();
            
            /// The Google Static Map API returns an image using the user's address, city, and state as the home
            /// Currently, the zoom is set to 12, with the map size being 600x600 and map type as roadmap
            /// A marker is placed on the location of the user's address using the latitude and longitude found from the Google Geocoder API
            /// This implementation will just create a string that contains the url for the static map API
            string googleStaticAPI = @"https://maps.googleapis.com/maps/api/staticmap?home=" + homeAddressTemp + ","+ homeCityTemp + ","+ homeStateTemp + "&zoom=12&size=600x600&maptype=roadmap&markers=color:red%7C" + 
                latString +"," + lngString + "&key=AIzaSyC4sRiQHF9Axdz7C6gaHxypBi_g7sY4WqM";

            // Create the JSON object that createMap service will return
            returnMapObject returnMapObject = new returnMapObject();
            // Set the string to ""
            returnMapObject.mapURL = "";
            // Set the string to the static map api url
            returnMapObject.mapURL = googleStaticAPI;
            // Return the object 
            //return returnMapObject;


            // ADD MARKER CODE
            /// IF THERE WAS A MARKER ATTACHED
            /// Check to see if the markerAddressTemp, markerCityTemp, markerStateTemp are null
            /// If they are not, then process the marker
            
            // This will check to see if there was a marker city and state, if there is not, just return the JSON object that contains the map URL 
            if (string.IsNullOrEmpty(markerCityTemp) && string.IsNullOrEmpty(markerStateTemp))
            {
                return returnMapObject;
            }
            // If they are not empty, add the marker to the map with the original address still on there
            else
            {
                // First, by using the Google Geocoder API, get the latitude and the longitude from the markerAddress, markerCity, markerState to be able to add the marker onto the map
                string googleMarkerAPI = @"https://maps.googleapis.com/maps/api/geocode/json?address=" + markerAddressTemp + "+" + markerCityTemp + "+" + markerStateTemp + "&key=AIzaSyC4sRiQHF9Axdz7C6gaHxypBi_g7sY4WqM";

                // Make the HTTPWebRequest
                request = (HttpWebRequest)WebRequest.Create(googleMarkerAPI);
                response = request.GetResponse();
                dataStream = response.GetResponseStream();
                sreader = new StreamReader(dataStream);
                responsereader = sreader.ReadToEnd();
                response.Close();

                // The Google Geocoder returns a Json Object, create the rootobject and then deserialize it
                RootObject geocoderObjectMarker = JsonConvert.DeserializeObject<RootObject>(responsereader);

                //Get the latitude and longitude from the object for the marker
                double latitudeM = geocoderObjectMarker.results[0].geometry.location.lat;
                double longitudeM = geocoderObjectMarker.results[0].geometry.location.lng;
                //Convert the doubles into strings to build the Static Map API URL
                string latStringM = latitudeM.ToString();
                string lngStringM = longitudeM.ToString();

                
                
                /// To create the urlhomeMarker, which will return the map image that has both markers (the original address and the new marker)
                /// Concatenate the url string with the home around the midpoint between the two markers,
                /// The zoom level adjusted to display both markers on the map image,
                /// Add the marker color
                /// Add the latitude and longitude of the marker,
                /// Finally, attach the API key at the end

                string urlhomeMarker = @"https://maps.googleapis.com/maps/api/staticmap?size=600x600&maptype=roadmap&markers=size:mid%7Ccolor:red%7C" 
                    + latString + "," + lngString + "%7C"
                    + latStringM + "," + lngStringM + "&key=AIzaSyC4sRiQHF9Axdz7C6gaHxypBi_g7sY4WqM";

                // Set the createMap Service JSON object's mapURL to the static map image with both markers
                returnMapObject.mapURL = urlhomeMarker;

            }
            // Return the createMap Service object
            return returnMapObject;
            



        }
        [Serializable]
        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }
        [Serializable]
        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
        [Serializable]
        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
        [Serializable]
        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
        [Serializable]
        public class Viewport
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }
        [Serializable]
        public class Geometry
        {
            public Location location { get; set; }
            public string location_type { get; set; }
            public Viewport viewport { get; set; }
        }
        [Serializable]
        public class PlusCode
        {
            public string compound_code { get; set; }
            public string global_code { get; set; }
        }
        [Serializable]
        public class Result
        {
            public List<AddressComponent> address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public PlusCode plus_code { get; set; }
            public List<string> types { get; set; }
        }
        [Serializable]
        public class RootObject
        {
            public List<Result> results { get; set; }
            public string status { get; set; }
        }

    }

    
}
