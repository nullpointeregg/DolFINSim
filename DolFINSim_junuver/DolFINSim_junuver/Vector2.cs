using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DolFINSim_junuver
{
    public struct IntegerVector2
    {
        public int X;
        public int Y;
        public IntegerVector2(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static IntegerVector2 operator +(IntegerVector2 _v1, IntegerVector2 _v2)
        {
            return new IntegerVector2(_v1.X + _v2.X, _v1 .Y + _v2.Y);
        }
        public static bool operator ==(IntegerVector2 _v1, IntegerVector2 _v2)
        {
            return _v1.X == _v2.X && _v1.Y == _v2.Y;
        }
        public static bool operator !=(IntegerVector2 _v1, IntegerVector2 _v2)
        {
            return _v1.X != _v2.X || _v1.Y != _v2.Y;
        }
    }
}
