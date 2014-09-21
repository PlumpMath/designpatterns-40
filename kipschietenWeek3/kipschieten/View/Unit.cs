using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.View
{
    abstract class Unit
    {
        public String UnitImage;

        public bool CanBeShot = false;
        public bool CanMove = false;

        public int Width = 50;
        public int Height = 50;

        public double LeftPosition;
        public double TopPosition;


    }
}
