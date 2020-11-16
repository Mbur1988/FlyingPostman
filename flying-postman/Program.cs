using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace flying_postman
{
    class Program
    {
        private static List<Station> stationList = new List<Station>();
        private static List<Plane> planeList = new List<Plane>();
        private static List<Tour> tourList = new List<Tour>();
        private static StreamWriter writer;

        static void Main(string[] args)
        {
            // Create new stopwatch for the purpose of timing the program runtime
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            // Start stopwatch
            stopwatch.Start();

            // Create string variable to store the program runtime
            string elapsed_time;

            // Create variables containing the names of the input .txt files and tour start time
            string station = "";
            string plane = "";
            string start = "";

            try
            {
                station = args[0];
            }

            catch (IndexOutOfRangeException IOOR)
            {
                Console.WriteLine("Argument for station file absent or incorrect");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
        
            try
            {
                plane = args[1];
            }

            catch (IndexOutOfRangeException IOOR)
            {
                Console.WriteLine("Argument for plane file absent or incorrect");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }

            try
            {
                start = args[2];
            }

            catch (IndexOutOfRangeException IOOR)
            {
                Console.WriteLine("Argument for tour start time absent or incorrect");
                Console.WriteLine("Input arguments as a minimum requirement should comprise:");
                Console.WriteLine("mail file (.txt)   plane file (.txt)   tour start time in 00:00 24h format");
                Console.WriteLine("Optional input argument for output file (must be preceeded by output command -o):");
                Console.WriteLine("-o output file (.txt)");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }

            // Run function to extract data from input files and create new classes
            Makestations(station);
            Makeplane(plane);

            // Create double in which to store tour length 
            double Total_length = 0;

            // Run function to return optimised route order as an array
            int[] order = Level1(ref Total_length);

            // Attempt at level 2 starting from line 280
            // order = level2(order);

            // Generate array contaaining tour duration in days[0], mins[1] and seconds[2]
            int[] time = Maketour(order, start);

            // Stop the stopwatch
            stopwatch.Stop();

            // Create a timespan for the total elapsed time of the stopwatch
            TimeSpan timespan = stopwatch.Elapsed;

            // Format the total elapsed time of the stopwatch to string variable formatted to 3 decimal places
            elapsed_time = String.Format("{0:0.000}", timespan.TotalSeconds);

            // If an output has been requested print the tour to console and create and save to specified .txt file
            if (args.Length > 4 && args[3] == "-o")
            {
                FileStream outfile = new FileStream(args[4], FileMode.Create, FileAccess.Write);
                writer = new StreamWriter(outfile);
                writer.WriteLine("Reading input from: " + args[0]);
                writer.WriteLine("Reading input from: " + args[1]);
                writer.WriteLine("Optimising tour length: Level 1 (algorithm 2)...");
                writer.WriteLine("Elapsed time: " + elapsed_time + " seconds");
                writer.WriteLine("Total tour time: " + time[0] + " Days " + time[1] + " Hours " + time[2] + " Minutes");
                writer.WriteLine("Tour length: " + Math.Round(Total_length, 3));
                Console.WriteLine("Reading input from: " + args[0]);
                Console.WriteLine("Reading input from: " + args[1]);
                Console.WriteLine("Optimising tour length: Level 1 (algorithm 2)...");
                Console.WriteLine("Elapsed time: " + elapsed_time + " seconds");
                Console.WriteLine("Total tour time: " + time[0] + " Days " + time[1] + " Hours " + time[2] + " Minutes");
                Console.WriteLine("Tour length: " + Math.Round(Total_length, 3));
                Print_trip(true);
                writer.WriteLine("Saving intinerary to: " + args[4]);
                Console.WriteLine("Saving intinerary to: " + args[4]);
                writer.Close();
                outfile.Close();
            }

            // If no output file has been requested then print tour to console only
            else
            {
                Console.WriteLine("Reading input from: " + args[0]);
                Console.WriteLine("Reading input from: " + args[1]);
                Console.WriteLine("Optimising tour length: Level 1 (algorithm 2)...");
                Console.WriteLine("Elapsed time: " + elapsed_time + " seconds");
                Console.WriteLine("Total tour time: " + time[0] + " Days " + time[1] + " Hours " + time[2] + " Minutes");
                Console.WriteLine("Tour length: " + Math.Round(Total_length, 3));
                Print_trip(false);
                Console.WriteLine("No output .txt file specified");
            }
                        
            // Await key press to end program
            Console.WriteLine("Press 'enter' to exit...");
            Console.ReadLine();
        }

        // Extract data from mail file and create new station classes
        private static void Makestations(string station)
        {
            // file reading
            try
            {
                FileStream readfile = new FileStream(station, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(readfile);
                string line;

                if (new FileInfo(station).Length == 0)
                {
                    Console.WriteLine(station + " is empty");
                    Console.WriteLine("Press 'enter' to exit");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    Station x = new Station(parts[0], short.Parse(parts[1]), short.Parse(parts[2]));
                    stationList.Add(x);
                }
                reader.Close();
                readfile.Close();
            }

            catch (FormatException F)
            {
                Console.WriteLine("Contents of " + station + " are not in the correct format");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (IndexOutOfRangeException IOOR)
            {
                Console.WriteLine("Contents of " + station + " have either too many or too little arguments");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (FileNotFoundException FNF)
            {
                Console.WriteLine("Mail file does not exist");
                Console.WriteLine("Input arguments as a minimum requirement should comprise:");
                Console.WriteLine("mail file (.txt)   plane file (.txt)   tour start time in 00:00 24h format");
                Console.WriteLine("Optional input argument for output file (must be preceeded by output command -o):");
                Console.WriteLine("-o output file (.txt)");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (DirectoryNotFoundException DiNF)
            {
                Console.WriteLine(station + " directory can not be found");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (DriveNotFoundException DrNF)
            {
                Console.WriteLine(station + " drive can not be found");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (PathTooLongException PTL)
            {
                Console.WriteLine(station + " path is longer than the system-defined maximum length.");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        // Extract data from plane file and create new plane class
        private static void Makeplane(string plane)
        {
            // file reading
            try
            {
                FileStream readfile = new FileStream(plane, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(readfile);
                string line;

                if (new FileInfo(plane).Length == 0)
                {
                    Console.WriteLine(plane + " is empty");
                    Console.WriteLine("Press 'enter' to exit");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    Plane x = new Plane(double.Parse(parts[0]), short.Parse(parts[1]), short.Parse(parts[2]), short.Parse(parts[3]), short.Parse(parts[4]));
                    planeList.Add(x);
                }
                reader.Close();
                readfile.Close();
            }
            catch (FormatException F)
            {
                Console.WriteLine("Contents of " + plane + " are not in the correct format");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (IndexOutOfRangeException IOOR)
            {
                Console.WriteLine("Contents of " + plane + " have either too many or too little arguments");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (FileNotFoundException FNF)
            {
                Console.WriteLine("Plane file does not exist");
                Console.WriteLine("Input arguments as a minimum requirement should comprise:");
                Console.WriteLine("mail file (.txt)   plane file (.txt)   tour start time in 00:00 24h format");
                Console.WriteLine("Optional input argument for output file (must be preceeded by output command -o):");
                Console.WriteLine("-o output file (.txt)");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (DirectoryNotFoundException DiNF)
            {
                Console.WriteLine(plane + " directory can not be found");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (DriveNotFoundException DrNF)
            {
                Console.WriteLine(plane + " drive can not be found");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            catch (PathTooLongException PTL)
            {
                Console.WriteLine(plane + " path is longer than the system-defined maximum length.");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }

            catch (NotSupportedException NS)
            {
                Console.WriteLine("Input arguments are of an invalid format");
                Console.WriteLine("Input arguments as a minimum requirement should comprise:");
                Console.WriteLine("mail file (.txt)   plane file (.txt)   tour start time in 00:00 24h format");
                Console.WriteLine("Optional input argument for output file (must be preceeded by output command -o):");
                Console.WriteLine("-o output file (.txt)");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
                                  
        }

        // Return optimised route order as an array
        private static int[] Level1(ref double Total_length)
        {
            int count = stationList.Count;
            int[] order = new int[count + 1];

            for (int x = 1; x < count; x++)
            {
                int[] order2 = new int[count + 1];
                double Len = 0;
                for (int y = 1; y <= x; y++)
                {
                    double len = 0;
                    int[] order1 = new int[count + 1];
                    order.CopyTo(order1, 0);
                    for (int z = x; z > y; z--)
                    {
                        order1[z] = order1[z - 1];
                    }
                    order1[y] = x;
                    for (int z = 0; z <= x; z++)
                    {
                        int x1 = stationList[order1[z]].Getx();
                        int x2 = stationList[order1[z + 1]].Getx();
                        int y1 = stationList[order1[z]].Gety();
                        int y2 = stationList[order1[z + 1]].Gety();
                        len += Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
                    }
                    if (len < Len || Len == 0)
                    {
                        order1.CopyTo(order2, 0);
                        Len = len;
                    }
                }
                order2.CopyTo(order, 0);
                Total_length = Len;

            }
            //Array.Reverse(order);
            return order;
        }

        // Unsuccessful attempt at level 2
        /*
        private static int[] level2(int[] input)
        {
            int count = input.Length;
            int[] output = new int[count];
            input.CopyTo(output, 0);
            for (int x = 2; x < count; x++) // Loop for each station
            {
                int[] order = new int[count]; // Create empty order array the same size as the input array
                output.CopyTo(order, 0);

                for (int y = x; y > 1; y--)
                {
                    order[y] = order[y-1];
                }
                order[1] = input[x];

                for (int y = 1; y < count-1; y++)
                {
                    double len = 0;
                    order[y] = order[y + 1];
                    order[y+1] = input[x];
                    
                    for (int z = 1; z <= count-1; z++)
                    {
                        int x1 = stationList[order[z - 1]].Getx();
                        int x2 = stationList[order[z]].Getx();
                        int y1 = stationList[order[z - 1]].Gety();
                        int y2 = stationList[order[z]].Gety();
                        len += Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
                    }
                    if (len < Total_length)
                    {
                        order.CopyTo(output, 0);
                        Total_length = len;
                        x = 2;
                        y = count;
                    }
                }
            }
            return output;
        }
        */

        // Create new tour classes
        private static int[] Maketour(int[] order, string args)
        {
            string[] words = args.Split(':');
            int starthours = 0;
            try
            {
                starthours = Int16.Parse(words[0]);
            }
            catch (System.FormatException FE)
            {
                Console.WriteLine("Argument for tour start time absent or incorrect");
                Console.WriteLine("Input arguments as a minimum requirement should comprise:");
                Console.WriteLine("mail file (.txt)   plane file (.txt)   tour start time in 00:00 24h format");
                Console.WriteLine("Optional input argument for output file (must be preceeded by output command -o):");
                Console.WriteLine("-o output file (.txt)");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
                Environment.Exit(0);
            }
            int startmins = Int16.Parse(words[1]) + (starthours * 60);
            int speed = planeList[0].Getspeed();
            int to_time = planeList[0].Getto_time();
            int l_time = planeList[0].Getl_time();
            int rf_time = planeList[0].Getrf_time();
            int depart = 0;
            int arrive = 0;
            int fuelcheck = 0;
            int[] time = new int[3];
            int total_mins = 0;

            for (int x = 0; x < order.Length - 1; x++)
            {
                string from = stationList[order[x]].Getname();
                string to = stationList[order[x + 1]].Getname();
 
                int x1 = stationList[order[x]].Getx();
                int x2 = stationList[order[x + 1]].Getx();
                int y1 = stationList[order[x]].Gety();
                int y2 = stationList[order[x + 1]].Gety();
                double Duration = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
                Duration = Duration / speed * 60;
                Duration = Math.Round(Duration, MidpointRounding.AwayFromZero);
                int duration = Convert.ToInt16(Duration) + to_time + l_time;
                total_mins += duration;
                depart = 0;

                if (fuelcheck + duration > planeList[0].Getrange() * 60)
                {
                    total_mins += planeList[0].Getrf_time();
                    fuelcheck = 0;
                    depart += planeList[0].Getrf_time();
                    arrive += planeList[0].Getrf_time();
                }
                fuelcheck += duration;
                if (x == 0)
                {
                    depart = startmins;
                    arrive = depart + duration;
                }
                else
                {
                    depart += tourList[x - 1].Getarrive();
                    arrive += duration;
                }
                if (depart >= 1440)
                {
                    depart -= 1440;
                }
                if (arrive >= 1440)
                {
                    arrive -= 1440;
                }
                Tour y = new Tour(from, to, depart, arrive, duration);
                tourList.Add(y);
            }
            time[0] = total_mins / 1440;
            time[1] = (total_mins % 1440) / 60;
            time[2] = total_mins % 60;
            return time;
        }

        // Print program output
        private static void Print_trip(bool txt)
        {
            if (txt == true)
            {
                int fuelcheck = 0;
                for (int x = 0; x < stationList.Count; x++)
                {
                    if (fuelcheck + tourList[x].Getduration() > planeList[0].Getrange() * 60)
                    {
                        writer.WriteLine("*** refuel " + planeList[0].Getrf_time() + " mins ***");
                        Console.WriteLine("*** refuel " + planeList[0].Getrf_time() + " mins ***");
                        fuelcheck = 0;
                        x -= 1;
                    }
                    else
                    {
                        int departmins = tourList[x].Getdepart() / 60;
                        int departhours = tourList[x].Getdepart() % 60;
                        int arrivemins = tourList[x].Getarrive() / 60;
                        int arrivehours = tourList[x].Getarrive() % 60;
                        writer.WriteLine(tourList[x].Getfrom() + "\t->\t" + tourList[x].Getto() + "\t" + departmins.ToString("D2") + ":" + departhours.ToString("D2") + "\t" + arrivemins.ToString("D2") + ":" + arrivehours.ToString("D2"));
                        Console.WriteLine(tourList[x].Getfrom() + "\t->\t" + tourList[x].Getto() + "\t" + departmins.ToString("D2") + ":" + departhours.ToString("D2") + "\t" + arrivemins.ToString("D2") + ":" + arrivehours.ToString("D2"));

                        fuelcheck += tourList[x].Getduration();
                    }
                }
            }
            if (txt == false)
            {
                int fuelcheck = 0;
                for (int x = 0; x < stationList.Count; x++)
                {
                    if (fuelcheck + tourList[x].Getduration() > planeList[0].Getrange() * 60)
                    {
                        Console.WriteLine("*** refuel " + planeList[0].Getrf_time() + " mins ***");
                        fuelcheck = 0;
                        x -= 1;
                    }
                    else
                    {
                        int departmins = tourList[x].Getdepart() / 60;
                        int departhours = tourList[x].Getdepart() % 60;
                        int arrivemins = tourList[x].Getarrive() / 60;
                        int arrivehours = tourList[x].Getarrive() % 60;
                        Console.WriteLine(tourList[x].Getfrom() + "\t->\t" + tourList[x].Getto() + "\t" + departmins.ToString("D2") + ":" + departhours.ToString("D2") + "\t" + arrivemins.ToString("D2") + ":" + arrivehours.ToString("D2"));
                        fuelcheck += tourList[x].Getduration();
                    }
                }
            }
        }
    }
}