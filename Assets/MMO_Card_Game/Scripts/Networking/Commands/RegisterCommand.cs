using MMO_Card_Game.Scripts.UI;
using TMPro;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking.Commands
{
    [CreateAssetMenu(fileName = "Register Command", menuName = "MMO Card Game/Commands/Register Command", order = 0)]
    public class RegisterCommand : Command
    {
        private void Awake()
        {
            cmd = "register";
        }
        public override void RunCommand(GameManager gameManager, string jsonString)
        {
            var registerPacket = JsonUtility.FromJson<RegisterDataPacket>(jsonString);
            Debug.Log($"Server : {registerPacket.message}");
            GameObject.FindWithTag("FormFeedback").GetComponent<TMP_Text>().text = registerPacket.message;

            if (registerPacket.success)
            {
                FindObjectOfType<MenuManager>().OpenMenu(0);
            }
        }
    }
    [System.Serializable]
    public class RegisterDataPacket : DataPacket
    {
        public bool success;
        public string message;
    }
}