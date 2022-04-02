using System.Collections;
using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.Player;
using UnityEngine;

public enum TurnPhase{
    Draw = 0,
    Equip = 1,
    Attack = 2,
    End = 3
}

public partial class CardGameManager : MonoBehaviour
{
    public Player player;
    public Player opponent;

    public GamePlayer gamePlayer;
    public GamePlayer gameOpponent;

    public GamePlayer currentTurn;
    public TurnPhase currentPhase;
    
    public GameObject cardRendererPrefab;
    public Canvas gameCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        gamePlayer = InitPlayer(player,gamePlayer);
        gameOpponent = InitPlayer(opponent, gameOpponent);
        
        gamePlayer.deck.Shuffle();
        gameOpponent.deck.Shuffle();

        gamePlayer.hand = gamePlayer.deck.DrawCards(5);
        gameOpponent.hand = gameOpponent.deck.DrawCards(5);

        StartCoroutine(AnimateCardDraw(gamePlayer, gamePlayer.hand));
        StartCoroutine(AnimateCardDraw(gameOpponent, gameOpponent.hand));

        currentPhase = TurnPhase.Draw;
        currentTurn = CoinFlip() ? gamePlayer : gameOpponent;
    }

    void NextTurn()
    {
        currentTurn = currentTurn == gamePlayer ? gameOpponent : gamePlayer;
        currentPhase = TurnPhase.Draw;
    }

    void NextPhase()
    {
        if (currentPhase == TurnPhase.End)
        {
            NextTurn();
            return;
        } 
        currentPhase++;
    }
    
    bool CoinFlip()
    {
        return Random.value > 0.5f;
    }

    GamePlayer InitPlayer(Player p, GamePlayer gp)
    {
        gp.deck = ScriptableObject.CreateInstance<Deck>();
        gp.deck.name = p.name + "'s Deck";
        gp.deck.deckName = p.decks[0].deckName;
        gp.deck.cards = new List<Card>(p.decks[0].cards);
        return gp;
    }

    IEnumerator AnimateCardDraw(GamePlayer pawn, List<Card> cards)
    {
        foreach (var card in cards)
        {
            StartCoroutine(AnimateCardDraw(pawn, card));
            yield return new WaitForSeconds(.2f);
        }
    }
    IEnumerator AnimateCardDraw(GamePlayer pawn, Card card)
    {
        var o = Instantiate(cardRendererPrefab);
        var cRenderer = o.GetComponent<CardRendererUI>();
        o.transform.SetParent(gameCanvas.transform);
        o.transform.localScale = new Vector3(1, 1, 1);
        cRenderer.cardData = card;
        cRenderer.CardRefreshRender();
        cRenderer.cardBack.gameObject.SetActive(true);
        
        float timer = 0f;
        while (timer < 1)
        {
            timer += Time.deltaTime*3f;
            o.transform.localPosition = Vector3.Lerp(pawn.deckLocation.localPosition,  pawn.handContainerLocation.localPosition, timer);
            yield return null;
        }
        
        cRenderer.cardBack.gameObject.SetActive(false);
        o.transform.SetParent(pawn.handContainer);
        o.GetComponent<CardInput>().owner = pawn;
    }
}
