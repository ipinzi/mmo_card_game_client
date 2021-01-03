using System.Collections.Generic;
using MMO_Card_Game.Scripts.Networking.Commands;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking
{
    [CreateAssetMenu(menuName = "MMO Card Game/Network Settings", fileName = "Network Settings", order = 0)]
    public class NetworkSettings : ScriptableObject
    {
        public string ip = "localhost";
        public string port = "4000";
        public List<Command> commands;

        public bool debugMode = false;
    }
}
