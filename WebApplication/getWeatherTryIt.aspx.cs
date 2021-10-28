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
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string address = TextBox1.Text; // text box where user will input the address
            string city = TextBox2.Text; // text box where user will input the city
            string state = TextBox3.Text; // text box where user will input the state
            TextBox4.Text = ""; // clears the results text box to ensure clean, correct results on the webform

            if (string.IsNullOrEmpty(TextBox1.Text) || string.IsNullOrEmpty(TextBox2.Text) || string.IsNullOrEmpty(TextBox3.Text)) // checks to see if user entered all parts of the address
            {
                Label4.Text = "Incorrect format. Please enter an address, city, and state"; // prompt user to enter correct format
            }
            else // if user entered the address in the correct format
            {
                Label4.Text = ""; // clears the incorrect format label 
                           string createWeatherURL = @"http://webstrar7.fulton.asu.edu/page9/Service1.svc/getweather/" + address + "/" + city + "/" + state; // builds url to call getWeather service
                //make HTTPWebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(createWeatherURL);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responsereader = reader.ReadToEnd();

                // my Weather Service returns a JSON object that contains a string array where each index contains the weather for that day
                returnWeatherObject returnWeatherObject = JsonConvert.DeserializeObject<returnWeatherObject>(responsereader);

                // Splits the string into a string array to print results by day 
                string[] weather = new string[5];
                // Due to the weather values being doubles, trunicate the strings by 2 decimals places
                for (int i = 0; i < weather.Length; i++)
                {
                    weather[i] = returnWeatherObject.weather5DayString[i].Substring(0, 5);
                }
                // Display the weather for the next 5 days
                TextBox4.Text = "Day 1: " + weather[0] + "F\n" + "Day 2: " + weather[1] + "F\n" + "Day 3: " + weather[2] + "F\n" + "Day 4: " + weather[3] + "F\n" + "Day 5: " + weather[4] + "F\n";

            }
        }
    }
    // JSON Object from my service
    public class returnWeatherObject
    {
        public string[] weather5DayString { get; set; }
    }
}