using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    public class PlayerCalculationPolicy
    {
        private readonly PlayerEnum[] m_playerBuffer;
        private readonly Player[] m_players;
        public PlayerCalculationPolicy(int _playerNum, int _moveNum)
        {
            m_playerBuffer = new PlayerEnum[_playerNum * _moveNum];
            for (int i = 0; i  < _playerNum; i++)
            {
                for (int j = 0; j < _moveNum; j++)
                {
                    m_playerBuffer[i * _moveNum + j] = (PlayerEnum)i;
                }
            }
        }
        public PlayerEnum GetPlayer(int _index)
        {
            return m_playerBuffer[_index % m_playerBuffer.Length];
        }
    }
}
