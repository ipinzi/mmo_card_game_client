using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Cards
{
    [CreateAssetMenu(menuName = "MMO Card Game/Deck", fileName = "Deck", order = 0)]
    public class Deck : ScriptableObject
    {
        public string deckName;
        public List<Card> cards;

        public void Shuffle()
        {
            cards.Shuffle();
        }

        public Card DrawCard()
        {
            if (cards.Count <= 0)
            {
                Debug.LogWarning("No Cards to Draw with DrawCard()");
                return null;
            }
            
            var c = cards[0];
            cards.RemoveAt(0);
            return c;
        }
        public List<Card> DrawCards(int amount)
        {
            var list = new List<Card>();
            for (var i = 0; i < amount; i++)
            {
                list.Add(DrawCard());
            }

            return list;
        }
    }
    
    [System.Serializable]
    public class DeckSerializableData
    {
        public string deckName;
        public List<string> cards;
    }
}