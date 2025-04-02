using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyRole : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private CardManager2 _cardManager;

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cardManager.SelectEnemy(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardManager.SelectEnemy(null);
    }
}
