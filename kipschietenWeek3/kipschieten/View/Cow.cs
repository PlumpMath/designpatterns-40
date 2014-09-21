using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.View
{
    class Cow : Unit
    {
        private bool _isShot = false;
        public bool isShot { get { return _isShot; } set { _isShot = value; } }

        public int xLocation = 0;
        public int yLocation = 0;

        private double _xStep = 0;
        private double _yStep = 0;

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

        public void Move()
        {
            // margin for location in grid
            LeftPosition += _xStep;
            TopPosition += _yStep;
        }

        public void setLocation(double xPos, double yPos)
        {
            // set location when new added
            LeftPosition = xPos;
            TopPosition = yPos;
        }
    }
}
