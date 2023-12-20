using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    public class PlayerCalculationPolicy
    {
        private readonly Player[] _playerBuffer;
        public PlayerCalculationPolicy(int _playerNum, int _moveNum)
        {
            _playerBuffer = new Player[_playerNum * _moveNum];
            for (int i = 0; i  < _playerNum; i++)
            {
                for (int j = 0; j < _moveNum; j++)
                {
                    _playerBuffer[i * _moveNum + j] = (Player)i;
                }
            }
        }
        public Player GetPlayer(int _index)
        {
            return _playerBuffer[_index % _playerBuffer.Length];
        }
    }
}
