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

            setLocation(xPos, yPos);
        }
    }
}
