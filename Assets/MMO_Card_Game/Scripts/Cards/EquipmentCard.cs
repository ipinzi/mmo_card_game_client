using Sirenix.OdinInspector;

namespace MMO_Card_Game.Scripts.Cards
{
    public class EquipmentCard : Card
    {
        [TableColumnWidth(10, true)]
        public int level = 1;
        [TableColumnWidth(10, true)]
        public SummonType summonType = SummonType.None;
    }
}