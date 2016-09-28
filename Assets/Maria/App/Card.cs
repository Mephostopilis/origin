using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Maria.App
{
    public class Card
    {
        private int _value;
        private int _type;
        private int _num;
        private int _idx;
        private GameObject _go = null;

        public Card(int v, GameObject go)
        {
            _go = go;
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

        public void SetPosition()
        {
        }
    }
}
