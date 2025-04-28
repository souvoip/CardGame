using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyRole : CharacterBase, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private StateBar hpBar;
    [SerializeField]
    private Transform aesistzTrans;
    [SerializeField]
    private EnemyIntention intention;
    [SerializeField]
    private Vector2 tempInfoOffset;
    [SerializeField]
    private Image enemyImg;
    [SerializeField]
    private Material dissolveMat;

    private float dieAnimTime = 1.2f;


    /// <summary>
    /// 敌人数据 (需要完善)
    /// </summary>
    private EnemyRoleData roleData;

    private EnemyDoAction currentRoundAction;

    protected override void Init()
    {
        AddEvents();
        enemyImg.material = Instantiate(dissolveMat);
    }

    public void SetEnemyData(int enemyID)
    {
        roleData = CharacterDataManager.GetEnemyRoleData(enemyID);

        hpBar.SetMaxHealth(roleData.MaxHP);
        roleData.InitActions(this);
    }

    private void AddEvents()
    {
        TurnManager.OnPlayerTurnStart += OnPlayerTurnStart;
        TurnManager.OnEnemyTurnStart += OnEnemyTurnStart;
        TurnManager.OnStartBattle += OnStartBattle;
        EventCenter.GetInstance().AddEventListener(EventNames.CHARACTER_BUFF_UPDATA, UpdateBuff);
    }

    private void RemoveEvents()
    {
        TurnManager.OnPlayerTurnStart -= OnPlayerTurnStart;
        TurnManager.OnEnemyTurnStart -= OnEnemyTurnStart;
        TurnManager.OnStartBattle -= OnStartBattle;
        EventCenter.GetInstance().RemoveEventListener(EventNames.CHARACTER_BUFF_UPDATA, UpdateBuff);
    }



    private void OnStartBattle()
    {
        // 添加战斗 Buff
        for (int i = 0; i < roleData.FixedBattleBuffs.Count; i++)
        {
            AddBuff(BuffDataManager.GetBuff(roleData.FixedBattleBuffs[i].BuffID), roleData.FixedBattleBuffs[i].Stacks);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isDie) { return; }
        BattleManager.Instance.CardManager.SelectEnemy(this);

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        var infos = buffControl.GetBuffInfos();
        if(currentRoundAction != null)
        {
            infos.Insert(0, currentRoundAction.GetActionInfo().detailInfo);
        }
        UIManager.Instance.holdDetailUI.ShowInfos(pos, tempInfoOffset, infos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDie) { return; }
        BattleManager.Instance.CardManager.SelectEnemy(null);

        UIManager.Instance.holdDetailUI.Hide();
    }

    public override void ChangeAttribute(ERoleAttribute attribute, int value)
    {
        switch (attribute)
        {
            case ERoleAttribute.HP:
                Debug.Log("受到 ：" + value + " 伤害");
                ChangeHealth(value);
                if(roleData.HP <= 0)
                {
                    Die();
                }
                break;
            case ERoleAttribute.Aesist:
                ChangeAesist(value);
                break;
        }
    }

    public override void Die()
    {
        Debug.Log($"敌人{roleData.Name}死亡");
        if (isDie) { return; }
        isDie = true;
        base.Die();
        // 移除相关事件
        RemoveEvents();
        // 播放死亡动画
        StartCoroutine(DieAnimCoroutine());
        //// 移除敌人
        //BattleManager.Instance.EnemyDie(this);
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

    private void OnPlayerTurnStart()
    {
        currentRoundAction = roleData.GetRandomAction();
        intention.ShowIntention(currentRoundAction.GetActionInfo());
    }

    private void OnEnemyTurnStart()
    {
        // 清空抵抗 TODO: 需要实现 Buff 对抵抗的影响
        ChangeAesist(int.MinValue);
    }

    private void UpdateBuff()
    {
        if(currentRoundAction != null)
        {
            intention.ShowIntention(currentRoundAction.GetActionInfo());
        }
    }

    public void DoAction()
    {
        Debug.Log($"敌人{roleData.Name}行动, 行动类型：{currentRoundAction.ActionType}");
        currentRoundAction.DoAction();
    }

    private IEnumerator DieAnimCoroutine()
    {
        Material material = enemyImg.material;
        float dieTime = dieAnimTime;
        while(dieTime > 0)
        {
            material.SetFloat("_Progress", dieTime / dieAnimTime);
            dieTime -= Time.deltaTime;
            yield return null;
        }
        // 移除敌人
        BattleManager.Instance.EnemyDie(this);
    }
}
