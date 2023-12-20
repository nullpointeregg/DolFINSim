using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DolFINSim_junuver
{
    public abstract class Grid
    {
        protected readonly int Width;
        protected readonly int Height;

        protected Grid(int _width, int _height)
        {
            Width = _width;
            Height = _height;
        }
    }
}
