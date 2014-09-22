using kipschieten.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace kipschieten.Model
{
    class MouseCapture
    {
        private Dictionary<double, double> _clickedCoordinates;
        private GameCanvas _playGrid;

        public MouseCapture(GameCanvas playGrid)
        {
            _clickedCoordinates = new Dictionary<double, double>();

            _playGrid           = playGrid;
            _playGrid.MouseUp  += gridClicked;
        }

        private void gridClicked(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(_playGrid);
            if(!_clickedCoordinates.ContainsKey(clickPoint.X)) 
                _clickedCoordinates.Add(clickPoint.X, clickPoint.Y);
        }

        public Dictionary<double, double> getClicks()
        {
            Dictionary<double, double> temp = new Dictionary<double,double>(_clickedCoordinates);
            _clickedCoordinates.Clear();

            return temp;
        }
    }
}
