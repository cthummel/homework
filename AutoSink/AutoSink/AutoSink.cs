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
        private Dictionary<string, bool> Verticies;
        private List<string> SortedCities;
        private Dictionary<string, int[]> Order;
        private int current;
        private Dictionary<string, int> Map;

        static void Main(string[] args)
        {
            string citypattern = @"([0-9a-zA-Z]*)(\s)(\d+)";
            string trippattern = @"^([0-9a-zA-Z]*)(\s)([0-9a-zA-Z]*)$";
            var Map = new Dictionary<string, int>();
            string line = Console.ReadLine();
            int CityCount = Int32.Parse(line);

            for (int i = 0; i < CityCount; i++)
            {
                line = Console.ReadLine();
                MatchCollection matches = Regex.Matches(line, citypattern);
                foreach (Match match in matches)
                {
                    Map.Add(match.Groups[1].ToString(), Int32.Parse(match.Groups[3].ToString()));
                }
            }

            //Collects all edges in the graph.
            line = Console.ReadLine();
            int HighwayCount = Int32.Parse(line);
            var Highways = new Dictionary<string, List<string>>();

            for (int i = 0; i < HighwayCount; i++)
            {
                line = Console.ReadLine();
                MatchCollection matches = Regex.Matches(line, trippattern);
                
                foreach (Match match in matches)
                {
                    var endpoints = new List<string>();

                    string start = match.Groups[1].ToString();
                    string end = match.Groups[3].ToString();

                    //Builds the adjecency list for the graph.
                    if (Highways.TryGetValue(start, out endpoints))
                    {
                        endpoints.Add(end);
                        Highways.Remove(start);
                        Highways.Add(start, endpoints);
                    }
                    else
                    {
                        endpoints = new List<string>();
                        endpoints.Add(end);
                        Highways.Add(start, endpoints);
                    }

                   
                }
            }

            //Collects the potential trips from input
            line = Console.ReadLine();
            int TripCount = Int32.Parse(line);

            var Trips = new List<string>();

            for (int i = 0; i < TripCount; i++)
            {
                line = Console.ReadLine();
                Trips.Add(line);
            }


            //Runs the check on each trip. Remember SortedHighways is backwards.
            for (int i = 0; i < TripCount; i++)
            {
                string trip = Trips.ElementAt(i);
                bool found = false;
                int startindex = 0;
                string start = "", end = "";
                MatchCollection matches = Regex.Matches(trip, trippattern);

                //Pulling out the starting and end points of a trip
                foreach (Match match in matches)
                {
                    start = match.Groups[1].ToString();
                    end = match.Groups[3].ToString();
                }

                //If no movement is required
                if (start == end)
                {
                    Console.WriteLine("0");
                    continue;
                }

                //Deals with edge case of no highways
                if (HighwayCount == 0)
                {
                    if (start == end)
                    {
                        Console.WriteLine("0");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("NO");
                        continue;
                    }
                }

                //Gets a topological sort of Highways.
                var SortedHighways = new List<string>();
                AutoSink sink = new AutoSink();
                sink.Map = Map;
                sink.DepthFirstSearch(Highways, start);
                SortedHighways = sink.SortedCities;

                //Checks to see if the start and end are in the same connected piece.
                int[] startprefix;
                int[] endprefix;

                sink.Order.TryGetValue(start, out startprefix);
                sink.Order.TryGetValue(end, out endprefix);

                //Shows disconnected pieces!
                if (startprefix[1] < endprefix[0] || endprefix[1] < startprefix[0])
                {
                    Console.WriteLine("NO");
                    continue;
                }


                //Searching through the sorted list to find where to start calculating costs.
                for (int j = SortedHighways.Count - 1; j >=0; j--)
                {
                    //If we find the endpoint before the start then they can never be reached.
                    if (SortedHighways.ElementAt(j) == end)
                    {
                        found = true;
                        break;
                    }
                    if (SortedHighways.ElementAt(j) == start)
                    {
                        startindex = j;
                        break;
                    }
                }

                //Go to the next trip if we have already solved it.
                if (found == true)
                {
                    Console.WriteLine("NO");
                    continue;
                }
                
                //int currentindex = startindex;
                int cost = 0;
                var CostMap = new Dictionary<string, int>();
                CostMap.Add(start, 0);

                //Applying costs to the graph starting from the start.
                for (int j = startindex; j >= 0; j--)
                {
                    var Children = new List<string>();

                    CostMap.TryGetValue(SortedHighways.ElementAt(j), out cost);
                    Highways.TryGetValue(SortedHighways.ElementAt(j), out Children);

                    //If the current vertex is the ending one we report the cost.
                    if (SortedHighways.ElementAt(j) == end)
                    {
                        CostMap.TryGetValue(end, out cost);
                        Console.WriteLine(cost);
                        break;
                    }
                    if (Children != null)
                    {
                        foreach (string child in Children)
                        {
                            int price = 0, kidcost = 0;
                            Map.TryGetValue(child, out price);

                            //If this child has been reached before we set its cost as the min of what was there and cost + price.
                            if (CostMap.TryGetValue(child, out kidcost))
                            {
                                CostMap.Remove(child);
                                CostMap.Add(child, Math.Min((price + cost), kidcost));
                            }
                            //If this is the first time we look at the child we give it the current running cost plus the price of the kid.
                            else
                            {
                                CostMap.Add(child, price + cost);
                            }
                        }
                    }
                    //So if we are looking at a sink and it isnt the endpoint we need then this cannot be a proper path.
                    
                }
            }
        }

        /// <summary>
        /// Performs a depth first search of a Graph and returns a list of cities in reverse topological order.
        /// </summary>
        /// <param name="Highways"></param>
        /// <returns></returns>
        public void DepthFirstSearch(Dictionary<string, List<string>> Graph, string start)
        {
            Verticies = new Dictionary<string, bool>();
            SortedCities = new List<string>();
            Order = new Dictionary<string, int[]>();
            current = 1;

            foreach (string key in Map.Keys)
            {
                Verticies.Add(key, false);
            }

            Explore(Graph, start);

            foreach(string key in Map.Keys)
            {
                if (key != start)
                {
                    bool check = false;
                    Verticies.TryGetValue(key, out check);
                    if (check == false)
                    {
                        Explore(Graph, key);
                    }
                }
            }
        }

        /// <summary>
        /// Explores verticies connected to city in Graph and assigns prefix and postfix numbers.
        /// </summary>
        /// <param name="Graph"></param>
        /// <param name="city"></param>
        public void Explore(Dictionary<string, List<string>> Graph, string city)
        {
            var children = new List<string>();

            //Sets city to visited and applies prefix
            int[] orderedpairs = new int[2];
            orderedpairs[0] = current;
            current += 1;

            Verticies.Remove(city);
            Verticies.Add(city, true);

            Order.Add(city, orderedpairs);

            Graph.TryGetValue(city, out children);

            if (children != null)
            {
                //Looking through edges connected to city.
                foreach (string child in children)
                {
                    bool check = false;
                    Verticies.TryGetValue(child, out check);
                    if (check == false)
                    {
                        Explore(Graph, child);
                    }
                }
            }

            //Add Postfix number.
            int[] temp = new int[2];
            Order.TryGetValue(city, out temp);
            temp[1] = current;
            current += 1;
            Order.Remove(city);
            Order.Add(city, temp);

            //Add this city to SortedCities list.
            SortedCities.Add(city);
        }
    }
}
