using System.Collections.Generic;
using MMO_Card_Game.Scripts.Networking.Commands;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking
{
    [CreateAssetMenu(menuName = "MMO Card Game/Network Settings", fileName = "Network Settings", order = 0)]
    public class NetworkSettings : ScriptableObject
    {
        [HorizontalGroup("Group")]
        [BoxGroup("Group/Settings")]
        [LabelWidth(100)]
        public string ip = "localhost";
        [BoxGroup("Group/Settings")]
        [LabelWidth(100)]
        public string port = "4000";
        [VerticalGroup("Group/Commands")]
        public List<Command> commands;

        [BoxGroup("Group/Settings")]
        public bool debugMode = false;
    }
}
