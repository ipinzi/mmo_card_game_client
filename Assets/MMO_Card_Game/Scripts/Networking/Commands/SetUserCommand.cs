using MMO_Card_Game.Scripts.Player;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Set User Command", menuName = "MMO Card Game/Commands/Set User Command", order = 0)]
    public class SetUserCommand : Command
    {
        private void Awake()
        {
            cmd = "setUser";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var setUserPacket = JsonUtility.FromJson<SetUserDataPacket>(jsonString);
            gameManager.LoadScene(setUserPacket.zone);

            gameManager.StartCoroutine(gameManager.WaitForSceneLoaded(() =>
            {
                Debug.Log("Adding player at " + setUserPacket.position);
                var player = Object.Instantiate(gameManager.playerPrefab,
                    setUserPacket.position,
                    Quaternion.identity).GetComponent<Pawn>();
                player.name = "Player: " + setUserPacket.user + " (" + setUserPacket.id + ")";
                player.username = setUserPacket.user;
                player.id = setUserPacket.id;
            }));
        }
    }
    [System.Serializable]
    public class SetUserDataPacket : DataPacket
    {
        public string id;
        public string user;
        public Vector3 position;
        public int zone;
    }
}