using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DolFINSim_junuver
{
    public class Player
    {
        private static readonly SolidColorBrush[] s_colorTable = new SolidColorBrush[]
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

        private readonly PlayerEnum m_playerStatus;
        private string m_name;
        private int m_caughtStonesCount;

        public PlayerEnum GetPlayer()
        {
            return m_playerStatus;
        }
        public string GetName()
        {
            return m_name;
        }
        public int GetCaughtStonesCount()
        {
            return m_caughtStonesCount;
        }

        public void SetName(string _name)
        {
            m_name = _name;
        }
        public void AddCount(int _count)
        {
            m_caughtStonesCount += _count;
        }
        public Player(PlayerEnum _playerStatus) : this(_playerStatus, "")
        {

        }
        public Player(PlayerEnum _playerStatus, string _name)
        {
            m_playerStatus = _playerStatus;
            m_name = _name;
            m_caughtStonesCount = 0;
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
