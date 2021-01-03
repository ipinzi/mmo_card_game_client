using MMO_Card_Game.Scripts.NPC;
using UnityEngine;
using UnityEngine.AI;

namespace MMO_Card_Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private NavMeshAgent _navAgent;
        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
        }
        
        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var cam = Camera.main;
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    _navAgent.destination = hit.point;
                    var interaction = hit.transform.GetComponent<Interaction>();
                    if (interaction)
                    {
                        interaction.RunInteraction();
                    }
                }
            }
        }
    }
}