using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DolFINSim_junuver
{
    public class Board : Grid
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
            new SolidColorBrush(Colors.Yellow),
            new SolidColorBrush(Colors.Transparent)
        };

        private int m_currentMoveIndex = 0;
        private readonly double m_cellSideLength;
        private readonly Point m_pivot;
        private readonly Policy m_policy;
        private readonly List<Stone> m_stones;
        private readonly Panel m_panel;

        #region Public Methods
        public void PlaceNew(Point _rawPoint)
        {
            IntegerVector2 _rounded = GetRoundedIndex(_rawPoint);
            PlayerEnum _nextPlayer = m_policy.PlayerCalculationPolicy.GetPlayer(m_currentMoveIndex);

            Stone _stone = new Stone(_rounded, _nextPlayer, GetEllipse(m_cellSideLength, _rounded, 1.0f, ColorTable[(int)_nextPlayer], ColorTable[0]));
            Place(_stone, m_currentMoveIndex, true);
        }
        public void ShowFromCurrentIndex(int _difference)
        {
            int _index = m_currentMoveIndex + _difference;
            if (_index < 0) 
                _index = 0;
            if (_index > m_stones.Count)
                _index = m_stones.Count;

            PlaceTo(_index);
        }
        public void DrawBoard()
        {
            Rectangle _boardRect = new Rectangle
            {
                Width = m_panel.ActualWidth,
                Height = m_panel.ActualHeight,
                Fill = new SolidColorBrush(Colors.BurlyWood), // #FFDEB887
                Stroke = ColorTable[(int)ColorEnum.Yellow],
                StrokeThickness = 6
            };
            m_panel.Children.Add(_boardRect);

            double _gridWidth = m_cellSideLength * (Width - 1);
            double _gridHeight = m_cellSideLength * (Height - 1);

            // Drawing Lines
            for (int i = 0; i <= Width - 1; i++)
            {
                var _line = new Line
                {
                    X1 = m_pivot.X + m_cellSideLength * i,
                    Y1 = m_pivot.Y,
                    X2 = m_pivot.X + m_cellSideLength * i,
                    Y2 = m_pivot.Y + _gridHeight,
                    Stroke = ColorTable[(int)ColorEnum.Black],
                    StrokeThickness = 2
                };
                m_panel.Children.Add(_line);
            }
            for (int i = 0; i <= Height - 1; i++)
            {
                var _line = new Line
                {
                    X1 = m_pivot.X,
                    Y1 = m_pivot.Y + m_cellSideLength * i,
                    X2 = m_pivot.X + _gridWidth,
                    Y2 = m_pivot.Y + m_cellSideLength * i,
                    Stroke = ColorTable[(int)ColorEnum.Black],
                    StrokeThickness = 2
                };
                m_panel.Children.Add(_line);
            }

            if (Width == 19 && Height == 19)
                Draw9Points();

            //Drawing Stones
            PlaceTo(m_stones.Count);
        }
        public void UpdateLabels(params TextBlock[] _textBlocks)
        {
            for (int i = 0; i < _textBlocks.Length; i++)
            {
                PlayerEnum _player = m_policy.PlayerCalculationPolicy.GetPlayer(m_currentMoveIndex + i);
                _textBlocks[i].Text = $"{m_currentMoveIndex + i + 1}. {_player}";
                _textBlocks[i].Foreground = ColorTable[(int)_player];
            }
        }                                                  
        private void Place(Stone _stone, int _currentMoveIndex, bool _isNew)
        {
            if (_stone.IsIllegal(m_stones.Take(_currentMoveIndex).ToArray(), m_policy))
            {
                return;
            }
            Stone[] _deadStones = _stone.FindDead(m_stones.Take(_currentMoveIndex).ToArray(), m_policy);
            if (_deadStones.Count() > 0)
            {
                _stone.Display(m_panel);
                Array.ForEach(_deadStones, s => s.Destroy(m_panel));
                if (_isNew)
                {
                    if (_currentMoveIndex != m_stones.Count)
                        TakeList(_currentMoveIndex);
                    m_stones.Add(_stone);
                    m_currentMoveIndex = _currentMoveIndex + 1;

                }
            }
            else if (!_stone.IsForbidden(m_stones.ToArray(), m_policy))
            {
                _stone.Display(m_panel);
                if (_isNew)
                {
                    if (_currentMoveIndex != m_stones.Count)
                        TakeList(_currentMoveIndex);
                    m_stones.Add(_stone);
                    m_currentMoveIndex = _currentMoveIndex + 1;
                }
            }
        }
        private Point GetPoint(IntegerVector2 _position,  bool _isForStone)
        {
            return new Point(m_pivot.X + m_cellSideLength * _position.X - (_isForStone ? m_cellSideLength / 2 : 0),
                m_pivot.Y + m_cellSideLength * (Height - _position.Y - 1) - (_isForStone ? m_cellSideLength / 2 : 0));
        }
        private IntegerVector2 GetRoundedIndex(Point _rawPoint)
        {
            double _xLeftLimit = m_pivot.X - m_cellSideLength / 2;
            double _xRightLimit = m_pivot.X + m_cellSideLength * (Width * 2 - 1) / 2;
            double _yTopLimit = m_pivot.Y - m_cellSideLength / 2;
            double _yBottomLimit = m_pivot.Y + m_cellSideLength * (Height * 2 - 1) / 2;

            if (_rawPoint.X < _xLeftLimit ||
                _rawPoint.X > _xRightLimit ||
                _rawPoint.Y < _yTopLimit ||
                _rawPoint.Y > _yBottomLimit)
                return new IntegerVector2(-1, -1);

            int _indexX = -1, _indexY = -1;
            for (int i = 1; i <= Width; i++)
            {
                if (_rawPoint.X <= _xLeftLimit + m_cellSideLength * i)
                {
                    _indexX = i;
                    break; 
                }
            }
            for (int i = 1; i <= Height; i++)
            {

                if (_rawPoint.Y <= _yTopLimit + m_cellSideLength * i)
                {
                    _indexY = i;
                    break;
                }
            }

            return new IntegerVector2(_indexX - 1, Height - _indexY);
        }
        #endregion
        private void PlaceTo(int _moveCount)
        {
            if (_moveCount > m_stones.Count || _moveCount < 0)
                return;
            
            ClearBoard();
            for (int i = 0; i < _moveCount; i++)
            {
                Place(m_stones[i], i, false);
            }
            for (int i = _moveCount; i < m_stones.Count; i++)
            {
                m_stones[i].Destroy(m_panel);
            }

            m_currentMoveIndex = _moveCount;
        }
        private void Draw9Points()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Ellipse _circle = GetEllipse(m_cellSideLength, new IntegerVector2(3 + 6 * j, 3 + 6 * i), 0.3333333f, ColorTable[(int)ColorEnum.Black], ColorTable[(int)ColorEnum.Black]);
                    m_panel.Children.Add(_circle);
                }
            }
        }
        private Ellipse GetEllipse(double _cellSideLength, IntegerVector2 _position, float _factor, SolidColorBrush _fillColor, SolidColorBrush _strokeColor)
        {
            Ellipse _piece = new Ellipse
            {
                Width = _cellSideLength * _factor,
                Height = _cellSideLength * _factor,
                Fill = _fillColor,
                Stroke = _strokeColor
            };
            Point _point = GetPoint(_position, false);
            Canvas.SetLeft(_piece, _point.X - _cellSideLength * _factor / 2);
            Canvas.SetTop(_piece, _point.Y - _cellSideLength * _factor / 2);

            return _piece;
        }
        private double GetCellSideLength(Panel _panel)
        {
            return Math.Min(_panel.ActualWidth / (Width + 2), _panel.ActualHeight / (Height + 2));
        }
        private Point GetPivot(double _cellSideLength, Panel _panel)
        {
            double _gridWidth = _cellSideLength * (Width - 1);
            double _gridHeight = _cellSideLength * (Height - 1);

            double _pivotX = (_panel.ActualWidth - _gridWidth) / 2;
            double _pivotY = (_panel.ActualHeight - _gridHeight) / 2;
            return new Point(_pivotX, _pivotY);
        }
        private void TakeList(int _toIndex)
        {
            m_stones.RemoveRange(_toIndex, m_stones.Count - _toIndex);
        }
        private void ClearBoard()
        {
            m_stones.ForEach(s => s.Destroy(m_panel));
        }
        public Board(Board _board) : this(_board.Width, _board.Height, _board.m_stones, _board.m_panel, _board.m_policy)
        {

        }
        public Board(int _width, int _height, List<Stone> _previousStones, Panel _panel, Policy _policy) : this(_width, _height, _panel, _policy)
        {
            List<Stone> _newStones = new List<Stone>();
            for (int i = 0; i < _previousStones.Count; i++)
            {
                _newStones.Add(new Stone(_previousStones[i], (IntegerVector2 _position, SolidColorBrush _fillColor) 
                    => GetEllipse(GetCellSideLength(_panel), _position, 1.0f, _fillColor, ColorTable[(int)ColorEnum.Black])));
            }
            m_stones = _newStones;
        }
        public Board(int _width, int _height, Panel _panel, Policy _policy) : base(_width, _height)
        {
            m_stones = new List<Stone>();
            m_cellSideLength = GetCellSideLength(_panel);
            m_pivot = GetPivot(m_cellSideLength, _panel);
            m_panel = _panel;
            m_policy = _policy;
        }
    }
}
