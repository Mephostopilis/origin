using System.Collections.Generic;
using UnityEngine;

namespace Maria.App
{
    public class AppContext : Context
    {
        private List<Card> _cards = new List<Card>();
        private int _idx = 0;
        private Assets _assets = new Assets();
        private GameObject _cardsParent = null;

        public AppContext(global::App app)
            : base(app)
        {
            GameController gctl = new GameController(this);
            _hash["game"] = gctl;
            
            _cardsParent = new GameObject();
            _cardsParent.transform.SetParent(Assets.transform);
            for (int i = 0; i < 3; i++)
            {
                GameObject go = _assets.GetCard("Card");
                go.transform.SetParent(_cardsParent.transform);
                go.SetActive(false);

                Card c = new Card(i, go);
                _cards.Add(c);
            }

            Config = new AppConfig();
        }

        public void Put()
        {
        }

        public Card Next()
        {
            var card = _cards[_idx];
            return card;
        }

        
    }
}
