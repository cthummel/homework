using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;


namespace RumorMill
{
    class RumorMill
    {
        static void Main(string[] args)
        {
            string namepattern = @"^(\S*)\s(\S*)$";

            var StudentList = new List<string>();
            var FriendList = new Dictionary<string, List<string>>();

            string line = Console.ReadLine();
            int StudentCount = Int32.Parse(line);
            
            //Create StudentList
            for (int i = 0; i < StudentCount; i++)
            {
                line = Console.ReadLine();
                StudentList.Add(line);
            }
            
            line = Console.ReadLine();
            int FriendCount = Int32.Parse(line);
            
            //Collects all the edges and saves them in an Adjecency List.
            //Remember that edges here are not directed.
            for (int i = 0; i < FriendCount; i++)
            {
                var f1templist = new List<string>();
                var f2templist = new List<string>();
                string friend1 = "", friend2 = "";

                line = Console.ReadLine();
                MatchCollection Matches = Regex.Matches(line, namepattern);
                
                foreach(Match match in Matches)
                {
                    friend1 = match.Groups[1].ToString();
                    friend2 = match.Groups[2].ToString();
                }

                //Adjecency List version
                if (FriendList.TryGetValue(friend1, out f1templist))
                {
                    //Adds Friend 2 to Friend 1's edge list.
                    f1templist.Add(friend2);
                    FriendList[friend1] = f1templist;

                    //Adds Friend 1 to Friend 2's edge list
                    if (FriendList.TryGetValue(friend2, out f2templist))
                    {
                        f2templist.Add(friend1);
                        FriendList[friend2] = f2templist;
                    }
                    else
                    {
                        f2templist = new List<string>();
                        f2templist.Add(friend1);
                        FriendList.Add(friend2, f2templist);
                    }
                }
                else
                {
                    //Makes a new entry for Friend 1 and adds Friend 2 to Friend 1's edge list.
                    f1templist = new List<string>();
                    f1templist.Add(friend2);
                    FriendList.Add(friend1, f1templist);

                    //Adds Friend 1 to Friend 2's edge list
                    if (FriendList.TryGetValue(friend2, out f2templist))
                    {
                        f2templist.Add(friend1);
                        FriendList[friend2] = f2templist;
                    }
                    else
                    {
                        f2templist = new List<string>();
                        f2templist.Add(friend1);
                        FriendList.Add(friend2, f2templist);
                    }
                }
            }

            //Generating reports.
            line = Console.ReadLine();
            int ReportCount = Int32.Parse(line);
            RumorMill rumor = new RumorMill();
            for (int i = 0; i < ReportCount; i++)
            {
                string initialperson = Console.ReadLine();
                rumor.GenerateReport(FriendList, StudentList, initialperson);
            }
        }

        /// <summary>
        /// Generates a report using established Adjecency List and a rumor starter.
        /// </summary>
        /// <param name="Graph"></param>
        /// <param name="initialperson"></param>
        private void GenerateReport(Dictionary<string, List<string>> Graph, List<string> StudentList, string initialperson)
        {
            
            string current;
            string finalelement = initialperson;
            StringBuilder OrderedList = new StringBuilder(initialperson);

            var Queue = new Queue<string>();
            var CurrentEdges = new List<string>();
            var Unchecked = new List<string>();
            var Levelverts = new List<string>();
            var Visited = new Dictionary<string, bool>();

            foreach (string s in StudentList)
            {
                Visited.Add(s, false);
            }

            Queue.Enqueue(initialperson);
            Visited[initialperson] = true;

            while (Queue.Count != 0)
            {
                current = Queue.Dequeue();
                Graph.TryGetValue(current, out CurrentEdges);
                
                if (CurrentEdges != null)
                {
                    foreach (string vertex in CurrentEdges)
                    {
                        if (Visited[vertex] == false)
                        {
                            Queue.Enqueue(vertex);
                            Visited[vertex] = true;
                            Levelverts.Add(vertex);
                        }
                    }
                }

                //Only save once we have all elements on a given level of the graph.
                if (current == finalelement)
                {
                    if (Levelverts.Count != 0)
                    {
                        finalelement = Levelverts.ElementAt(Levelverts.Count - 1);
                    }
                    
                    Levelverts.Sort();
                    foreach (string vertex in Levelverts)
                    {
                        OrderedList.Append(" " + vertex);
                    }
                    Levelverts = new List<string>();
                }
            }

            //Tack the leftovers on the end.
            foreach (string s in Visited.Keys)
            {
                if (Visited[s] == false)
                {
                    Unchecked.Add(s);
                }
            }
            Unchecked.Sort();
            foreach(string vertex in Unchecked)
            {
                OrderedList.Append(" " + vertex);
            }
            Console.WriteLine(OrderedList);
        }
    }
}
