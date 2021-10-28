using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;



// Jessica Hong
// 1210741516 
// CSE 445 HW3 Part 2
namespace hw3_test_1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate = "/getnews/{city}/{state}", ResponseFormat = WebMessageFormat.Json)] // setting the webmessage format to return a JSON object vs the default XML format
        returnArticleInfo GetNews(string city, string state);

    }

    [DataContract]
    public class returnArticleInfo
    {
        [DataMember]
        public List<string> title { get; set; }
        [DataMember]
        public List<string> description { get; set; }
        [DataMember]
        public List<string> url { get; set; }
    }
    [DataContract]
    public class Article
    {
        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string urlToImage { get; set; }
        [DataMember]
        public string publishedAt { get; set; }
    }
    [DataContract]
    public class RootObject
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string source { get; set; }
        [DataMember]
        public string sortBy { get; set; }
        [DataMember]
        public List<Article> ArticlesList { get; set; }
    }
   
}


   
   
