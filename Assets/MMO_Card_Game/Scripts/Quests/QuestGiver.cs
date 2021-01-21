using MMO_Card_Game.Scripts.Networking;
using MMO_Card_Game.Scripts.NPC;

namespace MMO_Card_Game.Scripts.Quests
{
    public class QuestGiver : Interaction
    {
        public Quest quest;
        
        public override void RunInteraction()
        {
            //Send quest add request to server
            var data = new CommandDataObject("questAdd");
            data.AddData("questID", quest.id);
            data.AddData("playerZone", Game.LocalPlayer.zone);
            Game.Network.SendData(data.Data());
        }
    }
}
