﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    public class ForbiddenMovePolicy
    {
        private readonly IntegerVector2[] s_goPlusDiffs = new IntegerVector2[]
        {
            new IntegerVector2(0, 1),   // Up
            new IntegerVector2(1, 0),   // Right
            new IntegerVector2(0, -1),  // Down
            new IntegerVector2(-1, 0)   // Left
        };
        private readonly IntegerVector2[] s_goCrossDiffs = new IntegerVector2[]
        {
            new IntegerVector2(1, 1),   // top right
            new IntegerVector2(1, -1),  // bottom right
            new IntegerVector2(-1, -1), // botton left
            new IntegerVector2(-1, 1)   // bottom right
        };

        private int m_width;
        private int m_height;
        Func<Player, IntegerVector2, Stone[], bool>[] m_forbiddenMoveFuncs;
        public ForbiddenMovePolicy(int _width, int _height, params ForbiddenMovePolicyEnum[] _forbiddens)
        {
            m_width = _width;
            m_height = _height;

            var _forbiddenMoveFuncList = new List<Func<Player, IntegerVector2, Stone[], bool>>();
            for (int i = 0; i < _forbiddens.Length; i++)
            {
                switch (_forbiddens[i])
                {
                    case ForbiddenMovePolicyEnum.Outside:
                        _forbiddenMoveFuncList.Add(IsOutside);
                        break;
                    case ForbiddenMovePolicyEnum.Overlay:
                        _forbiddenMoveFuncList.Add(IsOverlay);
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
                _forbiddenMoveFuncList.Add(ReturnsTrue);

            m_forbiddenMoveFuncs = _forbiddenMoveFuncList.ToArray();
        }

        public bool IsForbidden(Player _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return m_forbiddenMoveFuncs.Any(f => f(_player, _position, _placedStones));
        }
        private bool IsOutside(Player _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return _position.X >= 0 && _position.X < m_width && _position.Y >= 0 && _position.Y < m_height;
        }
        private bool IsOverlay(Player _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return _position.X >= 0 && _position.X < m_width && _position.Y >= 0 && _position.Y < m_height;
        }
        private bool IsKo(Player _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return _position.X >= 0 && _position.X < m_width && _position.Y >= 0 && _position.Y < m_height;
        }
        private bool IsSuicide(Player _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return _position.X >= 0 && _position.X < m_width && _position.Y >= 0 && _position.Y < m_height;
        }
        private bool IsRenzu(Player _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return _position.X >= 0 && _position.X < m_width && _position.Y >= 0 && _position.Y < m_height;
        }
        private bool ReturnsTrue(Player _player, IntegerVector2 _position, Stone[] _placedStones)
        {
            return true;
        }
        private IntegerVector2[] FindDead(Player _enemy, IntegerVector2 _position, Stone[] _placedStones)
        {
            Player[][] _playerMap = GetInitializedPlayerArray(_placedStones);
            _playerMap[_position.Y][_position.X] = _enemy;

            for (int i = 0; i < 4; i++)
            {
                int _x = _position.X + s_goDiffs[i].Item1;
                int _y = _position.Y + s_goDiffs[i].Item2;
                if (_x >= 0 && _x < _width && _y >= 0 && _y < _height)
                {
                    switch (s_statusTable[_y][_x])
                    {
                        case Status.Unknown:
                            s_statusTable[_y][_x] = Status.Dead;
                            break;
                        default: break;
                    }
                }
            }
            s_statusTable[ _position.Y][_position.X] = Status.Enemy;

            bool _changed = false;
            do
            {
                _changed = false;
                for (int _centerY = 0; _centerY < s_statusTable.Length; _centerY++)
                {
                    for (int _centerX = 0; _centerX < s_statusTable[_centerY].Length; _centerX++)
                    {
                        if (s_statusTable[_centerY][_centerX] == Status.Dead)
                        {
                            // Dead가 Alive로 바뀔 수 있는지 검사
                            for (int i = 0; i < 4; i++)
                            {
                                int _x = _centerX + s_goDiffs[i].Item1;
                                int _y = _centerY + s_goDiffs[i].Item2;
                                if (_x >= 0 && _x < _width && _y >= 0 && _y < _height)
                                {
                                    if (s_statusTable[_y][_x] == Status.Alive || s_statusTable[_y][_x] == Status.Unoccupied)
                                    {
                                        s_statusTable[_centerY][_centerX] = Status.Alive;
                                        _changed = true;
                                        break;
                                    }
                                }
                            }
                            // 바뀌지 않았다면 주변 Unknown을 CouldBeDead로 바꾼다.
                            if (s_statusTable[_centerY][_centerX] != Status.Alive)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    int _x = _centerX + s_goDiffs[i].Item1;
                                    int _y = _centerY + s_goDiffs[i].Item2;
                                    if (_x >= 0 && _x < _width && _y >= 0 && _y < _height)
                                    {
                                        if (s_statusTable[_y][_x] == Status.Unknown)
                                        {
                                            s_statusTable[_y][_x] = Status.Dead;
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


            for (int y = 0; y < s_statusTable.Length; y++)
            {
                for (int x = 0; x < s_statusTable[y].Length; x++)
                {
                    if (s_statusTable[y][x] == Status.Dead)
                        s_deadList.Add((x, y));
                }
            }

            return s_deadList.ToArray();
        }
        private Player[][] GetInitializedPlayerArray(Stone[] _placedStones)
        {
            var _map = new Player[m_height][];
            for (int y = 0; y < m_height; y++)
            {
                _map[y] = new Player[m_width];
                for (int x = 0; x < m_width; x++)
                {
                    _map[y][x] = Player.None;
                }
            }
            Array.ForEach(_placedStones, s => s.PlaceStone(_map));
            return _map;
        }
        private Status[][] GetInitializedStatusArray(Player _enemy, Player[][] _playerMap)
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
                    Player _player = _playerMap[y][x];
                    if (_player == Player.None)
                        _statusMap[y][x] = Status.Unoccupied;
                    else if (_player == _enemy)
                        _statusMap[y][x] = Status.Enemy;
                    else
                        _statusMap[y][x] = Status.Unknown;
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