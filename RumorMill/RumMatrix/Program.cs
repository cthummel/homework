using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace RumorMill
{
    class RumorMatrix
    {
        private string OrderedList;

        static void Main(string[] args)
        {
            string namepattern = @"^(\S*)\s(\S*)$";

            var StudentList = new List<string>();
            var FriendList = new Dictionary<string, List<string>>();

            string line = Console.ReadLine();
            int StudentCount = Int32.Parse(line);


            for (int i = 0; i < StudentCount; i++)
            {
                line = Console.ReadLine();
                StudentList.Add(line);
            }

            line = Console.ReadLine();
            int FriendCount = Int32.Parse(line);

            bool[,] edges = new bool[StudentCount, StudentCount];
            //Collects all the edges and saves them in an Adjecency List.
            //Remember that edges here are not directed.
            for (int i = 0; i < FriendCount; i++)
            {
                var f1templist = new List<string>();
                var f2templist = new List<string>();
                int f1index = -1, f2index = -1, j = 0;


                string friend1 = "", friend2 = "";

                line = Console.ReadLine();
                MatchCollection Matches = Regex.Matches(line, namepattern);

                foreach (Match match in Matches)
                {
                    friend1 = match.Groups[1].ToString();
                    friend2 = match.Groups[2].ToString();
                }


                foreach (string student in StudentList)
                {
                    if (student == friend1)
                    {
                        f1index = j;
                    }
                    if (student == friend2)
                    {
                        f2index = j;
                    }

                    if (f1index != -1 && f2index != -1)
                    {
                        break;
                    }

                    j++;
                }

                edges[f1index, f2index] = true;
                edges[f2index, f1index] = true;

                f1index = -1;
                f2index = -1;

            }

            //Generating reports.
            line = Console.ReadLine();
            int ReportCount = Int32.Parse(line);

            for (int i = 0; i < ReportCount; i++)
            {
                string initialperson = Console.ReadLine();
                RumorMatrix rumor = new RumorMatrix();

                rumor.GenerateReport(edges, StudentList, initialperson);

                //Write the solution.
                Console.WriteLine(rumor.OrderedList);
            }
        }

        /// <summary>
        /// Generates a report using established Adjecency List and a rumor starter.
        /// </summary>
        /// <param name="Graph"></param>
        /// <param name="initialperson"></param>
        private void GenerateReport(bool[,] Graph, List<string> StudentList, string initialperson)
        {
            var Queue = new Queue<string>();
            string current;
            int i = 0;
            //int index = 0;
            bool[] visited = new bool[StudentList.Count];
            //var Unchecked = new List<string>(StudentList);
            var Levelverts = new List<string>();
            string finalelement = initialperson;

            //Unchecked.Remove(initialperson);
            Queue.Enqueue(initialperson);
            OrderedList = initialperson;

            while (Queue.Count != 0)
            {
                current = Queue.Dequeue();
                foreach (string student in StudentList)
                {
                    if (student == current)
                    {
                        for (int j = 0; j < StudentList.Count; j++)
                        {
                            if (Graph[i, j] && visited[j] == false)
                            {
                                Queue.Enqueue(StudentList.ElementAt(j));
                                visited[j] = true;
                                Levelverts.Add(StudentList.ElementAt(j));
                            }
                        }
                    }
                    i++;
                }

                if (current == finalelement)
                {
                    if (Levelverts.Count != 0)
                    {
                        finalelement = Levelverts.ElementAt(Levelverts.Count - 1);
                    }

                    Levelverts.Sort();
                    foreach (string vertex in Levelverts)
                    {
                        OrderedList = (OrderedList + " " + vertex);
                    }
                    Levelverts = new List<string>();
                }
                i = 0;
            }
            //Tack the leftovers on the end.
            for (int k = 0; k < StudentList.Count; k++)
            {
                if (visited[k] == false)
                {
                    OrderedList = (OrderedList + " " + StudentList.ElementAt(k));
                }

            }

        }
    }
}
