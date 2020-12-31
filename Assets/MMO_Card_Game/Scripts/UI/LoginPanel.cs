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
            /*dynamic jsonData = new JObject();
            jsonData.cmd = "login";
            
            jsonData.data = new JObject();
            jsonData.data.username = userField.text;
            jsonData.data.password = passField.text;*/
            
            FindObjectOfType<WebsocketManager>().SendData("{'cmd': 'login', 'data': {'username': '"+userField.text+"', 'password': '"+passField.text+"'}}");
        }
        private void OpenRegisterMenu()
        {
            menuManager.OpenMenu(1);
        }
    }
}