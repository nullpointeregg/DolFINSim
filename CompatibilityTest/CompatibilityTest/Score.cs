using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompatibilityTest
{
    public class Score
    {
        private readonly float m_value;
        public Score(int _value) : this((float)_value)
        {
        }
        public Score(float _value)
        {
            m_value = _value;
        }
        public override string ToString()
        {
            return m_value.ToString();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #region Operator Overloadings
        public static Score operator +(Score _left, Score _right)
        {
            return new Score(_left.m_value + _right.m_value);
        }
        public static Score operator -(Score _left, Score _right)
        {
            return new Score(_left.m_value - _right.m_value);
        }
        public static Score operator *(Score _left, Score _right)
        {
            return new Score(_left.m_value * _right.m_value);
        }
        public static Score operator /(Score _left, Score _right)
        {
            return new Score(_left.m_value / _right.m_value);
        }
        public static bool operator ==(Score _left, Score _right)
        {
            return _left.m_value == _right.m_value;
        }
        public static bool operator !=(Score _left, Score _right)
        {
            return _left.m_value != _right.m_value;
        }
        public static bool operator <(Score _left, Score _right)
        {
            return _left.m_value < _right.m_value;
        }
        public static bool operator >(Score _left, Score _right)
        {
            return _left.m_value > _right.m_value;
        }
        public static bool operator <=(Score _left, Score _right)
        {
            return _left.m_value <= _right.m_value;
        }
        public static bool operator >=(Score _left, Score _right)
        {
            return _left.m_value >= _right.m_value;
        }
        #endregion
    }
}
