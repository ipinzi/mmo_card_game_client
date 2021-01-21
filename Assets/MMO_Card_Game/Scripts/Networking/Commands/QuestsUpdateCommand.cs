using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Quest Update Command", menuName = "MMO Card Game/Commands/Quest Update Command", order = 0)]
    public class QuestsUpdateCommand : Command
    {
        private void Awake()
        {
            cmd = "questsUpdate";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            //add quest after server confirms quest add is valid
            var questsUpdatePacket = JsonUtility.FromJson<QuestsUpdatePacket>(jsonString);

            Debug.Log(questsUpdatePacket.type + "ing Quest (From server)."+jsonString);

            switch (questsUpdatePacket.type)
            {
                case "add":
                    //Do add logic
                    break;
                case "progress":
                    //Do progress logic
                    break;
            }

            for(var i=0; i<questsUpdatePacket.questIds.Length; i++)
            {
                var quest = Game.Manager.questDatabase.GetQuest(questsUpdatePacket.questIds[i]);
                quest.progress = questsUpdatePacket.questProgress[i];
                Game.LocalPlayer.quests.Add(quest);
            }
        }
    }
    [System.Serializable]
    public class QuestsUpdatePacket : DataPacket
    {
        public string type;
        public string[] questIds;
        public int[] questProgress;
    }
}