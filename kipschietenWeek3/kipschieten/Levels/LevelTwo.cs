using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.Levels
{
    class LevelTwo : Level
    {
        public LevelTwo()
        {
            UnitsToHit = 7;
            MovingUnitAmount = 7;
            NonMovingUnitAmount = 10;
            MaxSpeed = 6;
            MinSpeed = 3;
        }
    }
}
