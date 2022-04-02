using System.Collections.Generic;
using MMO_Card_Game.Scripts.TacticalCCG.Pathfinding;
using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG
{
    public class GridSpace : MonoBehaviour
    {
        public Vector2 gridLocation;
        public List<GameObject> occupants;
        
        // Start is called before the first frame update
        void Start()
        {
            //occupants = new List<GameObject>();
        }

        public CardGamePawn GetOccupant()
        {
            foreach (var o in occupants)
            {
                if (o == null) continue;
                var pawn = o.GetComponent<CardGamePawn>();
                if (pawn) return pawn;
            }

            return null;
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Waypoint>().isWalkable = GetOccupant() == null;
        }
    }
}
