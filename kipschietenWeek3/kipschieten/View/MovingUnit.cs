using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.View
{
    abstract class MovingUnit : Unit
    {
        private bool _isShot = false;
        public bool isShot { get { return _isShot; } set { _isShot = value; } }

        public int xLocation = 0;
        public int yLocation = 0;

        public double _xStep = 0;
        public double _yStep = 0;

        public void Move()
        {
            // margin for location in grid
            LeftPosition += _xStep;
            TopPosition += _yStep;
        }

        public void ChangePosition()
        {
            _xStep = -_xStep;
            _yStep = -_yStep;
            LeftPosition += _xStep;
            TopPosition += _yStep;
        }
    }
}
