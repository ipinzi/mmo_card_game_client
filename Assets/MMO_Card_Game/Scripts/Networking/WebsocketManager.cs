using System.Text;
using NativeWebSocket;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MMO_Card_Game.Scripts.Networking
{
    public class WebsocketManager : MonoBehaviour
    {
        public string ip = "localhost";
        public string port = "4000";
        private WebSocket _websocket;
        private bool _isConnected = false;
        private bool _isShuttingDown = false;
        private CommandInterpreter _commandInterpreter;

        private void Awake()
        {
            var wsManagers = FindObjectsOfType<WebsocketManager>();
            if (wsManagers.Length > 1)
            {
                Debug.Log("Another Websocket Manager was found in the scene. Destroying the newer one.");
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            
            _commandInterpreter = new CommandInterpreter(this);
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            ConnectToServer();
        }

        private async void ConnectToServer()
        {
            if (_websocket != null) await _websocket.Close();
            
            _websocket = new WebSocket($"ws://{ip}:{port}");

            _websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
                _isConnected = true;
                //SendWebSocketMessage();
            };

            _websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
                if (!_isShuttingDown && !_isConnected) Invoke(nameof(ConnectToServer), 2f);
            };

            _websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed! Attempting to reconnect..");
                _isConnected = false;
                //if (!_isShuttingDown) Invoke(nameof(ConnectToServer), 2f);
            };

            _websocket.OnMessage += (bytes) =>
            {
                var str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                _commandInterpreter.InterpretCommand(str);
            };

            // waiting for messages
            await _websocket.Connect();
            
        }

        private void Update()
        {
            #if !UNITY_WEBGL || UNITY_EDITOR
                _websocket.DispatchMessageQueue();
            #endif
        }
        
        //test message
        private async void SendWebSocketMessage()
        {
            if (_websocket.State != WebSocketState.Open) return;

            var json = MakeJsonFromString("{'cmd': 'login', 'data': {'user': 'ben', 'pass': 'dragonbz'}}");
            dynamic jsonData = JObject.Parse(json);
            Debug.Log(jsonData.data.user);
        
            // Sending plain text
            await _websocket.SendText("plain text message");
            await _websocket.SendText(json);
        }

        public async void SendData(JObject json)
        {
            var jsonStr = JsonConvert.SerializeObject(json);
            await _websocket.SendText(jsonStr);
        }
        public async void SendData(string json)
        {
            var str = MakeJsonFromString(json);
            await _websocket.SendText(str);
        }
        private string MakeJsonFromString(string jsonString)
        {
            return jsonString.Replace("'", "\"");
        }
        private async void OnApplicationQuit()
        {
            await _websocket.Close();
            _isShuttingDown = true;
        }

    }
}