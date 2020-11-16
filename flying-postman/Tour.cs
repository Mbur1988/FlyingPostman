using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flying_postman
{
    class Tour
    {
        public string From { get; set; } // Origin
        public string To { get; set; } // Destination
        public int Depart { get; set; } // Departure time
        public int Arrive { get; set; } // Arrival time
        public int Duration { get; set; } // Duration of trip in mins

        // Method to get tour variables
        public Tour(string from, string to, int depart, int arrive, int duration)
        {
            From = from;
            To = to;
            Depart = depart;
            Arrive = arrive;
            Duration = duration;
        }

        // Method to return origin
        public string Getfrom()
        {
            return From;
        }

        // Method to return destination
        public string Getto()
        {
            return To;
        }

        // Method to return departure time
        public int Getdepart()
        {
            return Depart;
        }

        // Method to return arrival time
        public int Getarrive()
        {
            return Arrive;
        }

        // Method to return duration of trip leg in mins
        public int Getduration()
        {
            return Duration;
        }
    }
}
