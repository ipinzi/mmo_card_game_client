using System.Timers;
using MMO_Card_Game.Scripts.Networking;
using UnityEngine;
using UnityEngine.AI;

namespace MMO_Card_Game.Scripts.Player
{
    public class Pawn : NetworkID
    {
        public string username;
        public float positionSendInterval = .5f;
        public bool isLocalPlayer = true;
        
        private NavMeshAgent _navAgent;
        private WebsocketManager _wsManager;
        private float _posIntervalTimer = 0;
        
        // Start is called before the first frame update
        private void Start()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _wsManager = FindObjectOfType<WebsocketManager>();
        }
        
        // Update is called once per frame
        private void Update()
        {
            _posIntervalTimer += Time.deltaTime;
            
            if (IsMoving())
            {
                Debug.Log("Moving");

                if (_posIntervalTimer > positionSendInterval)
                {
                    SendPosition();
                    _posIntervalTimer = 0f;
                }
            }

            if (!isLocalPlayer) return;
            //player controls here
            
            if (Input.GetButtonDown("Fire1"))
            {
                var cam = Camera.main;
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    _navAgent.destination = hit.point;
                }
            }
        }

        public void SetDestination(Vector3 point)
        {
            _navAgent.destination = point;
        }
    
        private bool IsMoving()
        {
            return _navAgent.remainingDistance > 0;
        }

        private void SendPosition()
        {
            if(_wsManager) _wsManager.SendData(
                "{'cmd': 'pos', 'data': {'x': '"+transform.position.x+"','y': '"+transform.position.y+"','z': '"+transform.position.z+"'}}"
            );
        }
    }
}
