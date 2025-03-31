using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Transform cardBase;

    private bool isDragging = false;

    private Vector3 originPos;

    private UnityAction<CardController> SelectAct;

    private UnityAction<CardController> UnSelectAct;

    private void Awake()
    {
        cardBase = transform.Find("Base");
        // 测试
        cardBase.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }

    public void SetAction(UnityAction<CardController> sa, UnityAction<CardController> usa)
    {
        SelectAct = sa;
        UnSelectAct = usa;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        originPos = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        // Reset the card's position to its original position
        transform.position = originPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (isDragging)
        //{
        //    // Move the card to the mouse position
        //    transform.position = eventData.position;
        //}
    }

    /// <summary>
    /// 选中卡片
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectCard();
    }
    
    /// <summary>
    /// 离开卡片
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        UnSelectCard();
    }

    private void SelectCard()
    {
        cardBase.DOScale(1.2f, 0.3f);
        cardBase.DOLocalMoveY(760f, 0.3f);
        SelectAct?.Invoke(this);
    }

    private void UnSelectCard()
    {
        cardBase.DOScale(1f, 0.3f);
        cardBase.DOLocalMoveY(720f, 0.3f);
        UnSelectAct?.Invoke(this);
    }

}
