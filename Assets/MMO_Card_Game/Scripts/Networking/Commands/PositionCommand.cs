using System.Linq;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Position Command", menuName = "MMO Card Game/Commands/Position Command", order = 0)]
    public class PositionCommand : Command
    {
        private void Awake()
        {
            cmd = "pos";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var posPacket = JsonUtility.FromJson<PositionDataPacket>(jsonString);

            foreach (var p in gameManager.pawns.Where(p => p.id == posPacket.id))
            {
                p.SetDestination(posPacket.position);
            }
        }
    }
    [System.Serializable]
    public class PositionDataPacket : DataPacket
    {
        public string id;
        public Vector3 position;
    }
}