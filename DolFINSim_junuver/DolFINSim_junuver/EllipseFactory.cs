using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DolFINSim_junuver
{
    public static class EllipseFactory
    {
        public static Ellipse MakeEllipse(double _diameter, Point _position, SolidColorBrush _fillColor, SolidColorBrush _strokeColor)
        {
            Ellipse _ellipse = new Ellipse()
            {
                Width = _diameter,
                Height = _diameter,
                Fill = _fillColor,
                Stroke = _strokeColor
            };
            Canvas.SetLeft(_ellipse, _position.X);
            Canvas.SetRight(_ellipse, _position.Y);

            return _ellipse;
        }
    }
}
