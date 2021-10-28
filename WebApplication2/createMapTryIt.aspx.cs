using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace WebApplication2
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // This is where the user's input is stored
            string address = TextBox1.Text;
            string city = TextBox2.Text;
            string state = TextBox3.Text;
            
            /// Currently, this implementation will create a map with either:
            /// The address, city, and state OR
            /// The city and state
            /// The user will have the choice to do either one those two choices listed above
            /// The user will HAVE TO have a city and state in their input
            
            // This will check to make sure the user has entered a city and a state
            if (string.IsNullOrEmpty(TextBox2.Text) || string.IsNullOrEmpty(TextBox3.Text))
            {
                // If there is no city or state, print an error statement 
                Label7.Text = "Incorrect format. Please enter at least city and state for Home";
                Image1.ImageUrl = "";
            }
            // If the user entered at least the city and state
            else
            {
                // Reset the Error Label (Label7)
                Label7.Text = "";
                /// Due to the implementation, the user could not have entered an address and just entered a city and state
                /// This if statement will check that condition
                
                // If the user DID NOT enter an address and ONLY entered a city and state
                if (string.IsNullOrEmpty(TextBox1.Text))
                {
                    // The address string is set to just a space so that it can still build the URL that will call the CreateMap service
                    address = " ";      
                    string createMapUrl = @"http://webstrar7.fulton.asu.edu/page7/Service1.svc/createmap/" + address + "/" + city + "/" + state;
                    // Make the HTTPWebRequest
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createMapUrl);
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string responsereader = reader.ReadToEnd();

                    // The CreateMap Service will return a JSON Object that contains a string which is set to the CreateMapURL
                    returnMapObject returnMapObject = JsonConvert.DeserializeObject<returnMapObject>(responsereader);
                    // Set the imageURL to the CreateMapURL
                    Image1.ImageUrl = returnMapObject.mapURL; 
                }
                // If the user DID enter an address, city and state, the else statement will process it the same as above except the address string will contain the user's address
                else
                {
                    // Create the URL to call the CreateMap service          
                    string createMapUrl = @"http://webstrar7.fulton.asu.edu/page7/Service1.svc/createmap/" + address + "/" + city + "/" + state;
                    // Make the HTTPWebRequest
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createMapUrl);
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string responsereader = reader.ReadToEnd();

                    // Create the JSON Object to deserialize
                    returnMapObject returnMapObject = JsonConvert.DeserializeObject<returnMapObject>(responsereader);
                    // Set the ImageURL to the CreateMapURL
                    Image1.ImageUrl = returnMapObject.mapURL;

                }

            }
        }

        /// This Button (Add Marker) will allow for the user to additional marker given the address, city, and state
        /// The image map will update with the first address that was inputted and the new marker displayed
        /// The map will automatically adjust the zoom so that both markers are shown and it will be homeed around the midpoint between the two addresses
        protected void Button2_Click(object sender, EventArgs e)
        {
            // Get all the information for the home Address
            string homeAddress = TextBox1.Text;
            string homeCity = TextBox2.Text;
            string homeState = TextBox3.Text;
            // Get all the information for the Marker
            string markerAddress = TextBox4.Text;
            string markerCity = TextBox5.Text;
            string markerState = TextBox6.Text;

            /// To pass it to the createMap service, the strings are concatenated with a ',' separating the two addresses, two cities, two states
            /// The string address will have: home address,marker address 
            /// The string city will have: home city,markercity
            /// The string state will have: home state,markerstate
            /// 
            /// This will be used to create the URL to call the createMap Service
            string address = homeAddress + "," + markerAddress;
            string city = homeCity + "," + markerCity;
            string state = homeState + "," + markerState;

            // A check to ensure that the user entered all the required information: the home city, home state, marker city, and marker state.
            // This service does not require the user to enter an address to process. This is based off user's choice
            if (string.IsNullOrEmpty(TextBox2.Text) || string.IsNullOrEmpty(TextBox3.Text) ||string.IsNullOrEmpty(TextBox5.Text) || string.IsNullOrEmpty(TextBox6.Text))
            {
                // If the user did not enter the vital information, display the error message and then clear the image url so that no map is displayed
                Label7.Text = "Incorrect format. Please enter at least city and state for home address and marker";
                Image1.ImageUrl = "";
            }
            // If the user did enter the correct information
            else
            {
                // Clear the error message
                Label7.Text = "";
                // If the user did not enter an address for the home or marker
                if (string.IsNullOrEmpty(TextBox1.Text) || string.IsNullOrEmpty(TextBox4.Text))
                {
                    // Make a filler string for address so that the createMap service can still process the user's request
                    address = " ";  
                    string createMapUrl = @"http://webstrar7.fulton.asu.edu/page7/Service1.svc/createmap/" + address + "/" + city + "/" + state;
                    // Make the HTTPWebRequest
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createMapUrl);
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string responsereader = reader.ReadToEnd();

                    // Create the JSON Object to deserialize
                    returnMapObject returnMapObject = JsonConvert.DeserializeObject<returnMapObject>(responsereader);
                    // Set the ImageURL to the CreateMapURL
                    Image1.ImageUrl = returnMapObject.mapURL;
                }
                // If the user did enter an address, process the request like normal
                else
                {
                    // createMap service URL
                    string createMapUrl = @"http://webstrar7.fulton.asu.edu/page7/Service1.svc/createmap/" + address + "/" + city + "/" + state;
                    // Make the HTTPWebRequest
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createMapUrl);
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string responsereader = reader.ReadToEnd();

                    // Create the JSON Object to deserialize
                    returnMapObject returnMapObject = JsonConvert.DeserializeObject<returnMapObject>(responsereader);
                    // Set the ImageURL to the CreateMapURL
                    Image1.ImageUrl = returnMapObject.mapURL;

                }

            }

        }
    }
    // createMap Service Root Object
    public class returnMapObject
    {
        
        public string mapURL;
    }
}