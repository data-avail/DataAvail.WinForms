using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoohlShmoohl
{
    public class AddressSuggestion
    {
        public AddressSuggestion()
        {
        }

        private string _suggestion;

        private Marker[] _markers = new Marker[] { };

        public string Suggestion
        {
            get { return _suggestion; }
            set { _suggestion = value; }
        }

        public Marker[] Markers
        {
            get { return _markers; }
            set { _markers = value; }
        }

        public struct Marker
        {
            public Marker(int Start, int End)
            {
                this.Start = Start;

                this.End = End;
            }

            public int Start;

            public int End;
        }
    }
}
