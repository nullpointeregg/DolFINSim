using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DolFINSim_junuver
{
    public class ForbiddenMovePolicy
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
        private readonly IntegerVector2[] m_goDiffs;
        private readonly Func<PlayerEnum, IntegerVector2, Stone[], bool>[] m_illegalMoveFuncs;
        private readonly Func<PlayerEnum, IntegerVector2, Stone[], bool>[] m_forbiddenMoveFuncs;
        public ForbiddenMovePolicy(int _width, int _height, Panel _panel, BoardUpdatePolicyEnum _boardUpdatePolicy, params ForbiddenMovePolicyEnum[] _forbiddens)
        {
            m_width = _width;
            m_height = _height;
            m_panel = _panel;

            var _illegalMoveFuncsList = new List<Func<PlayerEnum, IntegerVector2, Stone[], bool>>();
            var _forbiddenMoveFuncList = new List<Func<PlayerEnum, IntegerVector2, Stone[], bool>>();
            for (int i = 0; i < _forbiddens.Length; i++)
            {
                switch (_forbiddens[i])
                {
                    case ForbiddenMovePolicyEnum.Outside:
                        _illegalMoveFuncsList.Add(IsOutside);
                        break;
                    case ForbiddenMovePolicyEnum.Overlay:
                        _illegalMoveFuncsList.Add(IsOverlay);
                        break;
                    case ForbiddenMovePolicyEnum.Ko:
                        _forbiddenMoveFuncList.Add(IsKo);
                        break;
                    case ForbiddenMovePolicyEnum.Suicide:
                        _forbiddenMoveFuncList.Add(IsSuicide);
                        break;
                    case ForbiddenMovePolicyEnum.Renzu:
                        _forbiddenMoveFuncList.Add(IsRenzu);
                        break;
                    default: break;
                }
            }

            if (_forbiddenMoveFuncList.Count == 0)
                _forbiddenMoveFuncList.Add(ReturnsFalse);

            switch (_boardUpdatePolicy)
            {
                case BoardUpdatePolicyEnum.None:
                    _forbiddenMoveFuncList.Clear();
                    _forbiddenMoveFuncList.Add(ReturnsFalse);
                    break;
                case BoardUpdatePolicyEnum.Plus:
                    m_goDiffs = s_goPlusDiffs;
                    break;
                case BoardUpdatePolicyEnum.Cross:
                    m_goDiffs = s_goCrossDiffs;
                    break;
                default: break;
            }

            m_illegalMoveFuncs = _illegalMoveFuncsList.ToArray();
            m_forbiddenMoveFuncs = _forbiddenMoveFuncList.ToArray();

        }

        public bool IsIllegal(PlayerEnum _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return m_illegalMoveFuncs.Any(f => f(_player, _position, _placedStones));
        }
        public bool IsForbidden(PlayerEnum _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return m_forbiddenMoveFuncs.Any(f => f(_player, _position, _placedStones));
        }
        private bool IsOutside(PlayerEnum _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return _position.X < 0 || _position.X >= m_width || _position.Y < 0 || _position.Y >= m_height;
        } 
        private bool IsOverlay(PlayerEnum _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return _placedStones.Any(s => s.IsOnDisplay(_position, m_panel));
        }
        public bool IsKo(PlayerEnum _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return false;
        }
        public bool IsSuicide(PlayerEnum _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            PlayerEnum[][] _playerMap = GetInitializedPlayerArray(_placedStones);
            _playerMap[_position.Y][_position.X] = _player;
            Status[][] _statusMap = GetInitializedplayerStatusArray(_player, _playerMap);
            _statusMap[_position.Y][_position.X] = Status.Dead;

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
                            for (int i = 0; i < m_goDiffs.Length; i++)
                            {
                                int _x = _centerX + m_goDiffs[i].X;
                                int _y = _centerY + m_goDiffs[i].Y;
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
                                for (int i = 0; i < m_goDiffs.Length; i++)
                                {
                                    int _x = _centerX + m_goDiffs[i].X;
                                    int _y = _centerY + m_goDiffs[i].Y;
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

            List<IntegerVector2> _suicideList = new List<IntegerVector2>();
            for (int y = 0; y < _statusMap.Length; y++)
            {
                for (int x = 0; x < _statusMap[y].Length; x++)
                {
                    if (_statusMap[y][x] == Status.Dead)
                        _suicideList.Add(new IntegerVector2(x, y));
                }
            }

            return _suicideList.Count != 0;
        }
        private bool IsRenzu(PlayerEnum _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return false;
        }
        private bool ReturnsFalse(PlayerEnum _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return false;
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
        private Status[][] GetInitializedplayerStatusArray(PlayerEnum _player, PlayerEnum[][] _playerMap)
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
                    else if (_cell == _player)
                        _statusMap[y][x] = Status.Unknown;
                    else
                        _statusMap[y][x] = Status.Enemy;
                }
            }

            return _statusMap;
        }
    }

    public enum ForbiddenMovePolicyEnum
    {
        None = -1,
        Outside,
        Overlay,
        Ko,
        Suicide,
        Renzu,
        Max
    }

    enum Status
    {
        Unknown = 0, // 기본값
        Alive,
        Dead,
        Enemy,
        Unoccupied
    }
}
