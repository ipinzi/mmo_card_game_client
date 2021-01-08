using System;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Money Update Command", menuName = "MMO Card Game/Commands/Money Update Command", order = 0)]
    public class MoneyUpdateCommand : Command
    {
        private void Awake()
        {
            cmd = "moneyUpdate";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var moneyPacket = JsonUtility.FromJson<MoneyUpdatePacket>(jsonString);
            Game.LocalPlayer.money = moneyPacket.money;
        }
    }
    [Serializable]
    public class MoneyUpdatePacket : DataPacket
    {
        public int money;
    }
}