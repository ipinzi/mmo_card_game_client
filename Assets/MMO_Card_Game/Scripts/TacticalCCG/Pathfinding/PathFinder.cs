using System.Collections.Generic;
using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG.Pathfinding
{
    public class PathFinder : MonoBehaviour
    {
        public Vector3[] generatedPath;
        public Color pathColor = Color.red;
        public float pathDistance;
        
        private Stack<Vector3> currentPath;
        private Vector3 currentWaypointPosition;

        private void Start()
        {
//            CalculatePath(tempStartPos.transform.position, tempDestination.transform.position);
        }

        public void CalculatePath(Vector3 startPos, Vector3 destination)
        {
            currentPath = new Stack<Vector3>();
            var currentNode = FindClosestWaypoint(startPos);
            var endNode = FindClosestWaypoint(destination);

            if (currentNode == null || endNode == null || currentNode == endNode) return;

            var openList = new SortedList<float, Waypoint>();
            var closedList = new List<Waypoint>();
            openList.Add(0, currentNode);
            currentNode.previous = null;
            currentNode.distance = 0f;

            while (openList.Count > 0)
            {
                currentNode = openList.Values[0];
                openList.RemoveAt(0);
                var dist = currentNode.distance;
                closedList.Add(currentNode);
                
                if (currentNode == endNode) break;

                foreach (var neighbor in currentNode.neighbors)
                {
                    if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor)  || (neighbor != endNode && !neighbor.isWalkable)) continue;

                    neighbor.previous = currentNode;
                    neighbor.distance = dist + (neighbor.transform.position - currentNode.transform.position).magnitude+Random.Range(0,1f);
                    var distanceToTarget = (neighbor.transform.position - endNode.transform.position).magnitude;
                    openList.Add(neighbor.distance + distanceToTarget, neighbor);
                }
            }

            if (currentNode == endNode)
            {
                while (currentNode.previous)
                {
                    currentPath.Push(currentNode.transform.position);
                    pathDistance += Vector3.Distance(currentNode.transform.position,
                        currentNode.previous.transform.position);
                    currentNode = currentNode.previous;
                }
                currentPath.Push(startPos);
            }

            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = currentPath.Count;
            
            //offset path line
            var drawnPath = new Stack<Vector3>(currentPath).ToArray();
            for(var i=0;i<drawnPath.Length;i++)
            {
                drawnPath[i] += new Vector3(0, 0.2f, 0);
            }
            lineRenderer.SetPositions(drawnPath);

            ChangeColor(pathColor);
            
            generatedPath = currentPath.ToArray();
        }

        public void ChangeColor(Color color)
        {
            pathColor = color;
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = pathColor;
            lineRenderer.endColor = pathColor;
        }

        public void ClearPath()
        {
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }

        public Waypoint FindClosestWaypoint(Vector3 target)
        {
            GameObject closest = null;
            float closestDist = Mathf.Infinity;
            foreach (var waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
            {
                var dist = (waypoint.transform.position - target).magnitude;
                if (dist < closestDist)
                {
                    closest = waypoint;
                    closestDist = dist;
                }
            }

            if (closest)
            {
                return closest.GetComponent<Waypoint>();
            }

            return null;
        }
    }
}