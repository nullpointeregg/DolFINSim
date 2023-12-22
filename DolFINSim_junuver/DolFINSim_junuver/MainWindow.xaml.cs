﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
            new SolidColorBrush(Colors.Yellow)
        };
        private Board m_board;
        private Policy m_policy;

        public MainWindow()
        {
            InitializeComponent();
            MainCanvas.MouseDown += CanvasOnMouseDown;
        }

        #region Interactive Methods
        private void CanvasOnMouseDown(object sender, MouseEventArgs e)
        {
            m_board.PlaceNew(e.GetPosition(this));
            m_board.UpdateLabels(FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel);
        }
        private void OnClickFirstMoveButton(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(-20050906);
            m_board.UpdateLabels(FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel);
        }
        private void OnClickBackward10Button(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(-10);
            m_board.UpdateLabels(FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel);
        }
        private void OnClickBackwardButton(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(-1);
            m_board.UpdateLabels(FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel);
        }
        private void OnClickForward10Button(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(10);
            m_board.UpdateLabels(FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel);
        }
        private void OnClickLastMoveButton(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(20050906);
            m_board.UpdateLabels(FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel);
        }
        private void OnClickForwardButton(object sender, RoutedEventArgs e)
        {
            m_board.ShowFromCurrentIndex(1);
            m_board.UpdateLabels(FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel);
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
            _isPlayerInputSolid &= _playerNum > 0 && _playerNum <= (int)Player.Max;

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

                Panel _panel = DisplayGrid.Children
                    .Cast<Panel>()
                    .First(element => System.Windows.Controls.Grid.GetRow(element) == 0);
                PlayerCalculationPolicy _playerCalculationPolicy = new PlayerCalculationPolicy(_playerNum, _moveNum);
                ForbiddenMovePolicy _forbiddenMovePolicy = new ForbiddenMovePolicy(_width, _height, _panel, BoardUpdatePolicyEnum.Plus, ForbiddenMovePolicyEnum.Outside, ForbiddenMovePolicyEnum.Overlay, ForbiddenMovePolicyEnum.Ko, ForbiddenMovePolicyEnum.Suicide);
                BoardUpdatePolicy _boardUpdatePolicy = new BoardUpdatePolicy(_width, _height, _panel, BoardUpdatePolicyEnum.Plus);

                m_board = new Board(_width, _height, _panel, new Policy(_boardUpdatePolicy, _forbiddenMovePolicy, _playerCalculationPolicy));
                DrawCurrentBoard();
                m_board.UpdateLabels(FirstPlayerLabel, SecondPlayerLabel, ThirdPlayerLabel);
            }
        }
        private void OnClickFitButton(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            m_board = new Board(m_board);
            m_board.DrawBoard();
        }
        private void OnCheckActivate(object sender, RoutedEventArgs e) { }
        private void OnUncheckActivate(object sender, RoutedEventArgs e) { }
        private void OnSelectMode(object sender, RoutedEventArgs e) { }
        private void OnCheckOverlay(object sender, RoutedEventArgs e) { }
        private void OnUncheckOverlay(object sender, RoutedEventArgs e) { }
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
}
