using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    [Serializable]
    public class PlayerCalculationPolicy
    {
        private readonly int[] m_playerBuffer;
        private readonly Player[] m_players;
        public PlayerCalculationPolicy(int _playerNum, int _moveNum)
        {
            m_playerBuffer = new int[_playerNum * _moveNum];
            m_players = new Player[_playerNum];
            for (int i = 0; i  < _playerNum; i++)
            {
                m_players[i] = new Player((PlayerEnum)i);
                for (int j = 0; j < _moveNum; j++)
                {
                    m_playerBuffer[i * _moveNum + j] = i;
                }
            }
        }
        public Player GetPlayer(int _index)
        {
            return m_players[m_playerBuffer[_index % m_playerBuffer.Length]];
        }
    }
}
