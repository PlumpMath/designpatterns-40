using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kipschieten.View
{
    class GameCanvas : Canvas
    {

        public void DrawUnits(List<Unit> units)
        {
            // remove previous units
            Children.Clear();

            foreach (Unit unit in units)
            {
                ImageBrush unitBrush = new ImageBrush(new BitmapImage(new Uri(unit.UnitImage)));
                unitBrush.Freeze();

                Rectangle unitRectangle = new Rectangle();
                unitRectangle.Height = unit.Height;
                unitRectangle.Width = unit.Width;
                unitRectangle.SetValue(Canvas.LeftProperty, unit.LeftPosition);
                unitRectangle.SetValue(Canvas.TopProperty, unit.TopPosition);
                unitRectangle.Fill = unitBrush;

                Children.Add(unitRectangle);
            }
        }
    }
}
