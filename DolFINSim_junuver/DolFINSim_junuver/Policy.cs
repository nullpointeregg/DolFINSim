using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    public struct Policy
    {
        public BoardUpdatePolicy BoardUpdatePolicy;
        public ForbiddenMovePolicy ForbiddenMovePolicy;
        public PlayerCalculationPolicy PlayerCalculationPolicy;

        public Policy(BoardUpdatePolicy _boardUpdatePolicy, ForbiddenMovePolicy _forbiddenMovePolicy, PlayerCalculationPolicy _playerCalculationPolicy)
        {
            BoardUpdatePolicy = _boardUpdatePolicy;
            ForbiddenMovePolicy = _forbiddenMovePolicy;
            PlayerCalculationPolicy = _playerCalculationPolicy;
        }
    }
}
