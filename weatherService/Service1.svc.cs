using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace weatherService
{
    /// <summary>
    ///  The weatherService project contains 2 elective services that I have implemented for Part 3.1
    ///  The first elective service is the GetWeather service
    ///  The second elective service is the GetSolarData service
    ///  
    /// As there are only 3 pages for each member on the Webstrar server, the professor stated that we had to combine our elective services into one project. Each service has its unique endpoint and try it page
    /// </summary>
    public class Service1 : IService1
    {
        /// This is the second elective service that I have chosen to implement for Part3.1 
        /// returnWeatherObject will take in an address, city, and state and find the average temperature for the next 5 days
        /// Due to the user's input being in an address, city, state format, I call the Google's Geocoder API to retrieve the latitude and longitude to be able to use the weather API
        /// Once the latitude and longitude are retrieved, I build the weather API url to be able to retrieve the temperatures for the next 5 days
        /// The weather API takes weather measurements every 3 hours, therefore to get a day's average temperature, I averaged the temperatures for every 8 indexes and stored those values
        /// The weather API also returns the temperatures in Kelvin, so I had to convert all the average temperatures to Fahrenheit before returning my JSON object
       
        public returnWeatherObject GetWeather(string address, string city, string state)
        {
            // Takes user's inputs for address and city and splits it by spaces to be able to create the url later
            string[] addressArray = address.Split(' ');
            string[] cityArray = city.Split(' ');

            // Create temp strings to later create the url 
            string addressTemp = "";
            string cityTemp = "";

            // If the address contains more than one word, add a '+' at the end of each word to build the API url (Used for address with house numbers, street names, etc)
            if (addressArray.Length > 0)
            {
                for (int i = 0; i < addressArray.Length; i++)
                {
                    addressTemp += addressArray[i] + "+";
                }
                // The for loop will add an extra '+' at the end of the address, this will remove the last character in the string
                addressTemp = addressTemp.Remove(addressTemp.Length - 1);
                // Append a ',' to finish off the address part for the API URL
                addressTemp += ",";
            }
            
            //Similarily to address, if the city contains more than one word, add a '+' at the end of each word (Used for cities like San Diego, Los Angeles)
            if (cityArray.Length > 0)
            {
                for (int i = 0; i < cityArray.Length; i++)
                {
                    cityTemp += cityArray[i] + "+";
                }
                // Removes the extra '+' at the end of the string and then appends a ',' to finish building the city string for the API URL
                cityTemp = cityTemp.Remove(cityTemp.Length - 1);
                cityTemp += ",";
            }

            /// Builds the API url with the addressTemp, cityTemp, state, and api key
            string googleGeocoderAPI = @"https://maps.googleapis.com/maps/api/geocode/json?address=" + addressTemp + "+" + cityTemp + "+" + state + "&key=AIzaSyC4sRiQHF9Axdz7C6gaHxypBi_g7sY4WqM";

            // Makes the HTTPWebRequest 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(googleGeocoderAPI);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();

            // The Geocoder API returns a Json Object, deserialize the object
            GRootObject geocoderObject = JsonConvert.DeserializeObject<GRootObject>(responsereader);

            // Extract the user's address' latitude and longitude to use for the weather service
            double latitude = geocoderObject.results[0].geometry.location.lat;
            double longitude = geocoderObject.results[0].geometry.location.lng;

            // Convert the latitude and longitude to strings to build the weather service api url
            string latString = latitude.ToString();
            string lngString = longitude.ToString();

            // Build the API url for the weather service using the latitude, longitude and API key
            string weatherServiceAPI = @"http://api.openweathermap.org/data/2.5/forecast?lat=" + latString + "&lon=" + lngString + "&appid=c4a6960312a7e97bdb2ebd3c9d4ba44d";

            // HTTPWebRequest
            HttpWebRequest wrequest = (HttpWebRequest)WebRequest.Create(weatherServiceAPI);
            WebResponse wresponse = wrequest.GetResponse();
            Stream wdataStream = wresponse.GetResponseStream();
            StreamReader wsreader = new StreamReader(wdataStream);
            string wresponsereader = wsreader.ReadToEnd();
            wresponse.Close();

            // The Weather Service API returns a Json object, deserailize the object
            WRootObject weatherObject = JsonConvert.DeserializeObject<WRootObject>(wresponsereader);

            // This Weather API will take weather measurements in 3 hour increments for the next 5 days of the given location.
            // Using a double[] to store only the temperatures from the Json Object. This is found under RootObject.list[i].main.temp
            double[] weatherTemp = new double[weatherObject.list.Count];
            for (int i = 0; i < weatherTemp.Length; i++)
            {
                weatherTemp[i] = weatherObject.list[i].main.temp;
            }

            // As the weather measurements are in 3 hour increments, this means 8 measurements for that one day 
            // Calculate the average using 8 indexes at a time and storing it into a double[] 
            double[] dayWeathers = new double[5];
            dayWeathers[0] = calcAvg(weatherTemp, 0, 7);
            dayWeathers[1] = calcAvg(weatherTemp, 8, 15);
            dayWeathers[2] = calcAvg(weatherTemp, 16, 23);
            dayWeathers[3] = calcAvg(weatherTemp, 24, 31);
            dayWeathers[4] = calcAvg(weatherTemp, 32, 39);

            // The weather API records the temperatures in Kelvin
            // convertKtoF will convert all Kelvin temperatures into Fahrenheit
            double[] fDayWeathers = new double[5];
            fDayWeathers = convertKtoF(dayWeathers);

          
            // Create the return JSON object
            returnWeatherObject returnWeatherObject = new returnWeatherObject();
            // initialize the string[]
            returnWeatherObject.weather5DayString = new string[5];
            // fill the string array with the average temps that are now converted to Fahrenheit
            // Need to convert the doubles into strings 
            for (int i = 0; i < returnWeatherObject.weather5DayString.Length; i++)
            {
                returnWeatherObject.weather5DayString[i] = fDayWeathers[i].ToString();
            }

            // Return the JSON object
            return returnWeatherObject;
        }


        // Used to calculate the average 
        // Takes in the double[], a start index and an end index for ease of us
        // returns the average which will be used in the double[] 
        public double calcAvg(double[] weatherTemp, int start, int end)
        {
            double average;
            double sum = 0.00;
            for (int i = start; i <= end; i++)
            {
                sum +=  weatherTemp[i];
            }
            average = sum / 8.00;
            return average;
        }

        // Used to convert Kelvin temperatures to Fahrenheit
        // Takes in the double[] and applies the formula at each index
        // Returns a double[]
        public double [] convertKtoF(double[] dayWeathers)
        {
            double [] fDayWeathers = new double[5];
            for (int i = 0; i < 5; i++)
            {
                fDayWeathers[i] = (dayWeathers[i] - 273.15)  * (9.00/5.00) + 32.00;
            }
            return fDayWeathers;
            
        }
        


        /// Professor Balasooriya stated that for part 3.1 of this HW, we could implement at minimum 2 elective services and at max 4 elective services.
        /// However, if we were to choose to implement more than 2 elective services, we would not have enough pages on the Webstrar service to upload the services
        /// Therefore, we were instructed to combine the extra services to another service so that we would be able to upload onto the server
      
        /// GetSolar is the third service that I will be implementing for part 3.1
        /// This service takes in two pairs of a city, state and compare the solar intensity of these given pairs
        /// As the user will input a city and state and the solar API requires a latitude and longitude pair, I use Google's Geocoder API to get the latitude and longitude of the city, state
        /// The latitude and longitude are then stored to create the url to call the solar API and the JSON object returned is parsed and stored in my own JSON object that my service will return to the web application
        /// where it will be displayed to the user
        /// START OF THE THIRD SERVICE
        public returnSolarObject GetSolar(string city1, string state1, string city2, string state2)
        {
            // as the user can enter a city with a space (San Diego, Los Angeles, etc) split the string by the spaces and store into a string[] for both cities
            string[] cityArray1 = city1.Split(' ');
            string[] cityArray2 = city2.Split(' ');
            // Same is done to the states (New Mexico, New York, etc)
            string[] stateArray1 = state1.Split(' ');
            string[] stateArray2 = state2.Split(' ');
            // Create temp strings to be used to build the url to call the Google Geocoder API
            string cityTemp1 = "";
            string cityTemp2 = "";
            string stateTemp1 = "";
            string stateTemp2 = "";

             
         
            /// To build the Google Geocoder API correctly, need to add a + in the place of every space that was originally inputted by the user
            /// Go through each string[] and add a '+' after each element
            /// This process will add an extra '+' at the end of the string and .Remove(string.Length - 1) will remove that extra '+'
            /// Then append a ',' at the end to continue building the URL
            for (int i = 0; i < cityArray1.Length; i++)
            {
                cityTemp1 += cityArray1[i] + "+";
            }
            cityTemp1 = cityTemp1.Remove(cityTemp1.Length - 1);
            cityTemp1 += ",";

            for (int i = 0; i < cityArray2.Length; i++)
            {
                cityTemp2 += cityArray2[i] + "+";
            }
            cityTemp2 = cityTemp2.Remove(cityTemp2.Length - 1);
            cityTemp2 += ",";

            // For the states, follow the same procedure as the cities, but don't need the ',' at the end
            for (int i = 0; i < stateArray1.Length; i++)
            {
                stateTemp1 += stateArray1[i] + "+";
            }
            stateTemp1 = stateTemp1.Remove(stateTemp1.Length - 1);
           

            for (int i = 0; i < stateArray2.Length; i++)
            {
                stateTemp2 += stateArray2[i] + "+";
            }
            stateTemp2 = stateTemp2.Remove(stateTemp2.Length - 1);
            


            // https://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&key=YOUR_API_KEY
            // Build the URL for the Google Geocoder API
            string googleGeoCoderAPI = @"https://maps.googleapis.com/maps/api/geocode/json?address=" + cityTemp1 + "+" + stateTemp1 + "&key=AIzaSyC4sRiQHF9Axdz7C6gaHxypBi_g7sY4WqM";
            // Make the HTTPWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(googleGeoCoderAPI);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            
            // The Google Geocoder API returns a JSON Object that will contain the city, state pair's latitude and longitude 
            GRootObject geocoderObject1 = JsonConvert.DeserializeObject<GRootObject>(responsereader);
            // Store the latitude and longitude for the first city, state pair
            double latitude1 = geocoderObject1.results[0].geometry.location.lat;
            double longitude1 = geocoderObject1.results[0].geometry.location.lng;
            // Convert them to strings
            string latString1 = latitude1.ToString();
            string lngString1 = longitude1.ToString();
            
            // Follow the same procedure for the second city, state pair
            googleGeoCoderAPI = @"https://maps.googleapis.com/maps/api/geocode/json?address=" + cityTemp2 + "+" + stateTemp2 + "&key=AIzaSyC4sRiQHF9Axdz7C6gaHxypBi_g7sY4WqM";
            //Make the HTTPWebRequest
            request = (HttpWebRequest)WebRequest.Create(googleGeoCoderAPI);
            response = request.GetResponse();
            dataStream = response.GetResponseStream();
            sreader = new StreamReader(dataStream);
            responsereader = sreader.ReadToEnd();
            response.Close();
            // Store the second pair's latitude and longitude
            GRootObject geocoderObject2 = JsonConvert.DeserializeObject<GRootObject>(responsereader);

            double latitude2 = geocoderObject2.results[0].geometry.location.lat;
            double longitude2 = geocoderObject2.results[0].geometry.location.lng;
            
            string latString2 = latitude2.ToString();
            string lngString2 = longitude2.ToString();

            // https://developer.nrel.gov/api/solar/solar_resource/v1.json?api_key=iVcEzvj7Bvx5ePqoycnPO0AcgWLgV2aayaGbgPmj&lat=33&lon=-112

            /// Now that I have the latitude and longitude for the city, state, I build the URL for the Solar API for the first pair
            /// The Solar API returns a JSON Object that will contain both the annual solar intensity and the monthly average solar intensities from that latitude and longitude coordinates
            string solarAPIURL = @"https://developer.nrel.gov/api/solar/solar_resource/v1.json?api_key=iVcEzvj7Bvx5ePqoycnPO0AcgWLgV2aayaGbgPmj&lat=" + latString1 + "&lon=" + lngString1;
            // Make the HTTPWebRequest
            request = (HttpWebRequest)WebRequest.Create(solarAPIURL);
            response = request.GetResponse();
            dataStream = response.GetResponseStream();
            sreader = new StreamReader(dataStream);
            responsereader = sreader.ReadToEnd();
            response.Close();

            SRootObject solarObject1 = JsonConvert.DeserializeObject<SRootObject>(responsereader);
            string[] solarValues1 = new string[13];

            /// There is probably a more efficient way to store the all the monthly averages for the city, state pair but
            /// 
            //Stores the monthly aveerages in the string[] to return back to the web application in the returnSolarObject
            // [0] contains the annual solar intensity
            // [1] - [12] contain all the monthly averages
            solarValues1[0] = solarObject1.outputs.avg_dni.annual.ToString();
            solarValues1[1] = "Jan: " + solarObject1.outputs.avg_dni.monthly.jan.ToString();
            solarValues1[2] = "Feb: " + solarObject1.outputs.avg_dni.monthly.feb.ToString();
            solarValues1[3] = "Mar: " + solarObject1.outputs.avg_dni.monthly.mar.ToString();
            solarValues1[4] = "Apr: " + solarObject1.outputs.avg_dni.monthly.apr.ToString();
            solarValues1[5] = "May: " + solarObject1.outputs.avg_dni.monthly.may.ToString();
            solarValues1[6] = "Jun: " + solarObject1.outputs.avg_dni.monthly.jun.ToString();
            solarValues1[7] = "Jul: " + solarObject1.outputs.avg_dni.monthly.jul.ToString();
            solarValues1[8] = "Aug: " + solarObject1.outputs.avg_dni.monthly.aug.ToString();
            solarValues1[9] = "Sep: " + solarObject1.outputs.avg_dni.monthly.sep.ToString();
            solarValues1[10] = "Oct: " + solarObject1.outputs.avg_dni.monthly.oct.ToString();
            solarValues1[11] = "Nov: " + solarObject1.outputs.avg_dni.monthly.nov.ToString();
            solarValues1[12] = "Dec: " + solarObject1.outputs.avg_dni.monthly.dec.ToString();

            // Repeat the same process above but for the second city, state pair
            solarAPIURL = @"https://developer.nrel.gov/api/solar/solar_resource/v1.json?api_key=iVcEzvj7Bvx5ePqoycnPO0AcgWLgV2aayaGbgPmj&lat=" + latString2 + "&lon=" + lngString2;
            // Make the HTTPWebRequest
            request = (HttpWebRequest)WebRequest.Create(solarAPIURL);
            response = request.GetResponse();
            dataStream = response.GetResponseStream();
            sreader = new StreamReader(dataStream);
            responsereader = sreader.ReadToEnd();
            response.Close();

            SRootObject solarObject2 = JsonConvert.DeserializeObject<SRootObject>(responsereader);

            string[] solarValues2 = new string[13];
            // Store the solar intensities
            // [0] is annual solar inensitiy
            // [1] - [12] are the monthly averages
            solarValues2[0] = solarObject2.outputs.avg_dni.annual.ToString();
            solarValues2[1] = "Jan: " + solarObject2.outputs.avg_dni.monthly.jan.ToString();
            solarValues2[2] = "Feb: " + solarObject2.outputs.avg_dni.monthly.feb.ToString();
            solarValues2[3] = "Mar: " + solarObject2.outputs.avg_dni.monthly.mar.ToString();
            solarValues2[4] = "Apr: " + solarObject2.outputs.avg_dni.monthly.apr.ToString();
            solarValues2[5] = "May: " + solarObject2.outputs.avg_dni.monthly.may.ToString();
            solarValues2[6] = "Jun: " + solarObject2.outputs.avg_dni.monthly.jun.ToString();
            solarValues2[7] = "Jul: " + solarObject2.outputs.avg_dni.monthly.jul.ToString();
            solarValues2[8] = "Aug: " + solarObject2.outputs.avg_dni.monthly.aug.ToString();
            solarValues2[9] = "Sep: " + solarObject2.outputs.avg_dni.monthly.sep.ToString();
            solarValues2[10] = "Oct: " + solarObject2.outputs.avg_dni.monthly.oct.ToString();
            solarValues2[11] = "Nov: " + solarObject2.outputs.avg_dni.monthly.nov.ToString();
            solarValues2[12] = "Dec: " + solarObject2.outputs.avg_dni.monthly.dec.ToString();

            // create the JSON object that getSolarData service will return
            returnSolarObject returnSolarObject = new returnSolarObject();
            returnSolarObject.solarValues1 = solarValues1;
            returnSolarObject.solarValues2 = solarValues2;
            // Compare the annual solar intensity of both pairs and build a string for the results, also contains the annual solar intensity value
            // If statement checks to see it the first pair has a greater annual solar intensity
            if (solarObject1.outputs.avg_dni.annual > solarObject2.outputs.avg_dni.annual)
            {
                returnSolarObject.greaterAnnual = city1 + ", " + state1 +"(" + solarValues1[0] + ") has a greater annual solar intensity than " + city2 + ", " + state2 + "(" + solarValues2[0] + ")\n";
            }
            // if second pair has a greater annaul solar intensity
            else
            {
                returnSolarObject.greaterAnnual = city2 + ", " + state2 + "(" + solarValues2[0] + ") has a greater annual solar intensity than " + city1 + ", " + state1  + "(" + solarValues1[0] + ")\n";
            }
            // Return the returnSolarObject
            return returnSolarObject;
        }

        // START OF GOOGLE GEOCODER API CLASSES

        #region Google Geocoder API Classes
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
        public class GRootObject
        {
            public List<Result> results { get; set; }
            public string status { get; set; }
        }
        #endregion

        // END OF GOOGLE GEOCODER API CLASSES

        // START OF WEATHER API CLASSES
        #region Weather API Classes
        [Serializable]
        public class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public int pressure { get; set; }
            public int sea_level { get; set; }
            public int grnd_level { get; set; }
            public int humidity { get; set; }
            public double temp_kf { get; set; }
        }
        [Serializable]
        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }
        [Serializable]
        public class Clouds
        {
            public int all { get; set; }
        }
        [Serializable]
        public class Wind
        {
            public double speed { get; set; }
            public int deg { get; set; }
        }
        [Serializable]
        public class Sys
        {
            public string pod { get; set; }
        }
        [Serializable]
        public class Rain
        {
            public double __invalid_name__3h { get; set; }
        }

        [Serializable]
        public class Snow
        {
            public double snowfall { get; set; }
        }
        [Serializable]
        public class List
        {
            public int dt { get; set; }
            public Main main { get; set; }
            public List<Weather> weather { get; set; }
            public Clouds clouds { get; set; }
            public Wind wind { get; set; }
            public Sys sys { get; set; }
            public string dt_txt { get; set; }
            public Rain rain { get; set; }
            public Snow snow { get; set; }
        }
        [Serializable]
        public class Coord
        {
            public double lat { get; set; }
            public double lon { get; set; }
        }
        [Serializable]
        public class City
        {
            public int id { get; set; }
            public string name { get; set; }
            public Coord coord { get; set; }
            public string country { get; set; }
            public int population { get; set; }
            public int timezone { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }
        [Serializable]
        public class WRootObject
        {
            public string cod { get; set; }
            public int message { get; set; }
            public int cnt { get; set; }
            public List<List> list { get; set; }
            public City city { get; set; }
        }
        #endregion

        // END OF WEATHER API CLASSES
        #region Solar API Classes
        // START OF SOLAR API OBJECTS
        [Serializable]
        public class Metadata
        {
            public List<string> sources { get; set; }
        }
        [Serializable]
        public class Inputs
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }
        [Serializable]
        public class Monthly
        {
            public double jan { get; set; }
            public double feb { get; set; }
            public double mar { get; set; }
            public double apr { get; set; }
            public double may { get; set; }
            public double jun { get; set; }
            public double jul { get; set; }
            public double aug { get; set; }
            public double sep { get; set; }
            public double oct { get; set; }
            public double nov { get; set; }
            public double dec { get; set; }
        }
        [Serializable]
        public class AvgDni
        {
            public double annual { get; set; }
            public Monthly monthly { get; set; }
        }
        [Serializable]
        public class Monthly2
        {
            public double jan { get; set; }
            public double feb { get; set; }
            public double mar { get; set; }
            public double apr { get; set; }
            public double may { get; set; }
            public double jun { get; set; }
            public double jul { get; set; }
            public double aug { get; set; }
            public double sep { get; set; }
            public double oct { get; set; }
            public double nov { get; set; }
            public double dec { get; set; }
        }
        [Serializable]
        public class AvgGhi
        {
            public double annual { get; set; }
            public Monthly2 monthly { get; set; }
        }
        [Serializable]
        public class Monthly3
        {
            public double jan { get; set; }
            public double feb { get; set; }
            public double mar { get; set; }
            public double apr { get; set; }
            public double may { get; set; }
            public double jun { get; set; }
            public double jul { get; set; }
            public double aug { get; set; }
            public double sep { get; set; }
            public double oct { get; set; }
            public double nov { get; set; }
            public double dec { get; set; }
        }
        [Serializable]
        public class AvgLatTilt
        {
            public double annual { get; set; }
            public Monthly3 monthly { get; set; }
        }
        [Serializable]
        public class Outputs
        {
            public AvgDni avg_dni { get; set; }
            public AvgGhi avg_ghi { get; set; }
            public AvgLatTilt avg_lat_tilt { get; set; }
        }
        [Serializable]
        public class SRootObject
        {
            public string version { get; set; }
            public List<object> warnings { get; set; }
            public List<object> errors { get; set; }
            public Metadata metadata { get; set; }
            public Inputs inputs { get; set; }
            public Outputs outputs { get; set; }
        }
        #endregion

        // END OF SOLAR API CLASSES

    }
}
