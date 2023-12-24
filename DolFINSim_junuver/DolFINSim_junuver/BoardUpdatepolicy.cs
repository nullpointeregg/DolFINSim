using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DolFINSim_junuver
{
    [Serializable]
    public class BoardUpdatePolicy
    {
        private static readonly IntegerVector2[] s_goPlusDiffs = new IntegerVector2[]
        {
            new IntegerVector2(0, 1),   // Up
            new IntegerVector2(1, 0),   // Right
            new IntegerVector2(0, -1),  // Down
            new IntegerVector2(-1, 0)   // Left
        };
        private static readonly IntegerVector2[] s_goCrossDiffs = new IntegerVector2[]
        {
            new IntegerVector2(1, 1),   // top right
            new IntegerVector2(1, -1),  // bottom right
            new IntegerVector2(-1, -1), // botton left
            new IntegerVector2(-1, 1)   // bottom right
        };
        
        private readonly int m_width;
        private readonly int m_height;
        private readonly Panel m_panel;
        private readonly Func<PlayerEnum, IntegerVector2, Stone[], IntegerVector2[]> m_boardUpdateFunc;

        public BoardUpdatePolicy(int _width, int _height, Panel _panel, BoardUpdatePolicyEnum _boardUpdatePolicy)
        {
            m_width = _width;
            m_height = _height;
            m_panel = _panel;

            switch (_boardUpdatePolicy)
            {
                case BoardUpdatePolicyEnum.None:
                    m_boardUpdateFunc = ReturnEmpty;
                    break;
                case BoardUpdatePolicyEnum.Plus:
                    m_boardUpdateFunc = FindPlusDead;
                    break;
                case BoardUpdatePolicyEnum.Cross:
                    m_boardUpdateFunc = FindCrossDead;
                    break;
                default: break;
            }
        }
        public Stone[] FindDead(PlayerEnum _enemy, IntegerVector2 _position, Stone[] _placedStones)
        {
            IntegerVector2[] _deadStonePositions =  m_boardUpdateFunc(_enemy, _position, _placedStones);
            List<Stone> _deadStones = new List<Stone>();
            for (int i = 0; i < _placedStones.Length; i++)
            {
                for (int j = 0; j < _deadStonePositions.Length; j++)
                {
                    if (_placedStones[i].IsOnDisplay(_deadStonePositions[j], m_panel))
                    {
                        _deadStones.Add(_placedStones[i]);
                        break;
                    }
                }
            }

            return _deadStones.ToArray();

        }
        private IntegerVector2[] FindPlusDead(PlayerEnum _enemy, IntegerVector2 _position, Stone[] _placedStones)
        {
            PlayerEnum[][] _playerMap = GetInitializedPlayerArray(_placedStones);
            _playerMap[_position.Y][_position.X] = _enemy;
            Status[][] _statusMap = GetInitializedOpponentStatusArray(_enemy, _playerMap);

            for (int i = 0; i < s_goPlusDiffs.Length; i++)
            {
                int _x = _position.X + s_goPlusDiffs[i].X;
                int _y = _position.Y + s_goPlusDiffs[i].Y;
                if (_x >= 0 && _x < m_width && _y >= 0 && _y < m_height)
                {
                    switch (_statusMap[_y][_x])
                    {
                        case Status.Unknown:
                            _statusMap[_y][_x] = Status.Dead;
                            break;
                        default: break;
                    }
                }
            }
            _statusMap[_position.Y][_position.X] = Status.Enemy;

            bool _changed = false;
            do
            {
                _changed = false;
                for (int _centerY = 0; _centerY < _statusMap.Length; _centerY++)
                {
                    for (int _centerX = 0; _centerX < _statusMap[_centerY].Length; _centerX++)
                    {
                        if (_statusMap[_centerY][_centerX] == Status.Dead)
                        {
                            // Dead가 Alive로 바뀔 수 있는지 검사
                            for (int i = 0; i < 4; i++)
                            {
                                int _x = _centerX + s_goPlusDiffs[i].X;
                                int _y = _centerY + s_goPlusDiffs[i].Y;
                                if (_x >= 0 && _x < m_width && _y >= 0 && _y < m_height)
                                {
                                    if (_statusMap[_y][_x] == Status.Alive || _statusMap[_y][_x] == Status.Unoccupied)
                                    {
                                        _statusMap[_centerY][_centerX] = Status.Alive;
                                        _changed = true;
                                        break;
                                    }
                                }
                            }
                            // 바뀌지 않았다면 주변 Unknown을 CouldBeDead로 바꾼다.
                            if (_statusMap[_centerY][_centerX] != Status.Alive)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    int _x = _centerX + s_goPlusDiffs[i].X;
                                    int _y = _centerY + s_goPlusDiffs[i].Y;
                                    if (_x >= 0 && _x < m_width && _y >= 0 && _y < m_height)
                                    {
                                        if (_statusMap[_y][_x] == Status.Unknown)
                                        {
                                            _statusMap[_y][_x] = Status.Dead;
                                            _changed = true;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            } while (_changed);

            List<IntegerVector2> _deadList = new List<IntegerVector2>();
            for (int y = 0; y < _statusMap.Length; y++)
            {
                for (int x = 0; x < _statusMap[y].Length; x++)
                {
                    if (_statusMap[y][x] == Status.Dead)
                        _deadList.Add(new IntegerVector2(x, y));
                }
            }

            return _deadList.ToArray();
        }
        private IntegerVector2[] FindCrossDead(PlayerEnum _enemy, IntegerVector2 _position, Stone[] _placedStones)
        {
            PlayerEnum[][] _playerMap = GetInitializedPlayerArray(_placedStones);
            _playerMap[_position.Y][_position.X] = _enemy;
            Status[][] _statusMap = GetInitializedOpponentStatusArray(_enemy, _playerMap);

            for (int i = 0; i < s_goCrossDiffs.Length; i++)
            {
                int _x = _position.X + s_goCrossDiffs[i].X;
                int _y = _position.Y + s_goCrossDiffs[i].Y;
                if (_x >= 0 && _x < m_width && _y >= 0 && _y < m_height)
                {
                    switch (_statusMap[_y][_x])
                    {
                        case Status.Unknown:
                            _statusMap[_y][_x] = Status.Dead;
                            break;
                        default: break;
                    }
                }
            }
            _statusMap[_position.Y][_position.X] = Status.Enemy;

            bool _changed = false;
            do
            {
                _changed = false;
                for (int _centerY = 0; _centerY < _statusMap.Length; _centerY++)
                {
                    for (int _centerX = 0; _centerX < _statusMap[_centerY].Length; _centerX++)
                    {
                        if (_statusMap[_centerY][_centerX] == Status.Dead)
                        {
                            // Dead가 Alive로 바뀔 수 있는지 검사
                            for (int i = 0; i < s_goCrossDiffs.Length; i++)
                            {
                                int _x = _centerX + s_goCrossDiffs[i].X;
                                int _y = _centerY + s_goCrossDiffs[i].Y;
                                if (_x >= 0 && _x < m_width && _y >= 0 && _y < m_height)
                                {
                                    if (_statusMap[_y][_x] == Status.Alive || _statusMap[_y][_x] == Status.Unoccupied)
                                    {
                                        _statusMap[_centerY][_centerX] = Status.Alive;
                                        _changed = true;
                                        break;
                                    }
                                }
                            }
                            // 바뀌지 않았다면 주변 Unknown을 CouldBeDead로 바꾼다.
                            if (_statusMap[_centerY][_centerX] != Status.Alive)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    int _x = _centerX + s_goCrossDiffs[i].X;
                                    int _y = _centerY + s_goCrossDiffs[i].Y;
                                    if (_x >= 0 && _x < m_width && _y >= 0 && _y < m_height)
                                    {
                                        if (_statusMap[_y][_x] == Status.Unknown)
                                        {
                                            _statusMap[_y][_x] = Status.Dead;
                                            _changed = true;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            } while (_changed);

            List<IntegerVector2> _deadList = new List<IntegerVector2>();
            for (int y = 0; y < _statusMap.Length; y++)
            {
                for (int x = 0; x < _statusMap[y].Length; x++)
                {
                    if (_statusMap[y][x] == Status.Dead)
                        _deadList.Add(new IntegerVector2(x, y));
                }
            }

            return _deadList.ToArray();
        }
        private IntegerVector2[] ReturnEmpty(PlayerEnum _enemy, IntegerVector2 _position, Stone[] _placedStones)
        {
            return new IntegerVector2[0];
        }
        private PlayerEnum[][] GetInitializedPlayerArray(Stone[] _placedStones)
        {
            var _map = new PlayerEnum[m_height][];
            for (int y = 0; y < m_height; y++)
            {
                _map[y] = new PlayerEnum[m_width];
                for (int x = 0; x < m_width; x++)
                {
                    _map[y][x] = PlayerEnum.None;
                }
            }
            Array.ForEach(_placedStones, s => s.PlaceStone(_map, m_panel));
            return _map;
        }
        private Status[][] GetInitializedOpponentStatusArray(PlayerEnum _enemy, PlayerEnum[][] _playerMap)
        {
            Status[][] _statusMap = new Status[m_height][];
            for (int i = 0; i < m_height; i++)
            {
                _statusMap[i] = new Status[m_width];
            }

            for (int y = 0; y < _statusMap.Length; y++)
            {
                for (int x = 0; x < _statusMap[y].Length; x++)
                {
                    PlayerEnum _cell = _playerMap[y][x];
                    if (_cell == PlayerEnum.None)
                        _statusMap[y][x] = Status.Unoccupied;
                    else if (_cell == _enemy)
                        _statusMap[y][x] = Status.Enemy;
                    else
                        _statusMap[y][x] = Status.Unknown;
                }
            }

            return _statusMap;
        }
    }

    public enum BoardUpdatePolicyEnum
    {
        None = -1,
        Plus,
        Cross,
        Max
    }
}
