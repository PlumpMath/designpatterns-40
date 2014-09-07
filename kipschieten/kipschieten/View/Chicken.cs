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
    class Chicken : Image
    {
        public bool isShot   = false;
        public int xLocation = 0;
        public int yLocation = 0;

        private double _xStep = 0;
        private double _yStep = 0;

        public Chicken()
        {
            Random random = new Random();
            // Calculate random direction
            // divide by 60 because 60 fps
            double xStep = random.Next(-2, 3);
            _xStep = xStep / 60;

            double yStep = random.Next(-2, 3);
            _yStep = yStep / 60;

            MouseUp += Chicken_MouseUp;
        }

        private void Chicken_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isShot = true;
        }

        public void Move()
        {
            // margin for location in grid
            //Margin = new Thickness(20,20,0,0);
        }

        public void setLocation(double xPos, double yPos)
        {
            // set location when new added
            Margin = new Thickness(xPos, yPos, 0, 0);
        }
    }
}