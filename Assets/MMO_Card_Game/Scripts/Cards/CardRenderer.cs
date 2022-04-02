using System;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


//TODO: Finish this when ready to start rendering cards using textures instead of sprites
namespace MMO_Card_Game.Scripts.Cards
{
    [ExecuteInEditMode]
    public class CardRenderer : MonoBehaviour
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
        public Renderer cardFront;
        public Renderer cardImage;
        public Renderer typeIcon;
        
        [Title("Sprites Needed")]
        public Texture summonCardFrontTexture;
        public Texture equipCardFrontTexture;
        public Texture itemCardFrontTexture;
        
        [Title("Equip Slots")]
        public GameObject slotIcon;
        public Transform equipSlotContainer;

        [Title("Summon Type Icons")]
        public Texture beastIcon;
        public Texture mechIcon;
        public Texture flareIcon;
        public Texture shockIcon;
        public Texture waterIcon;
        public Texture gaiaIcon;

        private MaterialPropertyBlock _matBlock;

        private void Start()
        {
            CardRefreshRender();
        }
        [Button("Refresh")]
        private void CardRefreshRender()
        {
            _matBlock = new MaterialPropertyBlock();
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
            cardImage.GetPropertyBlock(_matBlock);
            _matBlock.SetTexture("_BaseMap", cardData.cardImage);
            cardImage.SetPropertyBlock(_matBlock);
            
            if (cardData.GetType() == typeof(SummonCard))
            {
                var summonCard = (SummonCard) cardData;
                levelText.text = summonCard.level.ToString();
                attackText.text = summonCard.attack.ToString();
                defenseText.text = summonCard.defense.ToString();
                //cardFront.sprite = summonCardFrontTexture;
                SetCardFrontTexture(summonCardFrontTexture);
                FillEquipSlotContainer(summonCard.equipSlots);
                ApplySummonTypeIcon(summonCard.summonType);
            }
            else if (cardData.GetType() == typeof(EquipmentCard))
            {
                DisableNonSummonCardElements();
                
                var equipCard = (EquipmentCard) cardData;
                levelText.text = equipCard.level.ToString();
                //cardFront.sprite = equipCardFrontTexture;
                SetCardFrontTexture(equipCardFrontTexture);
                ApplySummonTypeIcon(equipCard.summonType);
            }
            else if (cardData.GetType() == typeof(ItemCard))
            {
                DisableNonSummonCardElements();
                SetCardFrontTexture(itemCardFrontTexture);
                levelText.enabled = false;
                //cardFront.sprite = itemCardFrontTexture;
                var itemCard = (ItemCard) cardData;
            }
        }

        private void SetCardFrontTexture(Texture texture)
        {
            cardFront.GetPropertyBlock(_matBlock);
            _matBlock.SetTexture("_BaseMap", texture);
            cardFront.SetPropertyBlock(_matBlock);
        }
        private void SetTypeIconTexture(Texture texture)
        {
            typeIcon.enabled = true;
            typeIcon.GetPropertyBlock(_matBlock);
            _matBlock.SetTexture("_BaseMap", texture);
            typeIcon.SetPropertyBlock(_matBlock);
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
            
            ClearEquipSlotContainers();
            for(var i=0;i<slots;i++)
            {
                var pos = equipSlotContainer.position;
                pos.x -= (levelIconSeparation * i);
                var icon = Instantiate(slotIcon, pos, Quaternion.identity, equipSlotContainer);
            }
        }
        private void ClearEquipSlotContainers()
        {
            if (!Application.isPlaying) return;
            
            for(var i=0;i<equipSlotContainer.childCount;i++)
            {
                Destroy(equipSlotContainer.GetChild(i).gameObject);
            }
        }

        private void ApplySummonTypeIcon(SummonType summonType)
        {
            switch (summonType)
            {
                case SummonType.Beast:
                    SetTypeIconTexture(beastIcon);
                    break;
                case SummonType.Mech:
                    SetTypeIconTexture(mechIcon);
                    break;
                case SummonType.Flare:
                    SetTypeIconTexture(flareIcon);
                    break;
                case SummonType.Shock:
                    SetTypeIconTexture(shockIcon);
                    break;
                case SummonType.Water:
                    SetTypeIconTexture(waterIcon);
                    break;
                case SummonType.Gaia:
                    SetTypeIconTexture(gaiaIcon);
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
