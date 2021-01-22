using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Cards
{
    public class Card : ScriptableObject
    {
        [ReadOnly]
        [TableColumnWidth(10, true)]
        public string id = Guid.NewGuid().ToString();
        //[OnValueChanged(nameof(UpdateCardName))]
        [TableColumnWidth(100, true), OnValueChanged(nameof(RenameAsset))]
        public string cardName;
        [TextArea]
        [TableColumnWidth(200, true)]
        public string cardText = "";
        [PreviewField]
        //TODO: Possibly remove this (cardSprite) if sprite drawing functionality not needed
        [TableColumnWidth(20, true), HideInInspector]
        public Sprite cardSprite;
        [TableColumnWidth(20, true), PreviewField]
        public Texture cardImage;
        [ReadOnly]
        [TableColumnWidth(30, true)]
        public string cardType;

        private void Awake()
        {
            cardName = name;
            
            var t = GetType();
            if (t == typeof(SummonCard)) cardType = "summon";
            if (t == typeof(EquipmentCard)) cardType = "equipment";
            if (t == typeof(ItemCard)) cardType = "item";
        }
        private void RenameAsset()
        {
            #if UNITY_EDITOR
                var path = AssetDatabase.GetAssetPath(this);
                AssetDatabase.RenameAsset(path, cardName);
            #endif
        }

    }
    
    [Serializable]
    public class CardSerializableData
    {
        public string id;
        public string cardName;
        public string cardText = "";
        public string cardType = "";
        public int attack;
        public int defense;
        public SummonType summonType;
        public int level;
        public int equipSlots;
        
        public CardSerializableData(Card card)
        {
            id = card.id;
            cardName = card.cardName;
            cardText = card.cardText;
            cardType = card.cardType;
            if (card.GetType() == typeof(SummonCard))
            {
                var summonCard = (SummonCard) card;
                attack = summonCard.attack;
                defense = summonCard.defense;
                summonType = summonCard.summonType;
                level = summonCard.level;
                equipSlots = summonCard.equipSlots;
            }
            if (card.GetType() == typeof(EquipmentCard))
            {
                var equipCard = (EquipmentCard) card;
                level = equipCard.level;
                summonType = equipCard.summonType;
            }
            if (card.GetType() == typeof(ItemCard))
            {
                var itemCard = (ItemCard) card;
            }
        }
    }
}