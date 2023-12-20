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
        private static readonly SolidColorBrush[] ColorTable = new SolidColorBrush[]
        {
            new SolidColorBrush(Colors.Black),
            new SolidColorBrush(Colors.White),
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Lime),
            new SolidColorBrush(Colors.Aqua),
            new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.Fuchsia),
            new SolidColorBrush(Colors.Yellow)
        };

        private readonly IntegerVector2 m_indexPosition;
        private readonly Player m_player;
        private readonly Ellipse m_piece;
        public Stone(Stone _stone, Func<IntegerVector2, SolidColorBrush, Ellipse> _getEllipseFunc) : this(_stone, _getEllipseFunc(_stone.m_indexPosition, ColorTable[(int)_stone.m_player]))
        {

        }
        public Stone(Stone _stone, Ellipse _piece) : this(_stone.m_indexPosition, _stone.m_player, _piece)
        {

        }
        public Stone(IntegerVector2 _position, Player _player, Ellipse _piece)
        {
            m_indexPosition = _position;
            m_player = _player;
            m_piece = _piece;
        }

        public void PlaceStone(in Player[][] _grid)
        {
            _grid[m_indexPosition.Y][m_indexPosition.X] = m_player;
        }
        public void Display(Panel _panel)
        {
            if (!_panel.Children.Contains(m_piece))
                _panel.Children.Add(m_piece);
        }
        public void Destroy(Panel _panel)
        {
            _panel.Children.Remove(m_piece);
        }
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
