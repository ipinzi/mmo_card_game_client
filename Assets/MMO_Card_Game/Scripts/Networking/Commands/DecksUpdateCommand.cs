using System;
using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Decks Update Command", menuName = "MMO Card Game/Commands/Decks Update Command", order = 0)]
    public class DecksUpdateCommand : Command
    {
        private void Awake()
        {
            cmd = "decksUpdate";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var decksPacket = JsonUtility.FromJson<DeckUpdatePacket>(jsonString);
            
            //reallocate the memory from the old list to new empty list, old list will be dropped on GC
            Game.LocalPlayer.decks = new List<Deck>();
            
            foreach (var deck in decksPacket.decks)
            {
                var newDeck = CreateInstance<Deck>();
                newDeck.deckName = deck.deckName;
                newDeck.name = newDeck.deckName;
                foreach (var id in deck.cards)
                {
                    newDeck.cards.Add(Game.Manager.cardDatabase.GetCard(id));
                }
                Game.LocalPlayer.decks.Add(newDeck);
            }
        }
    }
    [Serializable]
    public class DeckUpdatePacket : DataPacket
    {
        public List<DeckSerializableData> decks;
    }
}