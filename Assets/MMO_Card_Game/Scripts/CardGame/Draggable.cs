using System.Collections;
using System.Collections.Generic;
using MMO_Card_Game.Scripts.Cards;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool dragging;
    private Vector2 offset;

    public void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - offset;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
        Invoke(nameof(PointerDown),0.05f);
    }

    private void PointerDown()
    {
        dragging = true;
        transform.SetParent(GetComponentInParent<Canvas>().transform);
        GetComponent<CardInput>().ToggleClickable(false);   
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        GetComponent<CardInput>().DeselectCard(false);
        Invoke(nameof(TurnClickOn), 0.1f);
    }

    private void TurnClickOn()
    {
        GetComponent<CardInput>().ToggleClickable(true);
    }
}