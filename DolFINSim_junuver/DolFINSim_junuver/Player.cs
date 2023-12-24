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
        public void SetName(string _name)
        {
            m_name = _name;
        }
        public void AddCount(int _count)
        {
            m_caughtStonesCount += _count;
        }
    }
    public enum PlayerEnum
    {

        None = -1, // 바둑판의 빈 공간
        Player1,
        Player2,
        Player3,
        Player4,
        Player5,
        Player6,
        Player7,
        Player8,
        Max,
        // Bot
        BotGeneral = 100,
        Bot1,
        Bot2
    }
}
