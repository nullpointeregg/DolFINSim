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
    public class Stone
    {
        private readonly IntegerVector2 m_indexPosition;
        private readonly Player m_player;
        private readonly Ellipse m_piece;
        private readonly Panel m_panel;
        public Stone(IntegerVector2 _position, Player _player, Ellipse piece, Panel panel)
        {
            m_indexPosition = _position;
            m_player = _player;
            m_piece = piece;
            m_panel = panel;

            Display();
        }
        public void PlaceStone(in Player[][] _grid)
        {
            _grid[m_indexPosition.Y][m_indexPosition.X] = m_player;
        }
        public void Display()
        {
            m_panel.Children.Add(m_piece);

        }
        public void Destroy()
        {
            m_panel.Children.Remove(m_piece);
        }
        /*
        private Ellipse GetEllipse(IntegerVector2 _position, float _factor, SolidColorBrush _fillColor, SolidColorBrush _strokeColor)
        {
            Ellipse _piece = new Ellipse
            {
                Width = m_cellSideLength * _factor,
                Height = m_cellSideLength * _factor,
                Fill = _fillColor,
                Stroke = _strokeColor
            };
            Point _point = GetPoint(_position, false);
            Canvas.SetLeft(_piece, _point.X - m_cellSideLength * _factor / 2);
            Canvas.SetTop(_piece, _point.Y - m_cellSideLength * _factor / 2);

            return _piece;
        }
        */
    }

    public enum Player
    {

        None = -1, // 바둑판의 빈 공간
        Player1,
        Player2,
        Player3,
        Player4,
        Player5,
        Player6,
        Player7,
        Player8,
        Max,
        // Bot
        BotGeneral = 100,
        Bot1,
        Bot2
    }
}
