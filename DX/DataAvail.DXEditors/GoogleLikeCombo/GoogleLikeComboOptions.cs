using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataAvail.DXEditors
{
    public class GoogleLikeComboOptions
    {
        private int _maxItemsCount = 10;

        Color _selectedTextColorTextEdit = Color.FromArgb(50, Color.Blue);

        Color _selectedTextColorListControl = Color.FromArgb(50, Color.Blue);

        Color _selectedItemListBarColor = Color.FromArgb(50, Color.Azure);

        public int MaxItemsCount
        {
            get { return _maxItemsCount; }
            set { _maxItemsCount = value; }
        }

        public Color SelectedTextColorTextEdit
        {
            get { return _selectedTextColorTextEdit; }
            set { _selectedTextColorTextEdit = value; }
        }

        public Color SelectedTextColorListControl
        {
            get { return _selectedTextColorListControl; }
            set { _selectedTextColorListControl = value; }
        }

        public Color SelectedItemListBarColor
        {
            get { return _selectedItemListBarColor; }
            set { _selectedItemListBarColor = value; }
        }
    }
}
