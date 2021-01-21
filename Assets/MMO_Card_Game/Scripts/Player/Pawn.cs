using Cinemachine;
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

            if (isLocalPlayer)
            {
                Game.LocalPlayer.pawn = this;
            }
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

            if (!isLocalPlayer)
            {
                Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
                Destroy(GetComponent<PlayerController>());
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
            //if this pawn is the local player then send our position to the server to sync this player
            if (!_wsManager || !isLocalPlayer) return;
            
            var pos = transform.position;
            var posPacket = new CommandDataObject("pos");
            posPacket.AddData("x",pos.x);
            posPacket.AddData("y",pos.y);
            posPacket.AddData("z",pos.z);
            _wsManager.SendData(posPacket.Data());
        }
    }
}
