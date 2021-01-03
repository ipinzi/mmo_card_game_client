using TMPro;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Login Command", menuName = "MMO Card Game/Commands/Login Command", order = 0)]
    public class LoginCommand: Command
    {
        private void Awake()
        {
            cmd = "login";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var loginPacket = JsonUtility.FromJson<LoginDataPacket>(jsonString);
            Debug.Log(loginPacket.message);
            GameObject.FindWithTag("FormFeedback").GetComponent<TMP_Text>().text = loginPacket.message;

            if (loginPacket.success)
            {
                Debug.Log($"Server : Connected as user - {loginPacket.username}");
            }

        }
        [System.Serializable]
        public class LoginDataPacket : DataPacket
        {
            public bool success;
            public string message;
            public string username;
        }
    }
}