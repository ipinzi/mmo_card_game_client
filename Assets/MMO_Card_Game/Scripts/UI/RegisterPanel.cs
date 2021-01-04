using MMO_Card_Game.Scripts.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MMO_Card_Game.Scripts.UI
{
    public class RegisterPanel : MonoBehaviour
    {
        public MenuManager menuManager;
        public TMP_InputField userField;
        public TMP_InputField emailField;
        public TMP_InputField passField;
        public Button registerButton;
        public Button backButton;

        private void Awake()
        {
            userField.onSubmit.AddListener(Register);
            emailField.onSubmit.AddListener(Register);
            passField.onSubmit.AddListener(Register);
            registerButton.onClick.AddListener(Register);
            backButton.onClick.AddListener(Back);
        }
        private void Register(string str)
        {
            Submit();
        }
        private void Register()
        {
            Submit();
        }
        private void Submit()
        {
            var registerPacket = new CommandDataObject("register");
            registerPacket.AddData("username", userField.text);
            registerPacket.AddData("email", emailField.text);
            registerPacket.AddData("password", passField.text);
            FindObjectOfType<WebsocketManager>().SendData(registerPacket.Data());
        }
        private void Back()
        {
            menuManager.OpenMenu(0);
        }
    }
}
