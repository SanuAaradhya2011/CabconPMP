using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CabconPMPREJECTIONTOOL
{
    public class MyCheckedListBox : CheckedListBox
    {
        private int _itemHeight = 22;

        public MyCheckedListBox(int itemHeight)
        {
            _itemHeight = itemHeight;
        }

        public override int ItemHeight
        {
            get { return _itemHeight; }
        }

        public MyCheckedListBox()
        {
        }
    }
}
