using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.TacticalCCG.Pathfinding;
using Sirenix.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MMO_Card_Game.Scripts.TacticalCCG.AI
{
    public class AIController : MonoBehaviour
    {
        private CardGamePlayer player;
        private BoardManager boardManager;
        private PathFinder pathFinder;
        private CardGameManager cardGameManager;
    
        void Awake()
        {
            pathFinder = FindObjectOfType<PathFinder>();
            boardManager = FindObjectOfType<BoardManager>();
            player = GetComponent<CardGamePlayer>();
            player.onTurnStart += CpuDoTurn;
            player.onMoveFinished += CpuDoTurn;
            cardGameManager = boardManager.GetComponent<CardGameManager>();
        }

        private void CpuDoTurn()
        {
            if (player.playerType != PlayerType.CPU || !player.isPlayerTurn) return;

            if (player.actionPoints <= 0) return;
            
            CpuDoMove();
        }

        private void CpuDoMove()
        {
            var moveList = SimulateMoves(20);

            var bestMove = GetBestMove(moveList);
            if (bestMove == null)
            {
                cardGameManager.NextTurn();
                return;
            };
            
            ExecuteMove(bestMove);
        }

        private List<AIMove> SimulateMoves(int iterations)
        {
            var moveList = new List<AIMove>();
            //var cardsOnBoard = boardManager.HasCardsOnBoard(player);
            for(var i=0;i<iterations;i++){
                /*if(cardsOnBoard.Count > 0 && boardManager.LifepointsAreExposed(player))
                {
                    //Move to attack lifepoints
                }
                else
                {*/
                    var newMove = SimulateRandomMove();
                    if(newMove != null) moveList.Add(newMove);
                //}
            }

            return moveList;
        }

        private AIMove SimulateRandomMove()
        {
            var summonCardsInHand = CardTypeInHand("summon");
            var rand = (AIMoveType)Random.Range(0, 2);
            
            
            if (rand == AIMoveType.Summon){
                //summon a card
                if (summonCardsInHand.Count <= 0) return null;
                var randCard = summonCardsInHand[Random.Range(0, summonCardsInHand.Count - 1)];
                if (randCard.card.ap > player.actionPoints) return null;
                var summonCoords = boardManager.GetRandomSummonSpot(player);
                var gridSpace = boardManager.board[(int)summonCoords.x, (int)summonCoords.y];
                
                if (gridSpace.GetOccupant() != null) return null;
                
                var summonCard = (SummonCard) randCard.card;
                return new AIMove(AIMoveType.Summon, gridSpace, summonCard.attack, summonCard.ap, randCard, null);
                
                
                
            }else if (rand ==  AIMoveType.AttackMove)
            {
                var cardPawns = boardManager.HasCardsOnBoard();
                foreach (var p in cardGameManager.players)
                {
                    cardPawns.Add(p.lifepointsObj.GetComponent<GridSpace>().GetOccupant().GetComponent<CardGamePawn>());
                }
                var myPawns = cardPawns.Where(p => p.owner == player && !p.CompareTag("Lifepoints")).ToList();
                if (myPawns.Count == 0) return null;
                var otherPawns = cardPawns.Where(p => p.owner != player && !p.CompareTag("Lifepoints")).ToList();
                if (otherPawns.Count == 0) return null;
                var attackingPawn = myPawns[Random.Range(0, myPawns.Count - 1)];
                var defendingPawn = otherPawns[Random.Range(0, otherPawns.Count - 1)];
                
                pathFinder.CalculatePath(attackingPawn.transform.position, defendingPawn.transform.position);
                if (player.actionPoints < pathFinder.generatedPath.Length)
                {
                    Array.Resize(ref pathFinder.generatedPath,player.actionPoints);
                }
                if (pathFinder.generatedPath.Length <= 1) return null;

                var pathEnd = pathFinder.generatedPath[pathFinder.generatedPath.Length - 1];
                var lastGridSpace = pathFinder.FindClosestWaypoint(pathEnd).GetComponent<GridSpace>();
                var newMove = new AIMove(AIMoveType.AttackMove, lastGridSpace, 1, pathFinder.generatedPath.Length, attackingPawn, pathFinder.generatedPath);
                pathFinder.ClearPath();
                return newMove;
                
            }else if (rand ==  AIMoveType.AttackLifepoints)
            {
                    
            }


            return null;
        }

        private void ExecuteMove(AIMove move)
        {
            switch (move.type)
            {
                case AIMoveType.Summon:
                    player.PlayCard(move.gridSpace, move.card.transform);
                    break;
                case AIMoveType.AttackMove:
                    player.TryAttackMove(move.gridSpace, move.card, move.path ,move.cost);
                    break;
                case AIMoveType.AttackLifepoints:
                    break;
            }
        }

        private AIMove GetBestMove(List<AIMove> moveList)
        {
            AIMove bestMove = null;
            foreach (var move in moveList)
            {
                bestMove ??= move;
                if (move.cost - move.damage < bestMove.cost - bestMove.damage) bestMove = move;
            }

            return bestMove;
        }

        private List<CardGamePawn> CardTypeInHand(string cardType)
        {
            var cards = new List<CardGamePawn>();
            foreach (Transform cardObj in player.GetComponent<CardController>().handLocation)
            {
                if (cardObj.TryGetComponent<CardGamePawn>(out var card))
                {
                    if(card.card && card.card.cardType == cardType) cards.Add(card);
                }
            }

            return cards;
        }
    }
}
