using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRole : CharacterBase, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private StateBar hpBar;
    [SerializeField]
    private Transform aesistzTrans;


    private PlayerRoleData roleData;

    public PlayerRoleData RoleData { get { return roleData; } }

    protected override void Init()
    {
        // TODO: 获取玩家数据
        roleData = CharacterDataManager.GetPlayerRoleData(1);

        hpBar.SetMaxHealth(roleData.MaxHP);
        AddEvents();
    }

    private void AddEvents()
    {
        TurnManager.OnPlayerTurnStart += OnPlayerTurnStart;
        TurnManager.OnStartBattle += OnStartBattle;
    }

    private void RemoveEvents()
    {
        TurnManager.OnPlayerTurnStart -= OnPlayerTurnStart;
        TurnManager.OnStartBattle -= OnStartBattle;
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

    private void OnStartBattle()
    {
        // 添加战斗 Buff
        for (int i = 0; i < roleData.FixedBattleBuffs.Count; i++)
        {
            AddBuff(BuffDataManager.GetBuff(roleData.FixedBattleBuffs[i].BuffID), roleData.FixedBattleBuffs[i].Stacks);
        }
    }


    public override void Die()
    {
        base.Die();
        // TODO: 游戏结束
        RemoveEvents();
        BattleManager.Instance.PlayerDie();
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
            aesistzTrans.gameObject.SetActive(false);
            return temp;
        }
        aesistzTrans.gameObject.SetActive(true);
        aesistzTrans.GetComponentInChildren<TMP_Text>().text = roleData.Aesist.ToString();
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

    [SerializeField]
    private Vector2 tempInfoOffset;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        UIManager.Instance.holdDetailUI.ShowInfos(pos, tempInfoOffset, buffControl.GetBuffInfos());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.Hide();
    }

    public void ClearBattleBuff()
    {
        buffControl.ClearBuff();
    }
}
