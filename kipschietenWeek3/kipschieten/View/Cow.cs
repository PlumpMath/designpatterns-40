using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.View
{
    class Cow : MovingUnit
    {

        public Cow(double xPos, double yPos)
        {
            UnitImage = AppDomain.CurrentDomain.BaseDirectory + "Resources\\cow.png";
            CanBeShot = true;
            CanMove   = true;

            Random random = new Random();

            while (_xStep == 0)
                _xStep = random.Next(-4, 4);
            while (_yStep == 0)
                _yStep = random.Next(-4, 4);

            setLocation(xPos, yPos);
        }
    }
}
