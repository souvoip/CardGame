using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyRole : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private CardManager _cardManager;

    [SerializeField]
    private StateBar hpBar;

    private float maxHp = 100f;

    private float hp = 100f;

    private void Awake()
    {
        hpBar.SetMaxHealth(maxHp);
        //hpBar.SetHealth(50f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
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

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void ChangeHealth(float health)
    {
        hp += health;
        hpBar.SetHealth(hp);
    }
}
