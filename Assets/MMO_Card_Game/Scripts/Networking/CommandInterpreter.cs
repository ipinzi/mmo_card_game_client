using System;
using System.Linq;
using MMO_Card_Game.Scripts.Networking.Commands;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking
{
    public class CommandInterpreter
    {
        private WebsocketManager _websocketManager;
        private GameManager _gameManager;
        public CommandInterpreter(WebsocketManager websocketManager)
        {
            _websocketManager = websocketManager;
            _gameManager = _websocketManager.GetComponent<GameManager>();
        }
        public void InterpretCommand(string jsonString)
        {
            //rewrite into modular version
            try
            {
                var jsonData = JsonUtility.FromJson<DataPacket>(jsonString);
                var commands = _gameManager.networkSettings.commands;

                foreach (var command in commands.Where(command => jsonData.cmd == command.cmd))
                {
                    command.RunCommand(_gameManager, jsonString);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Server : {jsonString}");
            }
        }
    }
}