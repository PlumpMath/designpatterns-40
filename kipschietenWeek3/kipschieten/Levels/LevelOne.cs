using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.Levels
{
    class LevelOne : Level
    {
        public LevelOne()
        {
            UnitsToHit = 5;
            MovingUnitAmount = 5;
            NonMovingUnitAmount = 5;
            MaxSpeed = 4;
            MinSpeed = 2;
        }
    }
}
