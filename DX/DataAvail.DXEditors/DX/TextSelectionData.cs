using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors.DX
{
    public class TextSelectionData
    {
        public TextSelectionData(System.Drawing.Color Color)
        {
            color = Color;
        }

        public TextSelectionData(string Filter, System.Drawing.Color Color)
        {
            filter = Filter;

            color = Color;
        }

        public TextSelectionData(int StartIndex, int Length, System.Drawing.Color Color)
        {
            _markers = new Marker[] { new Marker() { Start = StartIndex, End = StartIndex + Length} };

            color = Color;
        }

        private Marker[] _markers;

        private readonly string filter;

        internal readonly System.Drawing.Color color;

        public Marker[] Markers
        {
            get { return _markers; }

            set { _markers = value; }
        }

        public void Calculate(string String)
        {
            Calculate(String, filter);
        }

        public void Calculate(string String, string FilterString)
        {
            if (!string.IsNullOrEmpty(String) && !string.IsNullOrEmpty(FilterString))
            {
                int startIndex = String.ToUpper().IndexOf(FilterString.ToUpper());

                int length = FilterString.Length;

                _markers = new Marker[] { new Marker() { Start = startIndex, End = startIndex + length } };
            }
            else
            {
                Markers = null;
            }
        }

        public struct Marker
        {
            public int Start;

            public int End;

            public int Length { get { return End - Start; } }
        }
    }
}
