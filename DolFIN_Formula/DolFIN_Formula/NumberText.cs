using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DolFIN_Formula
{
    public class NumberText : IFormattable
    {
        private readonly string m_text;

        public override string ToString()
        {
            return ToString("", CultureInfo.CurrentCulture);
        }

        public string ToString(string _format, IFormatProvider _provider)
        {
            return m_text[0] == '.' ? $"0{m_text}" : m_text;
        }

        #region Operator Overloadings
        public static NumberText operator +(NumberText _left, NumberText _right)
        {
            string _leftText = _left.ToString();
            string _rightText = _right.ToString();
            bool _isLeftNegative = _leftText.StartsWith("-");
            bool _isRightNegative = _rightText.StartsWith("-");
            if (_isLeftNegative) 
                _leftText = _leftText.Substring(1);
            if (_isRightNegative)
                _rightText = _rightText.Substring(1);

            string[] _leftSplitedArray = _leftText.Split('.');
            string[] _rightSplitedArray = _rightText.Split('.');
            string _leftScaled = "";
            string _rightScaled = "";
            int _leftScaleIndex = 0;
            int _rightScaleIndex = 0;

            if (_leftSplitedArray.Length == 1)
                _leftScaleIndex = 0;
            else if (_leftSplitedArray.Length == 2)
                _leftScaleIndex = _leftSplitedArray[1].Length * -1;

            if (_rightSplitedArray.Length == 1)
                _rightScaleIndex = 0;
            else if (_rightSplitedArray.Length == 2)
                _rightScaleIndex = _rightSplitedArray[1].Length * -1;

            int _scaleIndex = Math.Min(_leftScaleIndex,  _rightScaleIndex);
            if (_leftSplitedArray.Length == 1)
                _leftScaled = $"{_leftSplitedArray[0]}{new string('0', Math.Abs(_scaleIndex))}";
            else if (_leftSplitedArray.Length == 2)
                _leftScaled = $"{_leftSplitedArray[0]}{_leftSplitedArray[1]}{new string('0', Math.Abs(_scaleIndex) - _leftSplitedArray[1].Length)}";

            if (_rightSplitedArray.Length == 1)
                _rightScaled = $"{_rightSplitedArray[0]}{new string('0', Math.Abs(_scaleIndex))}";
            else if (_rightSplitedArray.Length == 2)
                _rightScaled = $"{_rightSplitedArray[0]}{_rightSplitedArray[1]}{new string('0', Math.Abs(_scaleIndex) - _rightSplitedArray[1].Length)}";

            int _maxDigit = Math.Max(_leftScaled.Length, _rightScaled.Length);
            _leftScaled = $"{new string('0', _maxDigit - _leftScaled.Length)}{_leftScaled}";
            _rightScaled = $"{new string('0', _maxDigit - _rightScaled.Length)}{_rightScaled}";

            var _stringBuilder = new StringBuilder();
            int _carry = 0;
            if (_isLeftNegative && _isRightNegative || !_isLeftNegative && !_isRightNegative)
            {
                for (int i = _maxDigit - 1; i >= 0; i--)
                {
                    int _sum = _carry + (_leftScaled[i] - '0') + (_rightScaled[i] - '0');
                    _carry = _sum / 10;
                    _stringBuilder.Insert(0, (_sum % 10).ToString());
                }
                if (_carry != 0)
                {
                    _stringBuilder.Insert(0, _carry);
                }
            }
            else
            {
                int _whichOneFarther = 0;
                for (int i = 0; i < _maxDigit; i++)
                {
                    if (_leftScaled[i] > _rightScaled[i])
                    {
                        _whichOneFarther = -1;
                        break;
                    }
                    else if (_leftScaled[i] < _rightScaled[i])
                    {
                        _whichOneFarther = 1;
                        break;
                    }
                }
                if (_whichOneFarther == 0)
                    return new NumberText("0");

                for (int i = 0; i < _maxDigit; i++)
                {
                    int _sum = _carry * 10 + (_leftScaled[i] - '0') * (_isLeftNegative ? -1 : 1) + (_rightScaled[i] - '0') * (_isRightNegative ? -1 : 1);
                    _carry = _sum < 0 ? -1 : 0;
                    if (_sum >= 10)
                        _stringBuilder[_stringBuilder.Length - 1] = (char)(_stringBuilder[_stringBuilder.Length - 1]  - '0' + 1);
                    _stringBuilder.Append((_sum % 10) - 1);
                }
            }

            if (_scaleIndex != 0)
            {
                _stringBuilder.Insert(_stringBuilder.Length + _scaleIndex, '.');
            }

            return new NumberText( _stringBuilder.ToString() );
        } 
        public static NumberText operator *(NumberText _left, NumberText _right)
        {
            string[] _leftSplitedArray = _left.m_text.Split('.');
            string[] _rightSplitedArray = _right.m_text.Split('.');
            string _leftScaled = "";
            string _rightScaled = "";
            int _leftScaleIndex = 0;
            int _rightScaleIndex = 0;

            if (_leftSplitedArray.Length == 1)
            {
                _leftScaled = _leftSplitedArray[0].TrimEnd('0');
                _leftScaleIndex = _leftSplitedArray[0].Length - _leftScaled.Length;
            }
            else if (_leftSplitedArray.Length == 2)
            {
                _leftScaled = $"{_leftSplitedArray[0]}{_leftSplitedArray[1]}";
                _leftScaleIndex = _leftSplitedArray[1].Length * -1;
            }

            if (_rightSplitedArray.Length == 1)
            {
                _rightScaled = _rightSplitedArray[0].TrimEnd('0');
                _rightScaleIndex = _rightSplitedArray[0].Length - _rightScaled.Length;
            }
            else if (_rightSplitedArray.Length == 2)
            {
                _rightScaled = $"{_rightSplitedArray[0]}{_rightSplitedArray[1]}";
                _rightScaleIndex = _rightSplitedArray[1].Length * -1;
            }



            return null;
        }

        #endregion

        public NumberText(string _text)
        {
            bool _isNegative = _text.StartsWith("-");
            if (_isNegative)
                _text = _text.Substring(1);

            if (_text.Equals("") || Regex.IsMatch(_text, @"[^\d.-]+"))
                throw new ArgumentException("Invalid Input!");

            int _pointCount = _text.Count(c => c == '.');
            if (_pointCount == 0)
                _text = _text.TrimStart('0');
            else if (_pointCount == 1)
                _text = _text.Trim('0');
            else
                throw new ArgumentException("Invalid Input!");

            m_text = _text[_text.Length - 1] == '.' ? _text.Substring(0, _text.Length - 1) : _text;
            if (_isNegative)
                m_text = $"-{m_text}";
        }
    }
}
