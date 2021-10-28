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
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            /// Get the user's input for both addresses
            /// city1 and state1 will be for address 1
            /// city2 and state2 will be for address 2
            string city1 = TextBox1.Text;
            string state1 = TextBox2.Text;
            string city2 = TextBox3.Text;
            string state2 = TextBox4.Text;

            // Clears the result boxes
            TextBox5.Text = "";
            TextBox6.Text = "";
            Label8.Text = "";
            

            // Check to see if the user entered both cities and states for both addressees 
            if (string.IsNullOrEmpty(city1) || string.IsNullOrEmpty(state1) || string.IsNullOrEmpty(city2) || string.IsNullOrEmpty(state2))
            {
                //Print out error message and clear the results
                Label5.Text = "Incorrect format. Please enter the city and state for address 1 and the city and state for address 2";
                TextBox5.Text = "";
                TextBox6.Text = "";
                Label8.Text = "";
            }
            // If user entered both cities and states
            else
            {
                // clear the error label (label5)
                Label5.Text = "";
                // build the URL to call the getSolarData Service with city1, state1, city 2, state2     
                string getSolarDataAPIURL = @"http://webstrar7.fulton.asu.edu/page9/Service1.svc/getsolardata/" + city1 + "/" + state1 + "/" + city2 + "/" + state2;

                // Make the HTTPWebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getSolarDataAPIURL);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responsereader = reader.ReadToEnd();

                /// The getSolarDataService returns a JSON Object that contains the following:
                /// solarValues1 which is a string[] where at [0], it contains the annual solar intensity for city1, state1 and the remaining indexes are the monthly solar intensities
                /// solarValues2 which is a string[] where at [0], it contains the annual solar intensity for city2, state2 and the remaining indexes are the monthly solar intensities
                /// greaterAnnual which is a string that states which city, state pair has a higher annual solar intensity 
                
                returnSolarObject returnSolarObject = JsonConvert.DeserializeObject<returnSolarObject>(responsereader);
                // As this will only print the monthly values, the indexes start at 1 and will add a newline at the end of each element to format the print statements
                for (int i = 1; i < returnSolarObject.solarValues1.Length; i++)
                {
                    TextBox5.Text += returnSolarObject.solarValues1[i] + "\n";
                }
                for (int i = 1; i < returnSolarObject.solarValues2.Length; i++)
                {
                    TextBox6.Text += returnSolarObject.solarValues2[i] + "\n";
                }
                // Print which city, state pair has a greater annual solar intensity 
                Label8.Text = returnSolarObject.greaterAnnual;


            }
            

        }
    }
    public class returnSolarObject
    {
        public string[] solarValues1 { get; set; }
        public string[] solarValues2 { get; set; }
        public string greaterAnnual { get; set; }
    }
}