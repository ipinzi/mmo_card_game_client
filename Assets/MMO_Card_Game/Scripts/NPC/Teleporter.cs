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
            FindObjectOfType<WebsocketManager>().SendData("{'cmd': 'teleport', 'data': {'zone': '"+toZone+"', 'x': '"+toPosition.x+", 'y': '"+toPosition.y+", 'z': '"+toPosition.z+"'}}");
        }
    }
}