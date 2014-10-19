using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.View
{
    class Stone :Unit
    {
        public Stone(double xPos, double yPos)
        {
            UnitImage = AppDomain.CurrentDomain.BaseDirectory + "Resources\\stone.png";
            CanBeShot = false;
            CanMove = false;

            setLocation(xPos, yPos);
        }
    }
}
