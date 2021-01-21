using System.Collections.Generic;
using System.Linq;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.Quests;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SummonMmoEditorPane : MmoEditorPane
    {
        [InfoBox("This is a filtered view. You can edit cards here only.")]
        [Searchable, TableList, OnValueChanged(nameof(DoNotAdd))]
        public List<Card> summonCards;

        public SummonMmoEditorPane(List<Card> cardsToFilter)
        {
            _cards = cardsToFilter;
            RefreshList();
        }

        protected sealed override void RefreshList()
        {
            summonCards = new List<Card>();
            foreach (var card in _cards.Where(card => card.GetType() == typeof(SummonCard)))
            {
                summonCards.Add(card);
            }
        }
    }
    public class EquipmentMmoEditorPane : MmoEditorPane
    {
        [InfoBox("This is a filtered view. You can edit cards here only.")]
        [Searchable, TableList, OnValueChanged(nameof(DoNotAdd))]
        public List<Card> equipmentCards;

        public EquipmentMmoEditorPane(List<Card> cardsToFilter)
        {
            _cards = cardsToFilter;
            RefreshList();
        }
        protected sealed override void RefreshList()
        {
            equipmentCards = new List<Card>();
            foreach (var card in _cards.Where(card => card.GetType() == typeof(EquipmentCard)))
            {
                equipmentCards.Add(card);
            }
        }
    }
    public class ItemMmoEditorPane : MmoEditorPane
    {
        [InfoBox("This is a filtered view. You can edit cards here only.")]
        [Searchable, TableList, OnValueChanged(nameof(DoNotAdd))]
        public List<Card> itemCards;

        public ItemMmoEditorPane(List<Card> cardsToFilter)
        {
            _cards = cardsToFilter;
            RefreshList();
        }
        protected sealed override void RefreshList()
        {
            itemCards = new List<Card>();
            foreach (var card in _cards.Where(card => card.GetType() == typeof(ItemCard)))
            {
                itemCards.Add(card);
            }
        }
    }

    public class TasksPane : MmoEditorPane
    {
        [InfoBox("You can edit tasks here but if you want to add them to a quest you should drag them from the "
        +"[Quest/Tasks] dropdown in the explorer view on the left.")]
        [Searchable, TableList, OnValueChanged(nameof(DoNotAdd))]
        public List<QuestTask> tasks;

        public TasksPane()
        {
            RefreshList();
        }

        protected sealed override void RefreshList()
        {
            var tasksObj = AssetDatabase.FindAssets("t:QuestTask", new[] {"Assets/MMO_Card_Game/Data"});
            var questTasks = new List<QuestTask>();
            foreach (var guid in tasksObj) {
                var t = AssetDatabase.LoadAssetAtPath<QuestTask>(AssetDatabase.GUIDToAssetPath(guid));
                questTasks.Add(t);
            }

            tasks = questTasks;
        }
    }
    
    public class MmoEditorPane
    {
        protected List<Card> _cards;
        protected virtual void RefreshList()
        {
            
        }
        protected void DoNotAdd()
        {
            RefreshList();
            Debug.LogWarning("Do not add or remove to this list!! Please add new data to the specified database.");
        }
    }
}