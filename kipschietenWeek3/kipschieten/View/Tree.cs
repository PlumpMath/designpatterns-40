﻿using kipschieten.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.View
{
    class Tree : Unit
    {
        public Tree(double xPos, double yPos)
        {
            UnitImage = AppDomain.CurrentDomain.BaseDirectory + "Resources\\tree.png";
            CanBeShot = false;
            CanMove = false;

            setLocation(xPos, yPos);
        }
    }
}
