﻿using Cinemachine;
using UnityEngine;

namespace MMO_Card_Game.Scripts.Player
{
    public class CMFreelookOnCondition : MonoBehaviour {
        private void Start(){
            CinemachineCore.GetInputAxis = GetAxisCustom;
        }
        public float GetAxisCustom(string axisName){
            if(axisName == "Mouse X"){
                if (Input.GetMouseButton(1)){
                    return UnityEngine.Input.GetAxis("Mouse X");
                } else{
                    return 0;
                }
            }
            else if (axisName == "Mouse Y"){
                if (Input.GetMouseButton(1)){
                    return UnityEngine.Input.GetAxis("Mouse Y");
                } else{
                    return 0;
                }
            }
            return UnityEngine.Input.GetAxis(axisName);
        }
    }
}