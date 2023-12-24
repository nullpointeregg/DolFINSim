using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DolFINSim_junuver
{
    public class FloatingDropdown
    {
        private readonly Panel m_panel;
        private readonly Rectangle m_rect;
        public FloatingDropdown(Panel _panel, string[] _titles, Action[] _clickActions)
        {
            m_panel = _panel;

        }
    }
}
