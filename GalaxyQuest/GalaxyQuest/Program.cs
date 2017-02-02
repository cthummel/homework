using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyQuest
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = Console.ReadLine();

            string[] parameters = line.Split(' ');

            int distance = Int32.Parse(parameters[0]);
            int starcount = Int32.Parse(parameters[1]);
            
            var stars = new List<string>(); 
            var candidate = new List<string>(); 

            //If there are no stars then there is no majority galaxy.
            if (starcount == 0)
            {
                Console.WriteLine("NO");
                return;
            }

            //If there is one star than it is guarenteed to be a majority.
            if(starcount == 1)
            {
                Console.WriteLine("1");
                return;
            }

           //Saves all the stars into an array.
            while((line = Console.ReadLine()) != null)
            {
                stars.Add(line);
            }
            
            Galaxy galaxy = new Galaxy();


            //Narrows down stars to either a Candidate or nothing.
            candidate = galaxy.MajorityCandidate(stars, distance);
           

            //If there is a candidate remaining we search to find its galaxy memebers.
            if (candidate.Count == 1)
            {
                string[] final = candidate.ElementAt(0).Split(' ');

                int finalstarx = Int32.Parse(final[0]);
                int finalstary = Int32.Parse(final[1]);
                int count = 0;

                for(int i = 0; i < stars.Count; i++)
                {
                    string[] temp = stars[i].Split(' ');
                    int starx = Int32.Parse(temp[0]);
                    int stary = Int32.Parse(temp[1]);
                    int xdist = finalstarx - starx;
                    int ydist = finalstary - stary;


                    if ((Math.Pow(xdist, 2) + Math.Pow(ydist, 2)) <= Math.Pow(distance, 2))
                    {
                        count++;
                    }
                }

                if (count > (starcount / 2))
                {
                    Console.WriteLine(count);
                }

                else
                {
                    Console.WriteLine("NO");
                }
                
            }

            //If not we are done.
            else
            {
                Console.WriteLine("NO");
            }
        }
       
    }
    class Galaxy
    {
        /// <summary>
        /// Finds a candidate for a majority element. Takes an array of star coordinates and a restricting diameter.
        /// </summary>
        /// <param name="candidates"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<string> MajorityCandidate(List<string> candidates, int distance)
        {
            var results = new List<string>();

            if (candidates.Count <= 1)
            {
                return candidates;
            }
            
            for (int i = 0; i < (candidates.Count - 1); i += 2)
            {
                string[] star1 = candidates.ElementAt(i).Split(' ');
                string[] star2 = candidates.ElementAt(i + 1).Split(' ');
                
                int star1x = Int32.Parse(star1[0]);
                int star1y = Int32.Parse(star1[1]);
                int star2x = Int32.Parse(star2[0]);
                int star2y = Int32.Parse(star2[1]);

                int xdist = star1x - star2x;
                int ydist = star1y - star2y;

                //Checks if stars are within galatic distance. Saves one in a new array if so.
                if ((Math.Pow(xdist, 2) + Math.Pow(ydist, 2)) <= Math.Pow(distance,2))
                {
                    results.Add(candidates.ElementAt(i));
                }
            }
            //do odd  candidate length case
            if(candidates.Count % 2 != 0)
            {
                results.Add(candidates.ElementAt(candidates.Count - 1));
            }


            return MajorityCandidate(results, distance);
        }

    }
}
