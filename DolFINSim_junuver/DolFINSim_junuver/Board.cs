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
            new SolidColorBrush(Colors.Yellow)
        };

        private int m_currentMoveIndex = 0;
        private double m_cellSideLength;
        private Point m_pivot;
        private readonly List<Stone> m_stones;
        private readonly Panel m_panel;

        public void PlaceNew(Point _rawPoint, PlayerCalculationPolicy _policy)
        {
            IntegerVector2 _rounded = GetRoundedIndex(_rawPoint);
            if (_rounded.X == -1 || _rounded.Y == -1)
                return;
            Player _nextPlayer = _policy.GetPlayer(m_stones.Count());
            Stone _stone = new Stone(_rounded, _nextPlayer, GetEllipse(_rounded, 1.0f, ColorTable[(int)_nextPlayer], ColorTable[0]), m_panel);
            _stone.Display(m_panel);

            m_stones.Add(_stone);
            m_currentMoveIndex = m_stones.Count;
        }
        public void DisplayTo(int _index)
        {
            if (_index >= m_stones.Count)
                return;

            for (int i = 0; i < _index; i++)
            {
                m_stones[i].Display(m_panel);
            }
            for (int i = _index; i < m_stones.Count; i++)
            {
                m_stones[i].Destroy(m_panel);
            }

            m_currentMoveIndex = _index;
        }
        public void Place(Stone _stone)
        {
            _stone.Display(m_panel);
        }
        public void Remove(Stone _stone)
        {
            _stone.Destroy(m_panel);
            m_stones.Remove(_stone);
        }
        public void RemoveAt(int _index)
        {
            Stone _stone = m_stones[_index];
            _stone.Destroy(m_panel);
            m_stones.RemoveAt(_index);
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
        }
        public Point GetPoint(IntegerVector2 _position,  bool _isForStone)
        {
            return new Point(m_pivot.X + m_cellSideLength * _position.X - (_isForStone ? m_cellSideLength / 2 : 0),
                m_pivot.Y + m_cellSideLength * (Height - _position.Y - 1) - (_isForStone ? m_cellSideLength / 2 : 0));
        }
        public IntegerVector2 GetRoundedIndex(Point _rawPoint)
        {
            double _xLeftLimit = m_pivot.X - m_cellSideLength / 2;
            double _xRightLimit = m_pivot.X + m_cellSideLength * (Width * 2 + 1) / 2;
            double _yTopLimit = m_pivot.Y - m_cellSideLength / 2;
            double _yBottomLimit = m_pivot.Y + m_cellSideLength * (Height * 2 + 1) / 2;

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
        private void Draw9Points()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Ellipse _circle = GetEllipse(new IntegerVector2(3 + 6 * j, 3 + 6 * i), 0.3333333f, ColorTable[(int)ColorEnum.Black], ColorTable[(int)ColorEnum.Black]);
                    m_panel.Children.Add(_circle);
                }
            }
        }
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
        public Board(int _width, int _height, List<Stone> _stones, Panel _panel) : this(_width, _height, _panel)
        {
            m_stones = _stones;
        }
        public Board(int _width, int _height, Panel _panel) : base(_width, _height)
        {
            m_cellSideLength = GetCellSideLength(_panel);
            m_pivot = GetPivot(m_cellSideLength, _panel);
            m_panel = _panel;
        }
    }
}
