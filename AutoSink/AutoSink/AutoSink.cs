using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AutoSink
{
    class AutoSink
    {
        static void Main(string[] args)
        {
            string citypattern = @"([a-zA-Z]*)(\s)(\d+)";
            string trippattern = @"^([a-zA-Z]*)(\s)([a-zA-Z]*)$";
            var Map = new Dictionary<string, List<string>>();
            string line = Console.ReadLine();
            int CityCount = Int32.Parse(line);

            //Collects all the verticies and the associated costs.
            int[] costs = new int[CityCount];
            string[] cities = new string [CityCount];

            for (int i = 0; i < CityCount; i++)
            {
                line = Console.ReadLine();
                MatchCollection matches = Regex.Matches(line, citypattern);
                foreach (Match match in matches)
                {
                    cities[i] = match.Groups[1].ToString();
                    costs[i] = Int32.Parse(match.Groups[3].ToString());
                }
            }

            //Collects all edges in the graph.
            line = Console.ReadLine();
            int HighwayCount = Int32.Parse(line);
            var Highways = new Dictionary<string, string>();

            for (int i = 0; i < HighwayCount; i++)
            {
                line = Console.ReadLine();
                MatchCollection matches = Regex.Matches(line, trippattern);
                
                foreach (Match match in matches)
                {
                    Highways.Add(match.Groups[1].ToString(), match.Groups[3].ToString());
                }

            }

            //Collects the potential trips from input
            line = Console.ReadLine();
            int TripCount = Int32.Parse(line);

            var Trips = new Dictionary<string, string>();

            for (int i = 0; i < HighwayCount; i++)
            {
                line = Console.ReadLine();
                MatchCollection matches = Regex.Matches(line, trippattern);

                foreach (Match match in matches)
                {
                    Trips.Add(match.Groups[1].ToString(), match.Groups[3].ToString());
                }

            }

            //Builds and saves graph.
            var graph = new Dictionary<string, List<string>>();




            //Builds and saves reverse graph.
            var reversegraph = new Dictionary<string, List<string>>();



        }
        private class Node
        {
            //private List<string> parent;
            private int cost;
            private string city;
            private List<string> children;
            public Node(List<string> Children, string City, int Cost)
            {
                //parent = Parent;
                city = City;
                cost = Cost;
                children = Children;

            }

            //public string Parent
            //{
            //    get { return parent;  }
            //}
            public int Cost
            {
                get { return cost;  }
            }

        }
        public class Map
        {

        }


        
    }
}
