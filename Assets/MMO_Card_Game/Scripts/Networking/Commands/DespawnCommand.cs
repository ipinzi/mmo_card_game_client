using System.Linq;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Despawn Command", menuName = "MMO Card Game/Commands/Despawn Command", order = 0)]
    public class DespawnCommand : Command
    {
        private void Awake()
        {
            cmd = "despawn";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var dsPacket = JsonUtility.FromJson<DespawnPacket>(jsonString);
                        
            foreach (var p in gameManager.pawns.Where(p => p.id == dsPacket.id))
            {
                gameManager.pawns.Remove(p);
                Destroy(p.gameObject);
            }
        }
    }
    [System.Serializable]
    public class DespawnPacket : DataPacket
    {
        public string id;
    }
}