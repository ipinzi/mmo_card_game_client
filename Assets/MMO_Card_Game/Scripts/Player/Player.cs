using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        public int money;
        public List<Card> cards;
        public List<Deck> decks;

        private void Awake()
        {
            decks = new List<Deck>();
            Game.LocalPlayer = this;
        }
    }
}