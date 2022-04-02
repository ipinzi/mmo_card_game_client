using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardInput : MonoBehaviour
{
    public float selectSpeed;
    public float selectScale;
    [HideInInspector] public GamePlayer owner;
    
    private Coroutine _currentCoroutine;
    private Button _button;

    //private Vector3 _handLocation = new Vector3(Screen.width * 0.5f, (Screen.height * 0.5f) - 250f,0);
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }
    
    private void OnClick()
    {
        GetComponent<Draggable>().CancelInvoke();
        var otherCards = FindObjectsOfType<CardInput>();
        foreach (var card in otherCards)
        {
            card.DeselectCard();
        }
        SelectCard();
    }

    public void DeselectCard(bool scale = true)
    {
        if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Deselect(scale));
    }

    public void SelectCard()
    {
        if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(Select());
        _button.onClick.RemoveListener(OnClick);
    }

    public void ToggleClickable(bool canClick)
    {
        _button.onClick.RemoveListener(OnClick);
        if (canClick) _button.onClick.AddListener(OnClick);
    }

    IEnumerator Select()
    {
        float timer = 0f;
        var startPos = transform.position;
        transform.SetParent(GetComponentInParent<Canvas>().transform);
        while (timer < 1)
        {
            timer += Time.deltaTime*selectSpeed;
            transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(selectScale,selectScale,selectScale), timer);
            transform.position = Vector3.Lerp(startPos,  new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0), timer);
            yield return null;
        }
    }
    IEnumerator Deselect(bool scale = true)
    {
        if (transform.parent == owner.handContainer)
        {
            yield break;
        }
        float timer = 0f;
        var startPos = transform.localPosition;
        while (timer < 1)
        {
            timer += Time.deltaTime*selectSpeed;
            if(scale)transform.localScale = Vector3.Lerp(new Vector3(selectScale,selectScale,selectScale), new Vector3(1,1,1), timer);
            transform.localPosition = Vector3.Lerp(startPos, owner.handContainerLocation.localPosition, timer);
            yield return null;
        }
        _button.onClick.AddListener(OnClick);
        transform.SetParent(owner.handContainer);
    }
}
