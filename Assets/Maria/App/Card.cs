using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.App
{
    public class Card
    {
        private int _value;
        private int _type;
        private int _num;
        private int _idx;

        public Card(int v)
        {
            _value = v;
        }

        public void SetIdx(int idx)
        {
            _idx = idx;
        }

        public int GetIdx()
        {
            return _idx;
        }
    }
}
