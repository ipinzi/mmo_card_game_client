using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MMO_Card_Game.Scripts.Cards;
using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG
{
    public class CardGameManager : MonoBehaviour
    {
        public GameObject cardPrefab;
        public int actionPointsPerTurn = 3;
        public bool alwaysStartHuman;
        public BoardManager boardManager;
        public List<CardGamePlayer> players;

        public CardGamePlayer currentTurnPlayer;
        private int playerTurnIndex = -1;
        
        bool gameOver;

        void Start()
        {
            players = FindObjectsOfType<CardGamePlayer>().ToList();

            foreach (var player in players)
            {
                player.deck.Shuffle();
                player.hand = player.deck.DrawCards(5);

                var controller = player.GetComponent<CardController>();
                StartCoroutine(SpawnCards(player, player.hand.ToArray(), 0.3f));

                player.onActionPointUpdated += ap =>
                {
                    if (ap <= 0) player.isPlayerTurn = false;
                };
                player.onMoveFinished += () =>
                {
                    if (player.actionPoints <= 0) NextTurn(player);
                };
                player.onPlayerLost += () =>
                {
                    var playersList = CheckPlayersLeftInGame();
                    if (playersList.Count <= 1)
                    {
                        DisableAllPlayers();
                        gameOver = true;
                        Debug.Log("GAME HAS BEEN WON BY "+playersList[0].playerName);
                    }
                };
            }

            if (players.Count <= 0)
            {
                Debug.Log("NO PLAYERS IN SCENE");
                return;
            }
            
            NextTurn();
        }

        private void DisableAllPlayers()
        {
            foreach (var p in players)
            {
                p.isPlayerTurn = false;
            }
        }

        private List<CardGamePlayer> CheckPlayersLeftInGame()
        {
            var plist = new List<CardGamePlayer>();
            foreach (var p in players)
            {
                if (p == null) continue;
                if (p.hasPlayerLostGame) continue;
                plist.Add(p);
            }

            return plist;
        }

        IEnumerator SpawnCards(CardGamePlayer player, Card[] cards, float delay = 0.2f)
        {
            foreach (var card in cards)
            {
                SpawnCard(player, card);
                yield return new WaitForSeconds(delay);
            }
        }
        
        Transform SpawnCard(CardGamePlayer player, Card card)
        {
            var controller = player.GetComponent<CardController>();
            var cObj = Instantiate(cardPrefab, controller.deckLocation.position, controller.deckLocation.rotation).GetComponent<CardRenderer>();
            cObj.cardData = card;
            cObj.transform.parent = player.GetComponent<CardController>().handLocation;
            cObj.GetComponent<CardGamePawn>().owner = player;
            return cObj.transform;
        }

        public void NextTurn(CardGamePlayer thisPlayer = null)
        {
            if (gameOver) return;
            if (thisPlayer)
            {
                if(thisPlayer != currentTurnPlayer) return;
            }
            
            if (playerTurnIndex == -1)
            {
                playerTurnIndex = Random.Range(0, players.Count);
                if (alwaysStartHuman)
                {
                    foreach (var p in players.Where(p => p.playerType == PlayerType.Human))
                    {
                        currentTurnPlayer = p;
                        playerTurnIndex = players.IndexOf(p);
                    }
                }
            }
            else
            {
                playerTurnIndex++;
            }

            if (playerTurnIndex >= players.Count) playerTurnIndex = 0;
            currentTurnPlayer = players[playerTurnIndex];

            foreach (var player in players)
            {
                player.isPlayerTurn = false;
            }
        
            //Draw Card
            var card = currentTurnPlayer.deck.DrawCard();
            if(card){
                currentTurnPlayer.hand.Add(card);
                SpawnCard(currentTurnPlayer, card);
            }
            else
            {
                Debug.Log("No more cards in deck D:");
            }
            currentTurnPlayer.actionPoints += actionPointsPerTurn;
            currentTurnPlayer.NextTurn();
        }
    }
}
