using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    public class Player
    {
        private readonly PlayerEnum m_playerStatus;
        private string m_name;
        private int m_caughtStonesCount;

        public Player(PlayerEnum _playerStatus, string _name)
        {
            m_playerStatus = _playerStatus;
            m_name = _name;
            m_caughtStonesCount = 0;
        }
        public void AddCount(int _count)
        {
            m_caughtStonesCount += _count;
        }
    }
}
