using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    //[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class Command : ScriptableObject
    {
        public string cmd = "";

        public virtual void RunCommand(GameManager gameManager, string jsonString)
        {
            
        }
    }

    //Incoming Data Packets
    [System.Serializable]
    public class DataPacket
    {
        public string cmd;
    }
}