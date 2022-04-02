using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG.AI
{
    public enum AIMoveType
    {
        Summon = 0,
        AttackMove = 1,
        AttackLifepoints = 2,
    }
    public class AIMove
    {
        public AIMoveType type;
        public GridSpace gridSpace;
        public int damage;
        public int cost;
        public CardGamePawn card;
        public Vector3[] path;

        public AIMove(AIMoveType _type, GridSpace _gridSpace, int _damage, int _cost, CardGamePawn _card, Vector3[] _path)
        {
            type = _type;
            gridSpace = _gridSpace;
            damage = _damage;
            cost = _cost;
            card = _card;
            path = _path;
        }
    }
}