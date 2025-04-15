using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyRole : CharacterBase, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private StateBar hpBar;

    /// <summary>
    /// 敌人数据 (需要完善)
    /// </summary>
    private EnemyRoleData roleData = new EnemyRoleData() {MaxHP = 100, HP = 100 };

    protected override void Init()
    {
        hpBar.SetMaxHealth(roleData.MaxHP);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BattleManager.Instance.CardManager.SelectEnemy(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleManager.Instance.CardManager.SelectEnemy(null);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void ChangeAttribute(ERoleAttribute attribute, int value)
    {
        switch (attribute)
        {
            case ERoleAttribute.HP:
                Debug.Log("受到 ：" + value + " 伤害");
                ChangeHealth(value);
                break;
        }
    }

    private void ChangeHealth(int health)
    {
        roleData.HP += health;
        hpBar.SetHealth(roleData.HP);
    }
}
