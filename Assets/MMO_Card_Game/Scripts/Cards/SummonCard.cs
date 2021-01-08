using Sirenix.OdinInspector;

namespace MMO_Card_Game.Scripts.Cards
{
    public enum SummonType
    {
        Beast,
        Mech,
        Flare,
        Shock,
        Frost,
        Gaia
    }
    public class SummonCard : Card
    {
        [TableColumnWidth(10, true)]
        public int attack;
        [TableColumnWidth(10, true)]
        public int defense;
        [TableColumnWidth(30, true)]
        public SummonType summonType;
    }
}