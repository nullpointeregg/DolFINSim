using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DolFINSim_junuver
{
    public class FloatingDropdown
    {
        private readonly Panel m_panel;
        private readonly Rectangle m_rect;
        private readonly Button[] m_buttons;
        public FloatingDropdown(Panel _panel, Point _pivot, string[] _names, RoutedEventHandler[] _onClickActions) : this(120.0, 25.0, 15.0, _panel, _pivot, _names, _onClickActions)
        {
        }

        public FloatingDropdown(double _width, double _height, double _fontSize, Panel _panel, Point _pivot, string[] _names, RoutedEventHandler[] _onClickActions)
        {
            m_panel = _panel;
            m_rect = new Rectangle()
            {
                Width = _width,
                Height = _height * _names.Length,
                Fill = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(_pivot.X, _pivot.Y, 0, 0),
                Stroke = new SolidColorBrush(Colors.Gray),
            };
            _panel.Children.Add(m_rect);
            List<Button> _buttonList = new List<Button>();

            for (int i = 0; i < _names.Length; i++)
            {
                Button _button = new Button()
                {
                    Content = _names[i],
                    Width = _width * 0.95f,
                    Height = _height * 0.95f,
                    FontSize = _fontSize,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = new SolidColorBrush(Colors.White),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(_pivot.X + _width * 0.025f, _pivot.Y + _height * i + _height * 0.025f, 0, 0)
                };
                _button.Click += _onClickActions[i];
                _panel.Children.Add(_button);
                _buttonList.Add(_button);
            }

            m_buttons = _buttonList.ToArray();
        }

        public void Destroy()
        {
            m_panel.Children.Remove(m_rect);
            foreach (var _button in m_buttons)
            {
                m_panel.Children.Remove(_button);
            }
        }
    }
}
