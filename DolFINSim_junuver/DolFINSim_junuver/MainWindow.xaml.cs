﻿using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DolFINSim_junuver
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly SolidColorBrush[] ColorTable = new SolidColorBrush[]
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
        private Board m_board;
        private Policy m_policy;
        private Rectangle m_deselectToolRect;
        private FloatingDropdown m_currentOpenDropdown;


        #region Interactive Methods
        private void CanvasOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point _cursorPoint = e.GetPosition(this);
            _cursorPoint.Y -= 25;
            m_board.PlaceNew(_cursorPoint);
            m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });
        }
        private void OnClickFirstMoveButton(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(-20050906);
            m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });
        }
        private void OnClickBackward10Button(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(-10);
            m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });
        }
        private void OnClickBackwardButton(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(-1);
            m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });
        }
        public MainWindow()
        {
            InitializeComponent();
            MainCanvas.MouseDown += CanvasOnMouseDown;

            Panel _panel = DisplayGrid.Children
                .Cast<Panel>()
                .First(element => System.Windows.Controls.Grid.GetRow(element) == 0);
            PlayerCalculationPolicy _playerCalculationPolicy = new PlayerCalculationPolicy(2, 1);
            ForbiddenMovePolicy _forbiddenMovePolicy = new ForbiddenMovePolicy(19, 19, _panel, BoardUpdatePolicyEnum.Plus, ForbiddenMovePolicyEnum.Outside, ForbiddenMovePolicyEnum.Overlay, ForbiddenMovePolicyEnum.Ko, ForbiddenMovePolicyEnum.Suicide);
            BoardUpdatePolicy _boardUpdatePolicy = new BoardUpdatePolicy(19, 19, _panel, BoardUpdatePolicyEnum.Plus);

            // 시작 Board 생성 (이거 안하면 시작하고 New를 누르지 않은 상태로 Fit on Screen버튼 누르면 앱이 튕김.
            m_policy = new Policy(_boardUpdatePolicy, _forbiddenMovePolicy, _playerCalculationPolicy);
            m_board = new Board(19, 19, _panel, m_policy);
            DrawCurrentBoard();
            m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });

            // DeselectToolRect 인스턴스 생성
            m_deselectToolRect = new Rectangle()
            {
                Fill = ColorTable[(int)ColorEnum.Transparent]
            };
            m_deselectToolRect.MouseDown += OnDeselectTool;
            System.Windows.Controls.Grid.SetRow(m_deselectToolRect, 1);
        }
        private void OnClickForward10Button(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(10);
            m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });
        }
        private void OnClickLastMoveButton(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(20050906);
            m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });
        }
        private void OnClickForwardButton(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(1);
            m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });
        }
        private void OnClickNewButton(object sender, RoutedEventArgs e)
        {
            ClearCanvas();

            bool _isWidthInputSolid = int.TryParse(WidthText.Text, out int _width);
            bool _isHeightInputSolid = int.TryParse(HeightText.Text, out int _height);
            bool _isMoveInputSolid = int.TryParse(MoveText.Text, out int _moveNum);
            bool _isPlayerInputSolid = int.TryParse(PlayerText.Text, out int _playerNum);

            if (!_isWidthInputSolid)
            {
                WidthText.Background = ColorTable[(int)ColorEnum.Red];
            }
            if (!_isHeightInputSolid)
            {
                HeightText.Background = ColorTable[(int)ColorEnum.Red];
            }
            if (!_isMoveInputSolid)
            {
                MoveText.Background = ColorTable[(int)ColorEnum.Red];
            }
            if (!_isPlayerInputSolid)
            {
                PlayerText.Background = ColorTable[(int)ColorEnum.Red];
            }

            _isHeightInputSolid &= _height > 1;
            _isWidthInputSolid &= _width > 1;
            _isMoveInputSolid &= _moveNum > 0;
            _isPlayerInputSolid &= _playerNum > 0 && _playerNum <= (int)PlayerEnum.Max;

            if (!_isWidthInputSolid)
            {
                WidthText.Background = ColorTable[(int)ColorEnum.Red];
            }
            if (!_isHeightInputSolid)
            {
                HeightText.Background = ColorTable[(int)ColorEnum.Red];
            }
            if (!_isMoveInputSolid)
            {
                MoveText.Background = ColorTable[(int)ColorEnum.Red];
            }
            if (!_isPlayerInputSolid)
            {
                PlayerText.Background = ColorTable[(int)ColorEnum.Red];
            }

            if (_isWidthInputSolid && _isHeightInputSolid && _isMoveInputSolid && _isPlayerInputSolid)
            {
                WidthText.Background = ColorTable[(int)ColorEnum.White];
                HeightText.Background = ColorTable[(int)ColorEnum.White];

                BoardUpdatePolicyEnum _boardUpdatePolicyEnum = BoardUpdatePolicyEnum.None;
                switch ((Mode)ModeComboBox.SelectedIndex)
                {
                    case Mode.Analyze:
                        _boardUpdatePolicyEnum = BoardUpdatePolicyEnum.None;
                        break;
                    case Mode.Normal:
                        _boardUpdatePolicyEnum = BoardUpdatePolicyEnum.Plus;
                        break;
                    case Mode.Diagonal:
                        _boardUpdatePolicyEnum = BoardUpdatePolicyEnum.Cross;
                        break;
                    default:
                        _boardUpdatePolicyEnum = BoardUpdatePolicyEnum.None;
                        break;
                }

                Panel _panel = DisplayGrid.Children
                    .Cast<Panel>()
                    .First(element => System.Windows.Controls.Grid.GetRow(element) == 0);
                PlayerCalculationPolicy _playerCalculationPolicy = new PlayerCalculationPolicy(_playerNum, _moveNum);
                ForbiddenMovePolicy _forbiddenMovePolicy = new ForbiddenMovePolicy(_width, _height, _panel, _boardUpdatePolicyEnum, ForbiddenMovePolicyEnum.Outside, ForbiddenMovePolicyEnum.Overlay, ForbiddenMovePolicyEnum.Ko, ForbiddenMovePolicyEnum.Suicide);
                BoardUpdatePolicy _boardUpdatePolicy = new BoardUpdatePolicy(_width, _height, _panel, _boardUpdatePolicyEnum);

                m_policy = new Policy(_boardUpdatePolicy, _forbiddenMovePolicy, _playerCalculationPolicy);
                m_board = new Board(_width, _height, _panel, m_policy);
                DrawCurrentBoard();
                m_board.UpdateLabels(new TextBlock[] { Player1Counter, Player2Counter }, new TextBlock[] { FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel });
            }

        }
        private void OnClickFitButton(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            m_board = new Board(m_board);
            m_board.DrawBoard();
        } 
        private void OnCheckActivate(object sender, RoutedEventArgs e)
        {

        }
        private void OnUncheckActivate(object sender, RoutedEventArgs e) { }
        private void OnCheckOverlay(object sender, RoutedEventArgs e) { }
        private void OnUncheckOverlay(object sender, RoutedEventArgs e) { }

        /********************************************************************************/
        /*********************<PRIVATE METHODS FOR TOOL BUTTONS>*************************/
        /********************************************************************************/

        private void OnClickFileButton(object sender, RoutedEventArgs e)
        {
            if (BackGrid.Children.Contains(m_deselectToolRect))
            {
                OnDeselectTool(null, null);
            }
            else
            {
                BackGrid.Children.Add(m_deselectToolRect);
                m_currentOpenDropdown = new FloatingDropdown(BackGrid, new Point(0, 25), new string[] { "Save", "Save As", "Load" }, new RoutedEventHandler[]
                {
                    OnClickSaveButton,
                    OnClickSaveAsButton,
                    OnClickLoadButton
                });
            }
        }
        private void OnClickOptionButton(object sender, RoutedEventArgs e)
        {
            if (BackGrid.Children.Contains(m_deselectToolRect))
            {
                OnDeselectTool(null, null);
            }
            else
            {
                BackGrid.Children.Add(m_deselectToolRect);
            }
        }
        private void OnClickHelpButton(object sender, RoutedEventArgs e)
        {/*
            if (BackGrid.Children.Contains(m_deselectToolRect))
            {
                OnDeselectTool(null, null);
            }
            else
            {
                BackGrid.Children.Add(m_deselectToolRect);
            }*/
        }
        private void OnDeselectTool(object _sender, MouseButtonEventArgs _e)
        {
            BackGrid.Children.Remove(m_deselectToolRect);
            if (m_currentOpenDropdown != null)
                m_currentOpenDropdown.Destroy();
        }

        #endregion

        #region File I/O Methods

        private void OnClickSaveButton(object sender, RoutedEventArgs e)
        {
            OnDeselectTool(null, null);

        }
        private void OnClickSaveAsButton(object sender, RoutedEventArgs e)
        {
            OnDeselectTool(null, null);
        }
        private void OnClickLoadButton(object sender, RoutedEventArgs e)
        {
            OnDeselectTool(null, null);
            var _fileSelectionWindow = new FileSelectionWindow();
            _fileSelectionWindow.Show();
        }

        #endregion

        private void DrawCurrentBoard()
        {
            DrawBoard(m_board);
        }
        private void DrawBoard(Board _board)
        {
            _board.DrawBoard();
        }

        // Please do NOT touch these methods. Pleeeeeeeeeasssssssss
        private void ClearCanvas()
        {
            MainCanvas.Children.Clear();
        }
        private void Log(string _msg)
        {
            LogLabel.Text = _msg;
        }
    }
    public enum Mode
    {
        None = -1,
        Analyze,
        Normal,
        Moku,
        Scatter,
        Diagonal,
        Max
    }
}
