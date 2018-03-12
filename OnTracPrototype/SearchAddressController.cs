using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OnTracPrototype
{

    public class SearchAddressController
    {
        // Note, we use imperial units here
        public static string APIURL1 = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=";
        public static string APIURL2 = "destinations=";
        public static string APIURL3 = "key=";

        // Lat/lon of reno airport in API format
        public static string STARTLATLON = "39.4996,119.7681";

        public List<List<Route>> RouteMatrix { get; set; }
        public List<Node> NodeList { get; set; }
        public SearchAddressController()
        {
            NodeList = new List<Node>();

            RouteMatrix = new List<List<Route>>();
        }

      
        public string PullDataFromAPI(string url)
        {
            return new WebClient().DownloadString(url);
        }

        public void GenerateDistanceList()
        {
            foreach (var from in NodeList)
            {
                List<Route> fromThisNodeToAllOthers = new List<Route>();

                // Creates temp list without the current node
                var dataset = NodeList.Where(x => x.Id != from.Id).ToList();

                // Gets route information between every other node in list

                foreach (var to in dataset)
                {
                    if (from.Id != to.Id)
                    {
                        Route r = QueryDistanceMatrix(from, to);
                        fromThisNodeToAllOthers.Add(r);
                    }
                }

                RouteMatrix.Add(fromThisNodeToAllOthers);
            }
        }

        // Can optimize for time of day as well
        public Route QueryDistanceMatrix(Node from, Node to)
        {
            var latlong1 = from.Latitude + "," + from.Longitude;
            var latlong2 = to.Latitude + "," + to.Longitude;

            var queryUrl = BuildDistanceMatrixUrl(latlong1, latlong2);

            var json = PullDataFromAPI(queryUrl);

            JObject myJObject = JObject.Parse(json);

            IList<JToken> results = myJObject["rows"].Children().ToList();
            Route route = null;

            foreach (var data in results)
            {
                route = new Route()
                {
                    IdFrom = from.Id,
                    IdTo = to.Id,
                    TravelTime = data["elements"]["duration"]["value"].ToString(),
                    Distance = data["elements"]["distance"]["value"].ToString()
                };
            }

            return route;
        }

        // Returns API Url for the distance matrix API to get JSON for distance between two GPS coordinates
        // Use this to get JSON from the API and parse for distance info
        public string BuildDistanceMatrixUrl(string latlong1, string latlong2)
        {
            return APIURL1 + latlong1 + "|" + latlong2 + "&key=" + Program.apiKey;
        }
    }
}
