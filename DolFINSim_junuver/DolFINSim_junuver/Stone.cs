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
        private static readonly SolidColorBrush[] s_colorTable = new SolidColorBrush[]
        {
            new SolidColorBrush(Colors.Black),
            new SolidColorBrush(Colors.White),
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Lime),
            new SolidColorBrush(Colors.Aqua),
            new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.Fuchsia),
            new SolidColorBrush(Colors.Yellow),
            new SolidColorBrush(Colors.Transparent)
        };

        private readonly IntegerVector2 m_position;
        private readonly Player m_player;
        private readonly Ellipse m_piece;

        public void PlaceStone(in PlayerEnum[][] _grid, Panel m_panel)
        {
            if (IsOnDisplay(m_panel))
                _grid[m_position.Y][m_position.X] = m_player.GetPlayer();
        }
        public void Display(Panel _panel)
        {
            if (!IsOnDisplay(_panel))
                _panel.Children.Add(m_piece);/*
            else
                throw new Exception("이미 돌이 놓인 상태에서 동일한 돌을 또 놓으려고 함!");*/
        }
        public void Destroy(Panel _panel)
        {
            if (IsOnDisplay(_panel))
                _panel.Children.Remove(m_piece);/*
            else
                throw new Exception("돌은 이미 제거됐는데 동일한 돌을 또 제거하려고 함!");*/
        }
        public Stone[] FindDead(Stone[] _placedStones, Policy _policy)
        {
            return _policy.BoardUpdatePolicy.FindDead(m_player.GetPlayer(), m_position, _placedStones);
        }
        public bool IsIllegal(Stone[] _placedStones, Policy _policy)
        {
            return _policy.ForbiddenMovePolicy.IsIllegal(m_player.GetPlayer(), m_position, _placedStones);
        }
        public bool IsForbidden(Stone[] _placedStones, Policy _policy)
        {
            return _policy.ForbiddenMovePolicy.IsForbidden(m_player.GetPlayer(), m_position, _placedStones);
        }
        public bool IsOnDisplay(IntegerVector2 _position, Panel _panel)
        {
            return _position == m_position && IsOnDisplay(_panel);
        }
        private bool IsOnDisplay(Panel _panel)
        {
            return _panel.Children.Contains(m_piece);
        }
        public Stone(Stone _stone, Func<IntegerVector2, SolidColorBrush, Ellipse> _getEllipseFunc) : this(_stone, _getEllipseFunc(_stone.m_position, s_colorTable[(int)_stone.m_player.GetPlayer()]))
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
    }
}
