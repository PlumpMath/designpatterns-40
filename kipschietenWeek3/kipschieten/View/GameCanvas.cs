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
        public void DrawMovingUnits(List<MovingUnit> movingUnits)
        {
            Children.Clear();
            foreach (var movingUnit in movingUnits)
            {
                drawUnit(movingUnit);
            }
        }

        public void DrawUnits(List<Unit> units)
        {
            foreach (var unit in units)
            {
                drawUnit(unit);
            }
        }

        private void drawUnit(Unit unit)
        {
            var unitBrush = new ImageBrush(new BitmapImage(new Uri(unit.UnitImage)));
            unitBrush.Freeze();

            var unitRectangle = new Rectangle
            {
                Height = unit.Height, 
                Width = unit.Width,
                Stroke = Brushes.Blue,
                StrokeThickness = 1
            };
            unitRectangle.SetValue(LeftProperty, unit.LeftPosition);
            unitRectangle.SetValue(TopProperty, unit.TopPosition);
            unitRectangle.Fill = unitBrush;

            Children.Add(unitRectangle);
        }
    }
}
