using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyRole : CharacterBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private CardManager _cardManager;

    [SerializeField]
    private StateBar hpBar;

    private float maxHp = 100f;

    private float hp = 100f;

    protected override void Init()
    {
        hpBar.SetMaxHealth(maxHp);
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

    public override void GetHit(Damage damage)
    {
        Debug.Log(damage.GetDamage());
        ChangeHealth(-damage.GetDamage());
    }

    private void ChangeHealth(float health)
    {
        hp += health;
        hpBar.SetHealth(hp);
    }
}
