using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flying_postman
{
    class Station
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Station(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
        }

        public string Getname()
        {
            return Name;
        }

        public int Getx()
        {
            return X;
        }

        public int Gety()
        {
            return Y;
        }
    }
}
