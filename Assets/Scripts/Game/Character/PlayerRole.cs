using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRole : CharacterBase
{
    [SerializeField]
    private StateBar hpBar;
    [SerializeField]
    private Transform aesist;
    [SerializeField]
    private PlayerRoleData roleData;

    public PlayerRoleData RoleData { get { return roleData; } }

    protected override void Init()
    {
        hpBar.SetMaxHealth(roleData.MaxHP);
        TurnManager.OnPlayerTurnStart += OnPlayerTurnStart;
    }



    public override void ChangeAttribute(ERoleAttribute attribute, int value)
    {
        switch (attribute)
        {
            case ERoleAttribute.HP:
                Debug.Log("受到 ：" + value + " 伤害");
                ChangeHealth(value);
                break;
            case ERoleAttribute.AP:
                ChangeAP(value);
                break;
            case ERoleAttribute.Aesist:
                ChangeAesist(value);
                break;
        }
    }

    private void ChangeAP(int value)
    {
        roleData.AP += value;
        // TODO: UI 更新
        EventCenter<int, int>.GetInstance().EventTrigger(EventNames.CHANGE_AP, roleData.AP, roleData.MaxAP);
    }

    private void SetAP(int value)
    {
        roleData.AP = value;
        // TODO: UI 更新
        EventCenter<int, int>.GetInstance().EventTrigger(EventNames.CHANGE_AP, roleData.AP, roleData.MaxAP);
    }

    private void ChangeHealth(int value)
    {
        // 抵抗伤害
        value = ChangeAesist(value);

        roleData.HP += value;
        hpBar.SetHealth(roleData.HP);
    }

    private int ChangeAesist(int value)
    {
        roleData.Aesist += value;
        if (roleData.Aesist < 0)
        {
            int temp = roleData.Aesist;
            roleData.Aesist = 0;
            aesist.gameObject.SetActive(false);
            return temp;
        }
        aesist.gameObject.SetActive(true);
        aesist.GetComponentInChildren<TMP_Text>().text = roleData.Aesist.ToString();
        return 0;
    }

    /// <summary>
    /// 玩家回合开始
    /// </summary>
    private void OnPlayerTurnStart()
    {
        // 恢复行动力 TODO: 需要实现 Buff 对行动力的影响
        SetAP(roleData.MaxAP);

        // 清空抵抗 TODO: 需要实现 Buff 对抵抗的影响
        ChangeAesist(int.MinValue);
    }
}
