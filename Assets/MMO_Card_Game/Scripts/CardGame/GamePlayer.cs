using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.Player;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public Player player;
    public Deck deck;
    public List<Card> hand;
    public Transform deckLocation;
    public Transform handContainer;
    public Transform handContainerLocation;
}