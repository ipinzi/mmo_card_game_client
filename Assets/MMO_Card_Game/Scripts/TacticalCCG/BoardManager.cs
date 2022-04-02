using System;
using System.Collections.Generic;
using System.Linq;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.TacticalCCG.Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MMO_Card_Game.Scripts.TacticalCCG
{

    public enum BoardMovement
    {
        HorizontalVertical,
        Diagonal,
        Both
    }
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gridSpacePrefab;
        [SerializeField] private GameObject _boardPlane;
        [SerializeField] private int _numOfSquares = 6;

        public GridSpace[,] board;
        private CardGamePlayer[] players;
        
        
        // Start is called before the first frame update
        void Start()
        {
            CreateGrid(_numOfSquares);
            players = FindObjectsOfType<CardGamePlayer>();
        }

        public Vector3 GridWorldPosition(int x, int y)
        {
            return board[x, y].transform.position;
        }

        public List<GameObject> GridOccupants(int x, int y)
        {
            return board[x, y].occupants;
        }
        public CardGamePawn GridOccupant(int x, int y)
        {
            foreach (var o in board[x, y].occupants)
            {
                if (o.TryGetComponent<CardGamePawn>(out var pawn))
                {
                    return pawn;
                }
            }
            return null;
        }

        void CreateGrid(int size)
        {
            board = new GridSpace[size,size];
            var width = _boardPlane.GetComponent<Collider>().bounds.size.x;
            var height = _boardPlane.GetComponent<Collider>().bounds.size.z;

            var gridObj = new GameObject();
            gridObj.name = "Grid";
            gridObj.transform.position = transform.position;

            var squareWidth = (width / size);
            
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var pos = new Vector3((i * (width / size))+squareWidth/2, 0,
                        (j * (height / size))+squareWidth/2);
                    board[i,j] = Instantiate(_gridSpacePrefab,gridObj.transform.position + pos, Quaternion.identity, gridObj.transform).GetComponent<GridSpace>();
                    board[i, j].gridLocation = new Vector2(i, j);
                }
            }
        }
        public void BoardSetupMovement(BoardMovement movementType)
        {
            var size = _numOfSquares;
            if(movementType == BoardMovement.HorizontalVertical || movementType == BoardMovement.Both)
            {
                //Horizontal movement board
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        var waypoint = board[i, j].GetComponent<Waypoint>();
                        waypoint.neighbors.Clear();
                        if(j+1 < size) waypoint.neighbors.Add(board[i, j + 1].GetComponent<Waypoint>());
                        if(j-1 >= 0) waypoint.neighbors.Add(board[i, j - 1].GetComponent<Waypoint>());
                        if(i+1 < size) waypoint.neighbors.Add(board[i + 1, j].GetComponent<Waypoint>());
                        if(i-1 >= 0) waypoint.neighbors.Add(board[i - 1, j].GetComponent<Waypoint>());
                    }
                }
            }
            if(movementType == BoardMovement.Diagonal || movementType == BoardMovement.Both)
            {
                //Horizontal movement board
                for (var i = 0; i < size; i++)
                {
                    for (var j = 0; j < size; j++)
                    {
                        var waypoint = board[i, j].GetComponent<Waypoint>();
                        if(movementType != BoardMovement.Both) waypoint.neighbors.Clear();
                        if(i+1 < size &&  j+1 < size) waypoint.neighbors.Add(board[i + 1, j + 1].GetComponent<Waypoint>());
                        if(i-1 >= 0 && j-1 >= 0) waypoint.neighbors.Add(board[i - 1, j - 1].GetComponent<Waypoint>());
                        if(i+1 < size && j-1 >= 0) waypoint.neighbors.Add(board[i + 1, j - 1].GetComponent<Waypoint>());
                        if(i-1 >= 0 && j+1 < size) waypoint.neighbors.Add(board[i - 1, j + 1].GetComponent<Waypoint>());
                    }
                }
            }

            foreach (var player in players)
            {
                board[(int)player.lifepointLocation.x,(int)player.lifepointLocation.y].GetComponent<Waypoint>().neighbors.Add(player.lifepointsObj);
            }
        }

        public List<CardGamePawn> HasCardsOnBoard(CardGamePlayer player = null)
        {
            var size = _numOfSquares;
            var cards = new List<CardGamePawn>();
            //Horizontal movement board
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var card = board[i, j].GetOccupant();
                    if (!player && card)
                    {
                        cards.Add(card);
                        continue;
                    }
                    if (card && player == card.owner) cards.Add(card);
                }
            }

            return cards;
        }

        public bool LifepointsAreExposed(CardGamePlayer player)
        {
            var lpLocation = player.lifepointLocation;
            if (board[(int) lpLocation.x, (int) lpLocation.y].GetOccupant()) return true;

            return false;
        }

        public bool IsValidCard(CardGamePlayer player, Card cardData, int x, int y)
        {
            if (player.actionPoints - cardData.ap < 0)
            {
                Debug.Log("Not enough ap to use card");
                return false;
            } 
            
            return IsValidSummon(player, cardData, x, y);
        }
        
        public bool IsValidSummon(CardGamePlayer player, Card cardData, int x, int y)
        {
            if (cardData.cardType != "summon")
            {
                Debug.Log("Cannot Summon. Card is not a summon type card");
                return false;
            }
            
            foreach (var occupant in board[x, y].occupants)
            {
                if (occupant.GetComponent<CardGamePawn>())
                {
                    Debug.Log("Cannot summon. Space is occupied");
                    return false;
                }
            }
            
            switch(player.side)
            {
                case BoardSide.North:
                    if (y != 0) return false;
                    break;
                case BoardSide.South:
                    if (y != _numOfSquares-1) return false;
                    break;
                case BoardSide.East:
                    if (x != 0) return false;
                    break;
                case BoardSide.West:
                    if (x != _numOfSquares-1) return false;
                    break;
            }

            return true;
        }

        public Vector2 GetRandomSummonSpot(CardGamePlayer player)
        {
            return player.side switch
            {
                BoardSide.North => new Vector2(Random.Range(0, _numOfSquares ), 0),
                BoardSide.South => new Vector2(Random.Range(0, _numOfSquares ), _numOfSquares - 1),
                BoardSide.East => new Vector2(0, Random.Range(0, _numOfSquares - 1)),
                BoardSide.West => new Vector2(_numOfSquares - 1, Random.Range(0, _numOfSquares - 1))
            };
        }
    }
}