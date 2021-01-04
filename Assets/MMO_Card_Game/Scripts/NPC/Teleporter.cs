using MMO_Card_Game.Scripts.Networking;
using UnityEngine;

namespace MMO_Card_Game.Scripts.NPC
{
    public class Teleporter : Interaction
    {
        
        public int toZone = 2;
        public Vector3 toPosition;

        public override void RunInteraction()
        {
            Teleport();
        }

        public void Teleport()
        {
            var telePacket = new CommandDataObject("teleport");
            telePacket.AddData("zone", toZone);
            telePacket.AddData("x", toPosition.x);
            telePacket.AddData("y", toPosition.y);
            telePacket.AddData("z", toPosition.z);
            
            var wsMan = FindObjectOfType<WebsocketManager>();
            wsMan.SendData(telePacket.Data());
            wsMan.GetComponent<GameManager>().LoadScene(toZone);
        }
    }
}