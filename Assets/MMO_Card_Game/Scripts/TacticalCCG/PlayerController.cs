using System.Collections;
using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        var ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.GetComponent<CardRenderer>())
            {
                
            }
        }
    }
}
