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

        private readonly IntegerVector2 m_position;
        private readonly Player m_player;
        private readonly Ellipse m_piece;
        public Stone(Stone _stone, Func<IntegerVector2, SolidColorBrush, Ellipse> _getEllipseFunc) : this(_stone, _getEllipseFunc(_stone.m_position, ColorTable[(int)_stone.m_player]))
        {

        }
        public Stone(Stone _stone, Ellipse _piece) : this(_stone.m_position, _stone.m_player, _piece)
        {

        }
        public Stone(IntegerVector2 _position, Player _player, Ellipse _piece)
        {
            m_position = _position;
            m_player = _player;
            m_piece = _piece;
        }

        public void PlaceStone(in Player[][] _grid, Panel m_panel)
        {
            if (IsOnDisplay(m_panel))
                _grid[m_position.Y][m_position.X] = m_player;
        }
        public void Display(Panel _panel)
        {
            _panel.Children.Add(m_piece);
        }
        public void Destroy(Panel _panel)
        {
            _panel.Children.Remove(m_piece);
        }
        public Stone[] FindDead(Stone[] _placedStones, Policy _policy)
        {
            return _policy.BoardUpdatePolicy.FindDead(m_player, m_position, _placedStones);
        }
        public bool IsIllegal(Stone[] _placedStones, Policy _policy)
        {
            return _policy.ForbiddenMovePolicy.IsIllegal(m_player, m_position, _placedStones);
        }
        public bool IsForbidden(Stone[] _placedStones, Policy _policy)
        {
            return _policy.ForbiddenMovePolicy.IsForbidden(m_player, m_position, _placedStones);
        }
        public bool IsOnDisplay(IntegerVector2 _position, Panel _panel)
        {
            return _position == m_position && IsOnDisplay(_panel);
        }
        private bool IsOnDisplay(Panel _panel)
        {
            return _panel.Children.Contains(m_piece);
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
