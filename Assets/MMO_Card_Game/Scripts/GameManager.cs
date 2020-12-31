using System.Collections.Generic;
using MMO_Card_Game.Scripts.Player;
using UnityEngine;

namespace MMO_Card_Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject playerPrefab;
        public List<Pawn> pawns;

        private void Awake()
        {
            pawns = new List<Pawn>();
        }
    }
}
