using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG
{
    public class ApUI : MonoBehaviour
    {
        public CardGamePlayer player;
        public GameObject actionPointImageUI;

        private void Start()
        {
            Refresh();

            player.onActionPointUpdated += ap => { Refresh(); };
        }

        private void Refresh()
        {
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            for(var i=0;i<player.actionPoints;i++)
            {
                Instantiate(actionPointImageUI, Vector3.zero, Quaternion.identity, transform);
            }
        }
    }
}
