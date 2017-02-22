using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetShorty
{
    class GetShorty
    {
         


        static void Main(string[] args)
        {
            Dictionary<int, List<Edges>> PrisonMap = new Dictionary<int, List<Edges>>();
            string line = Console.ReadLine();
            string[] parameters = line.Split(' ');
            int IntersectionCount = Int32.Parse(parameters[0]);
            int CorridorCount = Int32.Parse(parameters[1]);
            //Building graph.
            for (int i = 0; i < CorridorCount; i++)
            {
                
                line = Console.ReadLine();
                parameters = line.Split(' ');
                int start = Int32.Parse(parameters[0]);
                int destination = Int32.Parse(parameters[1]);
                double weight = double.Parse(parameters[2]);
                Edges edge = new Edges(destination, weight);
                if (PrisonMap.ContainsKey(start))
                {

                }
                

            }


        }
    }


    class Edges
    {
        private int destination;
        private double weight;

        public Edges(int d, double w)
        {
            destination = d;
            weight = w;
        }
        public double Weight
        {
            get
            {
                return weight;
            }
        }
        public int Destination
        {
            get
            {
                return destination;
            }
        }
    }
}
