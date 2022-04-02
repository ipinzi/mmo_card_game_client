using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG.Pathfinding
{
    public class Waypoint : MonoBehaviour
    {
        public List<Waypoint> neighbors;
        [HideInInspector]public Waypoint previous;
        [HideInInspector]public float distance;
        public bool isWalkable = true;

        
        /*[Button]
    public void CreateNextWaypoint()
    {
        var waypointPrefab = FindObjectOfType<CardGameManager>().waypointPrefab;
        var waypointObj = (GameObject) PrefabUtility.InstantiatePrefab(waypointPrefab);
        var waypoint = waypointObj.GetComponent<Waypoint>();

        waypoint.transform.position = transform.position + Vector3.right;
        waypoint.transform.parent = transform.parent;
        Selection.activeObject = waypoint.gameObject;
            
        waypoint.neighbors.Add(this);
        neighbors.Add(waypoint);
    }*/

        public Waypoint waypointToJoin;
        [Button]
        public void JoinWaypoints()
        {
            waypointToJoin.neighbors.Add(this);
            neighbors.Add(waypointToJoin);
        }

        [Button]
        public void DeleteWaypoint()
        {
            foreach (var neighbor in neighbors)
            {
                neighbors.Remove(neighbor);
            }
            Destroy(gameObject);
        }
        void OnDrawGizmos()
        {
            if (neighbors == null) return;

            Gizmos.color = isWalkable ? Color.blue : Color.red;
        
            foreach(var neighbor in neighbors){
                if (neighbor)
                {
                    Gizmos.DrawLine(transform.position, neighbor.transform.position);
                }
            }
        }
    }
}