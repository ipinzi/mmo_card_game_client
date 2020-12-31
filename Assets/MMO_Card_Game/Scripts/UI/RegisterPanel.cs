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
            /*dynamic jsonData = new JObject();
            jsonData.cmd = "register";
            
            jsonData.data = new JObject();
            jsonData.data.username = userField.text;
            jsonData.data.email = emailField.text;
            jsonData.data.password = passField.text;*/
            
            FindObjectOfType<WebsocketManager>().SendData("{'cmd': 'register', 'data': {'username': '"+userField.text+"', 'email': '"+emailField.text+"', 'password': '"+passField.text+"'}}");
        }
        private void Back()
        {
            menuManager.OpenMenu(0);
        }
    }
}
