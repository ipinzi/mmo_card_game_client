using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.Quests;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [Sirenix.OdinInspector.ReadOnly]
        public Pawn pawn;
        
        public int money;
        public List<Card> cards;
        public List<Deck> decks;
        public List<Quest> quests;
        
        [Sirenix.OdinInspector.ReadOnly]
        public int zone;
        
        public Quest GetQuest(string id)
        {
            return quests.FirstOrDefault(q => id == q.id);
        }
        
        private void Awake()
        {
            Game.LocalPlayer = this;
            
            decks = new List<Deck>();
        }
    }
}