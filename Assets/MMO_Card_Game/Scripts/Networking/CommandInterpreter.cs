using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using MMO_Card_Game.Scripts.Player;
using MMO_Card_Game.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace MMO_Card_Game.Scripts.Networking
{
    public class CommandInterpreter
    {
        private WebsocketManager _websocketManager;
        private GameManager _gameManager;
        private bool _sceneLoaded = false;
        public CommandInterpreter(WebsocketManager websocketManager)
        {
            _websocketManager = websocketManager;
            _gameManager = _websocketManager.GetComponent<GameManager>();

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                _sceneLoaded = true;
            };
        }
        public void InterpretCommand(string jsonString)
        {
            try
            {
                var jsonData = JsonUtility.FromJson<DataPacket>(jsonString);

                switch (jsonData.cmd)
                {
                    case "login":
                        var loginPacket = JsonUtility.FromJson<LoginDataPacket>(jsonString);
                        Debug.Log(loginPacket.message);
                        GameObject.FindWithTag("FormFeedback").GetComponent<TMP_Text>().text = loginPacket.message;

                        if (loginPacket.success)
                        {
                            Debug.Log($"Server : Connected as user - {loginPacket.username}");
                        }

                        break;
                    
                    case "register":
                        var registerPacket = JsonUtility.FromJson<RegisterDataPacket>(jsonString);
                        Debug.Log($"Server : {registerPacket.message}");
                        GameObject.FindWithTag("FormFeedback").GetComponent<TMP_Text>().text = registerPacket.message;

                        if (registerPacket.success)
                        {
                            Object.FindObjectOfType<MenuManager>().OpenMenu(0);
                        }

                        break;
                    
                    case "setUser":

                        var setUserPacket = JsonUtility.FromJson<SetUserDataPacket>(jsonString);
                        LoadScene(setUserPacket.zone);
                        
                        _websocketManager.StartCoroutine(WaitForSceneLoaded(() => 
                        {
                            Debug.Log("Adding player at "+ setUserPacket.position);
                            var player = Object.Instantiate(_gameManager.playerPrefab,
                                setUserPacket.position,
                                Quaternion.identity).GetComponent<Pawn>();
                            player.name = "Player: "+setUserPacket.user+" ("+setUserPacket.id+")";
                            player.username = setUserPacket.user;
                            player.id = setUserPacket.id;
                        }));
                        break;
                    case "populate":
                        var popPacket = JsonUtility.FromJson<PopulateDataPacket>(jsonString);
                        _websocketManager.StartCoroutine(WaitForSceneLoaded(() => 
                        {
                            var pawn = Object.Instantiate(_gameManager.playerPrefab,
                                popPacket.position,
                                Quaternion.identity).GetComponent<Pawn>();
                            pawn.isLocalPlayer = false;
                            pawn.name = "Player: "+popPacket.user+" ("+popPacket.id+")";
                            pawn.username = popPacket.user;
                            pawn.id = popPacket.id;
                            _gameManager.pawns.Add(pawn);
                        }));
                        break;
                    case "despawn":
                        var dsPacket = JsonUtility.FromJson<DespawnPacket>(jsonString);
                        
                        foreach (var p in _gameManager.pawns.Where(p => p.id == dsPacket.id))
                        {
                            Object.Destroy(p.gameObject);
                        }

                        break;
                    case "pos":
                        var posPacket = JsonUtility.FromJson<PositionDataPacket>(jsonString);

                        foreach (var p in _gameManager.pawns.Where(p => p.id == posPacket.id))
                        {
                            p.SetDestination(posPacket.position);
                        }
                        
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Server : {jsonString}");
            }
        }

        private IEnumerator WaitForSceneLoaded(Action callbackFunc)
        {
            while (!_sceneLoaded)
            {
                yield return new WaitForSeconds(0.1f);
            }

            callbackFunc();
        }

        private void LoadScene(int buildIndex)
        {
            _sceneLoaded = false;
            SceneManager.LoadScene(buildIndex);
        }
    }


    //Incoming Data Packets
    [Serializable]
    public class DataPacket
    {
        public string cmd;
    }
    [Serializable]
    public class LoginDataPacket : DataPacket
    {
        public bool success;
        public string message;
        public string username;
    }
    [Serializable]
    public class RegisterDataPacket : DataPacket
    {
        public bool success;
        public string message;
    }
    [Serializable]
    public class SetUserDataPacket : DataPacket
    {
        public string id;
        public string user;
        public Vector3 position;
        public int zone;
    }
    [Serializable]
    public class PopulateDataPacket : DataPacket
    {
        public string id;
        public string user;
        public Vector3 position;
    }
    [Serializable]
    public class DespawnPacket : DataPacket
    {
        public string id;
    }
    [Serializable]
    public class PositionDataPacket : DataPacket
    {
        public string id;
        public Vector3 position;
    }
}