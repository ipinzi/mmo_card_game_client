using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Cards Update Command", menuName = "MMO Card Game/Commands/Cards Update Command", order = 0)]
    public class CardsUpdateCommand : Command
    {
        private void Awake()
        {
            cmd = "cardsUpdate";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var cuPacket = JsonUtility.FromJson<CardsUpdatePacket>(jsonString);
            Game.LocalPlayer.cards = new List<Card>();
                    
            foreach (var id in cuPacket.cards)
            {
                Game.LocalPlayer.cards.Add(Game.Manager.cardDatabase.GetCard(id));
            }
        }
    }
    [System.Serializable]
    public class CardsUpdatePacket : DataPacket
    {
        public string[] cards;
    }
}