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



// Jessica Hong
// 1210741516 
// CSE 445 HW3 Part 2
namespace hw3_test_1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public returnArticleInfo GetNews(string city, string state) // get the city and state that was given from user
        {
            // if user enters a city/state with a space in it, replace it with a '+' to be able to build the keyword
            
            string keyWord = city + "+" + state; // combine into a keyword 
            

            string url = @"http://newsapi.org/v2/everything?q=" + keyWord + "&sortBy=publishedAt&apiKey=cbb6186f24834f81aa74264b07a438ca"; // building the url for the api

            //make the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();

            //API returns a JSON object, deserialize the object to parse for key information: article title, description, and url
            RootObject articleObject = JsonConvert.DeserializeObject<RootObject>(responsereader);

            // Creating the returnArticleInfo object and setting all of the lists<string> to new lists to be able to add the article information when parsing through the API's JSON object
            returnArticleInfo returnArticleInfo = new returnArticleInfo();
            returnArticleInfo.title = new List<string>();
            returnArticleInfo.description = new List<string>();
            returnArticleInfo.url = new List<string>();

            // go through the JSON object, extract the article information (title, description, url) and append to the string
            for (int j = 0; j < articleObject.Articles.Count; j++)
            {
                returnArticleInfo.title.Add(articleObject.Articles[j].title);
                returnArticleInfo.description.Add(articleObject.Articles[j].description);
                returnArticleInfo.url.Add(articleObject.Articles[j].url);

                
            }

            // return the Json Object

            return returnArticleInfo;



        }
        [Serializable]
        public class Article // article object
        {

            public string author { get; set; }

            public string title { get; set; }

            public string description { get; set; }

            public string url { get; set; }

            public string urlToImage { get; set; }

            public string publishedAt { get; set; }

        }
        [Serializable]
        public class RootObject // root object
        {
            public string status { get; set; }
            public string source { get; set; }
            public string sortBy { get; set; }

            public List<Article> Articles { get; set; } // list of article objects
        }
    }
    
}
