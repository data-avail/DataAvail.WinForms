using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.DXEditors
{
    public class GoogleLikeComboData
    {
        private object _key;

        private string _text;

        private string _dropDownText;

        private GoogleLikeComboMarker[] _markers;

        public object Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public string DropDownText
        {
            get { return _dropDownText; }
            set { _dropDownText = value; }
        }

        public override string ToString()
        {
            return DropDownText;
        }

        public GoogleLikeComboMarker[] Markers
        {
            get { return _markers; }
            set { _markers = value; }
        }

        public bool IsMarked
        {
            get
            {
                return Markers != null && Markers.Length != 0;
            }
        }
    }

    public struct GoogleLikeComboMarker
    {
        public GoogleLikeComboMarker(int Start, int End)
        {
            this.Start = Start;

            this.End = End;
        }

        public int Start;

        public int End;

        public bool IsEmpty { get { return Start == 0 && End == 0; } }

        public int Length { get { return End - Start; } }
    }


}
