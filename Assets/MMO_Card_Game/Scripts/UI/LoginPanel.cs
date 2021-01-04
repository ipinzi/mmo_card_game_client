using MMO_Card_Game.Scripts.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MMO_Card_Game.Scripts.UI
{
    public class LoginPanel : MonoBehaviour
    {
        public MenuManager menuManager;
        public TMP_InputField userField;
        public TMP_InputField passField;
        public Button loginButton;
        public Button registerButton;

        private void Awake()
        {
            userField.onSubmit.AddListener(Login);
            passField.onSubmit.AddListener(Login);
            loginButton.onClick.AddListener(Login);
            registerButton.onClick.AddListener(OpenRegisterMenu);
        }
        private void Login(string str)
        {
            Submit();
        }
        private void Login()
        {
            Submit();
        }
        private void Submit()
        {
            var loginPacket = new CommandDataObject("login");
            loginPacket.AddData("username", userField.text);
            loginPacket.AddData("password", passField.text);
            
            FindObjectOfType<WebsocketManager>().SendData(loginPacket.Data());
        }
        private void OpenRegisterMenu()
        {
            menuManager.OpenMenu(1);
        }
    }
}