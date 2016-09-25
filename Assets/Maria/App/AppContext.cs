using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.App
{
    public class AppContext : Context
    {
        private List<Card> _cards = new List<Card>();
        private int _idx = 0;

        public AppContext()
            :base()
        {
            GameController gctl = new GameController(this);
            _hash["game"] = gctl;
            LoginController lctl = new LoginController(this);
            _hash["login"] = lctl;


        }

        public Card Next()
        {
            var card = _cards[_idx];
            return card;
        }
    }
}
