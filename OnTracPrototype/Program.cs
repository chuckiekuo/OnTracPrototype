using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OnTracPrototype
{
    class Program
    {
        

        public static string RADARSEARCHAPIURL = "https://maps.googleapis.com/maps/api/place/radarsearch/json?location=";
        // API key is shared for all google stuff
        public static string apiKey = "AIzaSyD2sCTxHUWNmIeSSoCfBJoE28StO8J6uiI";

        static void Main(string[] args)
        {
            SearchAddressController myController = new SearchAddressController();

            var url = RADARSEARCHAPIURL + "39.4996,119.7681" + "&radius=50000&type=restaurant&key=" + apiKey;

            var json = myController.PullDataFromAPI(url);

            JObject myJObject = JObject.Parse(json);

            IList<JToken> results = myJObject["results"].Children().ToList();

            foreach (var data in results)
            {
                Node newNode = new Node()
                {
                    Id = Guid.NewGuid().ToString(),
                    Latitude = data["geometry"]["location"]["lat"].ToString(),
                    Longitude = data["geometry"]["location"]["lng"].ToString()
                };

                myController.NodeList.Add(newNode);
            }

            // Node list created at this point, need to create groupings of closest nodes

            myController.GenerateDistanceList();
        }

        

       
    }
}
