using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class PlayerRole : CharacterBase, IPointerEnterHandler, IPointerExitHandler, ISaveLoad
{
    [SerializeField]
    private StateBar hpBar;
    [SerializeField]
    private Transform aesistzTrans;
    [SerializeField]
    private Image playerImg;
    [SerializeField]
    private Material dissolveMat;

    private float dieAnimTime = 1.2f;

    private PlayerRoleData roleData;

    public PlayerRoleData RoleData { get { return roleData; } }

    public override bool IsPlayer => true;

    protected override void Init()
    {
        // 获取玩家数据
        roleData = CharacterDataManager.GetPlayerRoleData(1);

        hpBar.SetMaxHealth(roleData.MaxHP);
        AddEvents();
        playerImg.material = dissolveMat;
        playerImg.sprite = Resources.Load<Sprite>(ResourcesPaths.RoleImgPath + roleData.RoleImgPath);

        // 添加固定道具
        for (int i = 0; i < roleData.FixedItemIDs.Count; i++)
        {
            AddItem(roleData.FixedItemIDs[i]);
        }

        UIManager.Instance.gameTopUI.SetHpTxt(roleData.HP, roleData.MaxHP);
        UIManager.Instance.gameTopUI.SetGoldTxt(roleData.Gold);
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
            case ERoleAttribute.MaxHP:
                roleData.MaxHP += value;
                roleData.HP += value;
                hpBar.SetMaxHealth(roleData.MaxHP);
                hpBar.SetHealth(roleData.HP);
                UIManager.Instance.gameTopUI.SetHpTxt(roleData.HP, roleData.MaxHP);
                break;
            case ERoleAttribute.HP:
                ChangeHealth(value);
                UIManager.Instance.gameTopUI.SetHpTxt(roleData.HP, roleData.MaxHP);
                break;
            case ERoleAttribute.AP:
                ChangeAP(value);
                break;
            case ERoleAttribute.Aesist:
                ChangeAesist(value);
                break;
            case ERoleAttribute.Gold:
                roleData.Gold += value;
                UIManager.Instance.gameTopUI.SetGoldTxt(roleData.Gold);
                break;
        }
    }

    public override int GetAttributeValue(ERoleAttribute attribute)
    {
        switch (attribute)
        {
            case ERoleAttribute.MaxHP:
                return roleData.MaxHP;
            case ERoleAttribute.HP:
                return roleData.HP;
            case ERoleAttribute.MaxMP:
                return roleData.MaxMP;
            case ERoleAttribute.MP:
                return roleData.MP;
            case ERoleAttribute.MaxAP:
                return roleData.MaxAP;
            case ERoleAttribute.AP:
                return roleData.AP;
            case ERoleAttribute.Aesist:
                return roleData.Aesist;
            case ERoleAttribute.Shield:
                return roleData.Shield;
            case ERoleAttribute.Gold:
                return roleData.Gold;
            case ERoleAttribute.DrawCardCount:
                return roleData.DrawCardCount;
            case ERoleAttribute.MaxCardCount:
                return roleData.MaxCardCount;
            default:
                return 0;
        }
    }

    private void OnStartBattle()
    {
        // 添加战斗 Buff
        for (int i = 0; i < roleData.FixedBattleBuffs.Count; i++)
        {
            AddBuff(BuffDataManager.GetBuff(roleData.FixedBattleBuffs[i].BuffID), roleData.FixedBattleBuffs[i].Stacks);
        }
        // 触发道具效果
        for (int i = 0; i < roleData.Items.Count; i++)
        {
            if (roleData.Items[i].ItemType == EItemType.Remains)
            {
                ((RemainsItemData)roleData.Items[i]).OnBattleStart();
            }
        }
    }

    public override void Die()
    {
        base.Die();
        // TODO: 判断是否有复活
        if (roleData.HP >= 0)
        {
            return;
        }
        // TODO: 游戏结束
        RemoveEvents();
        StartCoroutine(DieAnimCoroutine());
    }

    private IEnumerator DieAnimCoroutine()
    {
        Material material = playerImg.material;
        float dieTime = dieAnimTime;
        while (dieTime > 0)
        {
            material.SetFloat("_Progress", dieTime / dieAnimTime);
            dieTime -= Time.deltaTime;
            yield return null;
        }
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
        if (value < 0)
        {
            // 抵抗伤害
            value = ChangeAesist(value);
        }

        roleData.HP = Mathf.Min(roleData.MaxHP, roleData.HP + value);
        hpBar.SetHealth(roleData.HP);
        // 显示伤害数字
        BattleManager.Instance.ShowDamageNumber(value, transform.position);
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

    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="itemID"> 需要添加的道具ID </param>
    /// <returns> 是否添加成功 </returns>
    public bool AddItem(int itemID)
    {
        ItemDataBase temp;
        return AddItem(itemID, out temp);
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="itemID"> 需要添加的道具ID </param>
    /// <returns> 是否添加成功 </returns>
    public bool AddItem(int itemID, out ItemDataBase outItem)
    {
        var item = ItemDataManager.GetItem(itemID);

        if (item == null)
        {
            Debug.LogError("未找到对应物品，物品ID: " + itemID);
            outItem = null;
            return false;
        }
        // 判断是否已经拥有该物品
        if (roleData.Items.Exists(x => x.ID == itemID))
        {
            // 判断是否可以重复获取
            if(roleData.Items.Find(x => x.ID == itemID).IsUnique)
            {
                Debug.LogError("该物品不可重复获取，物品ID: " + itemID);
                outItem = null;
                return false;
            }
        }

        roleData.Items.Add(item);
        if (item.ItemType == EItemType.Remains)
        {
            ((RemainsItemData)item).OnAcquire();
        }
        outItem = item;
        UIManager.Instance.gameTopUI.AddItemInfo(item);
        return true;
    }

    public void AddRandomItem(List<int> itemIDs, int count)
    {
        // TODO: 随机获取物品, 需要保证物品的唯一性，并且当数量足够时，要满足添加的需求
        for (int i = 0; i < count; i++)
        {
            AddItem(itemIDs[UnityEngine.Random.Range(0, itemIDs.Count)]);
        }
    }

    public JSONObject Save()
    {
        return roleData.Save();
    }

    public void Load(JSONObject data)
    {
        roleData.Load(data);
        // 刷新UI
        hpBar.SetHealth(roleData.HP);
        hpBar.SetMaxHealth(roleData.MaxHP);

        Debug.Log(roleData.Items.Count);

        UIManager.Instance.gameTopUI.UpdatePlayerPotionItemInfo();
        UIManager.Instance.gameTopUI.UpdatePlayerRemainsItemInfo();

        UIManager.Instance.gameTopUI.SetHpTxt(roleData.HP, roleData.MaxHP);
        UIManager.Instance.gameTopUI.SetGoldTxt(roleData.Gold);
    }

    public void RemoveItem(PotionItemData potionItemData)
    {
        roleData.Items.Remove(potionItemData);
    }
}
