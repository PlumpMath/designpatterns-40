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
    class Chicken
    {
        public bool isShot   = false;
        public int xLocation = 0;
        public int yLocation = 0;

        private double _xStep = 0;
        private double _yStep = 0;

        public Thickness margin;
        public String imgChickens = AppDomain.CurrentDomain.BaseDirectory + "View\\chicken.png";

        public Chicken()
        {
            Random random = new Random();

            double xStep = random.Next(-4, 4);
            _xStep = xStep;

            double yStep = random.Next(-4, 4);
            _yStep = yStep;
        }

        public void Move()
        {
            // margin for location in grid
            margin.Left += _xStep;
            margin.Top += _yStep;
        }

        public void setLocation(double xPos, double yPos)
        {
            // set location when new added
            margin = new Thickness(xPos, yPos, 0, 0);
        }
    }
}