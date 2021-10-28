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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Takes the user's input for the city and state 
            string city = TextBox1.Text;
            string state = TextBox2.Text;

            // Clears the textbox that will contain all of the articles and their information in case it was already used
            // User could've entered an incorrect format after already calling the service so this needs to be cleared
            TextBox3.Text = " ";

            // Checks to see if any of the text boxes were empty. Must enter a city AND state to get articles
            if (string.IsNullOrEmpty(TextBox1.Text) || string.IsNullOrEmpty(TextBox2.Text))
            {
                Label3.Text = "Incorrect format, please enter city, and state.";
            }

            // User has entered the correct format
            else
            {
                // Clear the error label (Label3) in case they entered incorrect format before entering correct format
                Label3.Text = "";
                // News API URL with city and state appended 
                //http://webstrar7.fulton.asu.edu/page8/Service1.svc/getnews
                string getNewsUrl = @"http://webstrar7.fulton.asu.edu/page8/Service1.svc/getnews/" + city + "/" + state;
                

                //make request to service
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getNewsUrl);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responsereader = reader.ReadToEnd();


                // Service returns JSON object containing a list of articles with the following information:
                // Article's title
                // Article's description
                // Article's url

                returnArticleObject returnArticleObject = JsonConvert.DeserializeObject<returnArticleObject>(responsereader);
                // Counter for the article number
                int i = 1;

                // Create a temp string to concatenate the Article information to print
                // Need to parse through to retrieve the correct information. Each index contains 1 article
                string temp = "";
                for (int j = 0; j < returnArticleObject.url.Count; j++)
                {
                    temp += "Article " + i.ToString() + ":\n";
                    temp+= TextBox3.Text + "Title: " + returnArticleObject.title[j] + "\n" +
                        "Description: " + returnArticleObject.description[j] + "\n" +
                        "URL: " + returnArticleObject.url[j] + "\n\n\n";
                    i++;
                }

                // Set TextBox3.Text to temp string to display all the articles
                TextBox3.Text = temp;
            }
        }
    }

    // The JSON Article that my getNews service returns
    // It contains a list of strings for the article titles, article descriptions, and article urls
    public class returnArticleObject
    {
        public List<string> title { get; set; }
        public List<string> description { get; set; }
        public List<string> url { get; set; }
    }
}