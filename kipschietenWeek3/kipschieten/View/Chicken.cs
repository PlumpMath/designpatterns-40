using kipschieten.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace kipschieten.View
{
    class Chicken : MovingUnit
    {
        public Chicken(double xPos, double yPos)
        {
            UnitImage = AppDomain.CurrentDomain.BaseDirectory + "Resources\\chicken.png";
            CanBeShot = true;
            CanMove   = true;

            Random random = new Random();

            while (_xStep == 0)
                _xStep = random.Next(1,1);
            while (_yStep == 0)
                _yStep = random.Next(1,1);

            setLocation(xPos, yPos);
        }
    }
}