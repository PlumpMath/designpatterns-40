using kipschieten.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kipschieten.Model
{
   static class UnitFactory
    {

        public static Unit CreateUnit(UnitEnum unitType, double xPos, double yPos)
        {
            string className = "kipschieten.View." + unitType.ToString();

            Type t = Type.GetType(className);
            return Activator.CreateInstance(t, xPos, yPos) as Unit;
        }
    }
}
