using System.Linq;
using DG.Tweening;
using MMO_Card_Game.Scripts.Cards;
using MMO_Card_Game.Scripts.TacticalCCG.Pathfinding;
using UnityEngine;

namespace MMO_Card_Game.Scripts.TacticalCCG
{
    public class CardController : MonoBehaviour
    {
        public LayerMask gridLayer;
        
        [Header ("Animation")]
        public Transform deckLocation;
        public Transform handLocation;
        public float handSpacing = 2f;
        public float cardPopUpTime = 0.05f;
        public float cardPopUpDistance = 0.2f;
        public float cardPopForwardDistance = 1f;
        
        
        private Camera cam;
        public bool showHand = true;
        private Vector3 handStartPos;
        [HideInInspector]public Transform currentSelection;
        private CardGamePawn attackSelection;
        private int lastHandIndex;
        private LineRenderer attackLine;
        private CardGamePlayer player;
        private PathFinder pathFinder;
        private int prevGridSpaceID;
        private int moveCost = 0;

        void Start()
        {
            cam = Camera.main;
            player = GetComponent<CardGamePlayer>();
            pathFinder = GetComponent<PathFinder>();
            handStartPos = handLocation.position;
        }
        void Update()
        {
            Transform hitByRay = null;
            if(player.playerType == PlayerType.Human && player.isPlayerTurn){
                //Mouseover
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                hitByRay = DoMouseOver(ray, hitByRay);

                DoMouseOverWhileSelecting(ray);
                
                //Global mouse release
                if (Input.GetButtonUp("Fire1"))
                {
                    ClearSelection();
                }
                
                //Update hand position
                var handPos = showHand ? handStartPos : handStartPos + (-handLocation.up * 1f); 
                handLocation.DOMove(handPos, 0.15f).SetEase(Ease.OutCubic);
            }

            //Update card in hand positions
            foreach (Transform ho in handLocation)
            {
                if (ho == hitByRay) continue;

                ho.parent = handLocation;

                var pos = GetHandPosition(ho)-handLocation.position;
                handLocation.DOKill();
                ho.DOLocalMove(pos, 0.15f).SetEase(Ease.InOutBounce);
                ho.DORotate(handLocation.eulerAngles, 0.1f).SetEase(Ease.InOutBounce);
            }
        }
        
        private void DoMouseOverWhileSelecting(Ray ray)
        {
            if (currentSelection)
            {
                if (Physics.Raycast(ray, out var gridHit, 100f, gridLayer) && gridHit.transform.GetComponent<GridSpace>())
                {
                    currentSelection.DOMove(gridHit.transform.position + new Vector3(0, 0.1f, 0), 0.1f).SetEase(Ease.InOutBounce);
                    currentSelection.DORotate(gridHit.transform.eulerAngles + new Vector3(90, 0, 0), 0.1f)
                        .SetEase(Ease.InOutBounce);

                    if (Input.GetButtonUp("Fire1"))
                    {
                        var gridSpace = gridHit.transform.GetComponent<GridSpace>();
                        player.PlayCard(gridSpace, currentSelection);
                    }
                }
                else
                {
                    var pos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y, 4f));
                    currentSelection.DOMove(pos, 0.1f).SetEase(Ease.InOutBounce);
                    currentSelection.DORotate(handLocation.eulerAngles, 0.1f).SetEase(Ease.InOutBounce);
                }
            }
        }

        private Transform DoMouseOver(Ray ray, Transform hitByRay)
        {
            if (!currentSelection && Physics.Raycast(ray, out var hit))
            {
                if (attackSelection)
                {
                    //pathFinder.CalculatePath(attackSelection.transform.position, hit.point);
                }
                
                if (hit.transform.GetComponent<CardRenderer>() && hit.transform.GetComponent<CardGamePawn>().owner == player)
                {
                    hitByRay = hit.transform;
                    var dirToCam = cam.transform.position - hit.point;
                    hitByRay.DOMove(
                        GetHandPosition(hitByRay) + (hitByRay.up * cardPopUpDistance) +
                        (dirToCam * cardPopForwardDistance),
                        cardPopUpTime).SetEase(Ease.InOutBounce);

                    if (Input.GetButtonDown("Fire1"))
                    {
                        SetSelection(hit.transform);
                    }
                }
                else if (hit.transform.TryGetComponent<GridSpace>(out var gridSpace))
                {
                    if(Input.GetButtonDown("Fire1")){
                        var occupants = gridSpace.occupants;
                        var o = occupants.FirstOrDefault(o => o.GetComponent<CardGamePawn>());
                        if (o && o.GetComponent<CardGamePawn>().owner == player)
                        {
                            InitAttackMove(o.GetComponent<CardGamePawn>());
                        }
                    }

                    if (hit.transform.gameObject.GetInstanceID() != prevGridSpaceID)
                    {
                        if (attackSelection)
                        {
                            GetBoardRoute(attackSelection.transform.position, hit.transform.position, BoardMovement.HorizontalVertical);
                            moveCost = pathFinder.generatedPath.Length-1;
                        }
                    }
                    if (attackSelection)
                    {
                        var targetOccupent = gridSpace.GetOccupant();
                        pathFinder.ChangeColor(moveCost > player.actionPoints || (targetOccupent && attackSelection.owner == targetOccupent.owner) ? Color.red : Color.cyan);
                    }
                    prevGridSpaceID = hit.transform.gameObject.GetInstanceID();

                    if (Input.GetButtonUp("Fire1"))
                    {
                        if (attackSelection) player.TryAttackMove(gridSpace, attackSelection, pathFinder.generatedPath, moveCost);
                    }
                }
            }

            return hitByRay;
        }

        public void GetBoardRoute(Vector3 startPos, Vector3 endPos, BoardMovement movementType)
        {
            var boardManager = FindObjectOfType<BoardManager>();
            boardManager.BoardSetupMovement(movementType);
            pathFinder.CalculatePath(startPos, endPos);
        }

        public void InitAttackMove(CardGamePawn pawn)
        {
            Debug.Log("Selecting placed card "+pawn.card.name);
            attackSelection = pawn;
        }
        void SetSelection(Transform selection)
        {
            currentSelection = selection;
            lastHandIndex = currentSelection.GetSiblingIndex();
            currentSelection.parent = null;
            showHand = false;
        }

        public void ClearSelection()
        {
            if (currentSelection)
            {
                currentSelection.parent = handLocation;
                currentSelection.SetSiblingIndex(lastHandIndex);
            }
            currentSelection = null;
            attackSelection = null;
            showHand = true;
            pathFinder.ClearPath();
        }
        public Vector3 GetHandPosition(Transform cardTransform)
        {
            var spacer = handLocation.right * (cardTransform.GetSiblingIndex() * handSpacing);
            var halfWidth = ((handLocation.right * (handLocation.childCount * handSpacing))/2) - (handLocation.right * (handSpacing/2));
            var zOffset = -handLocation.forward*(cardTransform.GetSiblingIndex()*0.0075f);
            return handLocation.position + zOffset + spacer - halfWidth;
        }
    }
}
