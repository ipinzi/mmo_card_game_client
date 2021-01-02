using MMO_Card_Game.Scripts.Player;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Populate Command", menuName = "MMO Card Game/Commands/Populate Command", order = 0)]
    public class PopulateCommand : Command
    {
        private void Awake()
        {
            cmd = "populate";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var popPacket = JsonUtility.FromJson<PopulateDataPacket>(jsonString);
            gameManager.StartCoroutine(gameManager.WaitForSceneLoaded(() =>
            {
                var pawn = Object.Instantiate(gameManager.playerPrefab,
                    popPacket.position,
                    Quaternion.identity).GetComponent<Pawn>();
                pawn.isLocalPlayer = false;
                pawn.name = "Player: " + popPacket.user + " (" + popPacket.id + ")";
                pawn.username = popPacket.user;
                pawn.id = popPacket.id;
                gameManager.pawns.Add(pawn);
            }));
        }
    }
    [System.Serializable]
    public class PopulateDataPacket : DataPacket
    {
        public string id;
        public string user;
        public Vector3 position;
    }
}