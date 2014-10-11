using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kipschieten.Levels;

namespace kipschieten.Model
{
    static class LevelFactory
    {
        public static Level CreateLevel(LevelEnum level)
        {
            string className = "kipschieten.Levels." + level.ToString();

            Type t = Type.GetType(className);
            return Activator.CreateInstance(t) as Level;
        }
    }
}
