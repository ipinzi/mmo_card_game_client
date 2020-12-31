using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Networking
{
    public class NetworkID : MonoBehaviour
    {
        [ReadOnly] public string id = Guid.NewGuid().ToString();

        [FoldoutGroup("Edit ID"), Button("Regenerate ID", ButtonSizes.Small)]
        private void GenerateID()
        {
            id =  Guid.NewGuid().ToString();
        }
    }
}
