using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.TacticalCCG.Pathfinding;
using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG
{
    public enum PlayerType
    {
        Human,
        CPU
    }

    public enum BoardSide
    {
        North,South,East,West
    }
    public class CardGamePlayer : MonoBehaviour
    {
        public string playerName;
        public PlayerType playerType;
        public Color color;
        public Deck storedDeck;
        public BoardSide side;
        public Vector2 lifepointLocation;
        
        [Header("Game Variables")]
        public Deck deck;
        public List<Card> hand;

        public event Action<int> onActionPointUpdated;
        public int actionPoints
        {
            get => _actionPoints;
            set
            {
                _actionPoints = value;
                onActionPointUpdated?.Invoke(_actionPoints);
            }
        }
        [SerializeField] private int _actionPoints = 5;
        
        
        public bool showHand = true;
        public bool isPlayerTurn;
        public bool hasPlayerLostGame;
        public Waypoint lifepointsObj;
        public event Action onTurnStart;
        public event Action onMoveFinished;
        public event Action onPlayerLost;

        void Awake()
        {
            SetupDeck(storedDeck);

        }

        public void TryAttackMove(GridSpace gridSpace, CardGamePawn attackingCard, Vector3[] path, int moveCost)
        {
            if (moveCost > actionPoints)
            {
                Debug.Log("Not enough ap for this move");
                return;
            }
            var targetCard = gridSpace.occupants
                .FirstOrDefault(occupant => occupant.GetComponent<CardGamePawn>())
                ?.GetComponent<CardGamePawn>();
            if (targetCard && attackingCard.owner == targetCard.owner)
            {
                Debug.Log("Can't attack a card that you own");
                return;
            }
            
            actionPoints -= moveCost;
            
            if (targetCard)
            {
                //pop last node from path so we do not move to the attacked square but the one before it
                var updatedPath = path.ToList();
                updatedPath.RemoveAt(path.Length - 1);
                path = updatedPath.ToArray();
                attackingCard.MoveOnPath(path, () =>
                {
                    Debug.Log("Path Complete");
                    Debug.Log(attackingCard.card.cardName + " attacks "+ targetCard.card.cardName + " at "+gridSpace.gridLocation);
                    attackingCard.Attack(targetCard);
                    onMoveFinished?.Invoke();
                });
            }
            else
            {
                Debug.Log("Attempting to move to grid space "+gridSpace.gridLocation);
                attackingCard.MoveOnPath(path, () =>
                {
                    Debug.Log("Path Complete");
                    onMoveFinished?.Invoke();
                });
            }
        }

        public bool PlayCard(GridSpace gridSpace, Transform cardTransform)
        {
            var cardData = cardTransform.GetComponent<CardRenderer>().cardData;
            if (FindObjectOfType<BoardManager>().IsValidCard(this,
                cardData, (int) gridSpace.gridLocation.x,
                (int) gridSpace.gridLocation.y))
            {
                actionPoints -= cardData.ap;
                hand.Remove(cardData);
                if (cardData.cardType == "summon")
                {
                    gridSpace.occupants.Add(cardTransform.gameObject);
                    cardTransform.GetComponent<CardGamePawn>().currentSpace = gridSpace;
                    //gridSpace.GetComponent<Waypoint>().isWalkable = false;
                }
                GetComponent<CardController>().currentSelection = null;

                var handPos = GetComponent<CardController>().GetHandPosition(cardTransform);
                var originalPos = cardTransform.position;
                cardTransform.parent = null;
                cardTransform.position = handPos;
                cardTransform.DOMove(gridSpace.transform.position,.75f).OnComplete(() =>
                {
                    onMoveFinished?.Invoke();
                });
                var addedRotationVector = new Vector3(90, 0, 0);
                if (side == BoardSide.South)
                {
                    addedRotationVector = new Vector3(90, 0, 180);
                }
                cardTransform.DORotate(gridSpace.transform.eulerAngles + addedRotationVector,.75f);
                return true;
            }

            return false;
        }

        public void SetupDeck(Deck d)
        {
            deck = ScriptableObject.CreateInstance<Deck>();
            deck.name = playerName + "'s Deck";
            deck.deckName = d.deckName;
            deck.cards = new List<Card>(d.cards);
        }

        public void NextTurn()
        {
            isPlayerTurn = true;
            onTurnStart?.Invoke();
        }

        public void PlayerLostGame()
        {
            hasPlayerLostGame = true;
            onPlayerLost?.Invoke();
        }
    }
}