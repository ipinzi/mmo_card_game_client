using MMO_Card_Game.Scripts.NPC;
using UnityEngine;
using UnityEngine.AI;

namespace MMO_Card_Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float interactionDistance = 2f;
        
        private Transform _target;
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
                    var interaction = hit.collider.GetComponentInParent<Interaction>();
                    if (interaction) _target = interaction.transform;
                }
            }
            if (_target && Vector3.SqrMagnitude(_target.position - transform.position) < interactionDistance)
            {
                Debug.Log("Trying to run interaction");
                var interaction = _target.GetComponent<Interaction>();
                interaction.RunInteraction();
                _target = null;
            }
        }
    }
}