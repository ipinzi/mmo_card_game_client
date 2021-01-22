using System;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Cards
{
    //TODO: Current version uses sprites as is easier to set up but will implement a texture based version later
    [ExecuteInEditMode]
    public class CardRendererSprite : MonoBehaviour
    {
        
        [Title("Settings")]
        [OnValueChanged(nameof(CardRefreshRender))]
        public Card cardData;
        public float levelIconSeparation = 2f;

        [Title("Renderers")]
        public TMP_Text levelText;
        public TMP_Text nameText;
        public TMP_Text descriptionText;
        public TMP_Text cardTypeText;
        public TMP_Text attackText;
        public TMP_Text defenseText;
        public SpriteRenderer cardFront;
        public SpriteRenderer cardImage;
        public SpriteRenderer typeIcon;
        
        [Title("Sprites Needed")]
        public Sprite summonCardFrontSprite;
        public Sprite equipCardFrontSprite;
        public Sprite itemCardFrontSprite;
        
        [Title("Equip Slots")]
        public GameObject slotIcon;
        public Transform equipSlotContainer;

        [Title("Summon Type Icons")]
        public Sprite beastIcon;
        public Sprite mechIcon;
        public Sprite flareIcon;
        public Sprite shockIcon;
        public Sprite waterIcon;
        public Sprite gaiaIcon;

        private void Start()
        {
            CardRefreshRender();
        }
        private void CardRefreshRender()
        {
            EnableNonSummonCardElements();
            
            nameText.text = cardData.cardName;
            if(nameText.text.Length <= 20)
            {
                nameText.fontSize = 6;
            }
            if (nameText.text.Length > 20)
            {
                nameText.fontSize = 5;
            }
            if (nameText.text.Length > 40)
            {
                nameText.fontSize = 4;
            }
            
            descriptionText.text = cardData.cardText;
            if (descriptionText.text.Length > 130)
            {
                descriptionText.fontSize = 3;
            }
            else
            {
                descriptionText.fontSize = 4;
            }
            
            cardTypeText.text = FirstCharToUpper(cardData.cardType);
            cardImage.sprite = cardData.cardSprite;
            
            if (cardData.GetType() == typeof(SummonCard))
            {
                var summonCard = (SummonCard) cardData;
                levelText.text = summonCard.level.ToString();
                attackText.text = summonCard.attack.ToString();
                defenseText.text = summonCard.defense.ToString();
                cardFront.sprite = summonCardFrontSprite;
                FillEquipSlotContainer(summonCard.equipSlots);
                ApplySummonTypeIcon(summonCard.summonType);
            }
            else if (cardData.GetType() == typeof(EquipmentCard))
            {
                DisableNonSummonCardElements();
                
                var equipCard = (EquipmentCard) cardData;
                levelText.text = equipCard.level.ToString();
                ApplySummonTypeIcon(equipCard.summonType);
                cardFront.sprite = equipCardFrontSprite;

            }
            else if (cardData.GetType() == typeof(ItemCard))
            {
                DisableNonSummonCardElements();

                levelText.enabled = false;
                cardFront.sprite = itemCardFrontSprite;
                var itemCard = (ItemCard) cardData;
            }
        }

        private void DisableNonSummonCardElements()
        {
            attackText.enabled = false;
            defenseText.enabled = false;
            typeIcon.enabled = false;
        }
        private void EnableNonSummonCardElements()
        {
            attackText.enabled = true;
            defenseText.enabled = true;
            typeIcon.enabled = true;
            levelText.enabled = true;
        }
        private void FillEquipSlotContainer(int slots)
        {
            if (!Application.isPlaying) return;
            
            for(var i=0;i<slots;i++)
            {
                var pos = equipSlotContainer.position;
                pos.x -= (levelIconSeparation * i);
                var icon = Instantiate(slotIcon, pos, Quaternion.identity, equipSlotContainer);
            }
        }

        private void ApplySummonTypeIcon(SummonType summonType)
        {
            switch (summonType)
            {
                case SummonType.Beast:
                    typeIcon.sprite = beastIcon;
                    break;
                case SummonType.Mech:
                    typeIcon.sprite = mechIcon;
                    break;
                case SummonType.Flare:
                    typeIcon.sprite = flareIcon;
                    break;
                case SummonType.Shock:
                    typeIcon.sprite = shockIcon;
                    break;
                case SummonType.Water:
                    typeIcon.sprite = waterIcon;
                    break;
                case SummonType.Gaia:
                    typeIcon.sprite = gaiaIcon;
                    break;
                case SummonType.None:
                    typeIcon.enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(summonType), summonType, null);
            }
        }
        private string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
