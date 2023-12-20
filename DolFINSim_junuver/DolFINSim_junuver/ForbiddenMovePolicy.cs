using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    public class ForbiddenMovePolicy
    {
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
        private bool ReturnsTrue(Player _player, IntegerVector2 position, Stone[] _placedStones)
        {
            return true;
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
}
