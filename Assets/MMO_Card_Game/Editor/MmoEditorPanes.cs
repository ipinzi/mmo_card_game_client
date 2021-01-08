using System.Collections.Generic;
using System.Linq;
using MMO_Card_Game.Scripts.Cards;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Editor
{
    public class SummonCardsPane : CardsPane
    {
        [Searchable, TableList, OnValueChanged(nameof(DoNotAdd))]
        public List<Card> summonCards;

        public SummonCardsPane(List<Card> cardsToFilter)
        {
            _cards = cardsToFilter;
            RefreshList();
        }

        protected override void RefreshList()
        {
            summonCards = new List<Card>();
            foreach (var card in _cards.Where(card => card.GetType() == typeof(SummonCard)))
            {
                summonCards.Add(card);
            }
        }
    }
    public class EquipmentCardsPane : CardsPane
    {
        [Searchable, TableList, OnValueChanged(nameof(DoNotAdd))]
        public List<Card> equipmentCards;

        public EquipmentCardsPane(List<Card> cardsToFilter)
        {
            _cards = cardsToFilter;
            RefreshList();
        }
        protected override void RefreshList()
        {
            equipmentCards = new List<Card>();
            foreach (var card in _cards.Where(card => card.GetType() == typeof(EquipmentCard)))
            {
                equipmentCards.Add(card);
            }
        }
    }
    public class ItemCardsPane : CardsPane
    {
        [Searchable, TableList, OnValueChanged(nameof(DoNotAdd))]
        public List<Card> itemCards;

        public ItemCardsPane(List<Card> cardsToFilter)
        {
            _cards = cardsToFilter;
            RefreshList();
        }
        protected override void RefreshList()
        {
            itemCards = new List<Card>();
            foreach (var card in _cards.Where(card => card.GetType() == typeof(ItemCard)))
            {
                itemCards.Add(card);
            }
        }
    }
    
    public class CardsPane
    {
        protected List<Card> _cards;
        protected virtual void RefreshList()
        {
            
        }
        protected void DoNotAdd()
        {
            RefreshList();
            Debug.LogWarning("Do not add or remove cards here / to this list!! Please add new cards to the Card Database.");
        }
    }
}