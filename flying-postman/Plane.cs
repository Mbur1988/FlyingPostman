using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flying_postman
{
    class Plane
    {
        public double Range { get; set; } // Range of plane
        public int Speed { get; set; } // Speed of plane
        public int TO_time { get; set; } // Time it takes for plane to take off
        public int L_time { get; set; } // Time it takes for plane to land
        public int RF_time { get; set; } // Time it takes for plane to refuel

        // // Method to get plane variables
        public Plane(double range, int speed, int to_time, int l_time, int rf_time)
        {
            Range = range;
            Speed = speed;
            TO_time = to_time;
            L_time = l_time;
            RF_time = rf_time;
        }

        // Method to return range
        public double Getrange()
        {
            return Range;
        }

        // Method to return speed
        public int Getspeed()
        {
            return Speed;
        }

        // Method to return take off time
        public int Getto_time()
        {
            return TO_time;
        }

        // Method to return landing time
        public int Getl_time()
        {
            return L_time;
        }

        // Method to return refuel time
        public int Getrf_time()
        {
            return RF_time;
        }
    }
}
