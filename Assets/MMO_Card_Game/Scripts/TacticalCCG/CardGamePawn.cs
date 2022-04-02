using System;
using System.Collections;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.TacticalCCG.Pathfinding;
using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG
{
    public class CardGamePawn : MonoBehaviour
    {
        public ParticleSystem attackPrefab;
        public Card card;
        public GridSpace currentSpace;
        public CardGamePlayer owner;

        public Vector3[] currentPath;
        public float moveSpeed = 1;
        public bool isMoving = false;
        public int hp = 0;

        private Coroutine moveCoroutine;
    
        // Start is called before the first frame update
        void Start()
        {
            card = GetComponent<CardRenderer>().cardData;

            if (card.GetType() == typeof(SummonCard))
            {
                var summonCard = (SummonCard) card;
                hp = summonCard.hp;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void MoveOnPath(Vector3[] path, Action onPathComplete)
        {
            if(moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(PathfindMove(path, onPathComplete));
        }

        IEnumerator PathfindMove(Vector3[] path, Action onPathComplete)
        {
            var currentWaypoint = 0;

            if (isMoving) yield break;

            currentSpace.occupants.Remove(gameObject);

            while (true)
            {
                if (currentWaypoint + 1 >= path.Length)
                {
                    isMoving = false;
                    var gridSpace = FindClosestGridspace(path[currentWaypoint]);
                    gridSpace.occupants.Add(gameObject);
                    currentSpace = gridSpace;
                    onPathComplete.Invoke();
                    yield break;
                };
                
                isMoving = true;
                if (Vector3.Distance(transform.position, path[currentWaypoint+1]) < 0.001f)
                {
                    currentWaypoint++;
                }

                if (currentWaypoint + 1 >= path.Length)
                {
                    continue;
                };
            
                transform.position = Vector3.MoveTowards(transform.position, path[currentWaypoint+1], moveSpeed);

                yield return null;
            }
        }

        private GridSpace FindClosestGridspace(Vector3 target)
        {
            GridSpace closest = null;
            float closestDist = Mathf.Infinity;
            foreach (var space in FindObjectsOfType<GridSpace>())
            {
                var dist = (space.transform.position - target).magnitude;
                if (dist < closestDist)
                {
                    closest = space;
                    closestDist = dist;
                }
            }

            if (closest)
            {
                return closest;
            }

            return null;
        }

        public void Attack(CardGamePawn targetCard)
        {
            var thisCard = (SummonCard) card;
            targetCard.hp -= thisCard.attack;
            var ps = Instantiate(attackPrefab, targetCard.currentSpace.transform.position, Quaternion.identity);
            ps.Play();

            if (targetCard.hp <= 0)
            {
                targetCard.hp = 0;
                if (!targetCard.CompareTag("Lifepoints"))
                {
                    targetCard.currentSpace.occupants.Remove(targetCard.gameObject);
                    Destroy(targetCard.gameObject);
                }
                else
                {
                    targetCard.owner.PlayerLostGame();
                }
            }
        }
    }
}
