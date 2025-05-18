using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTopUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameTxt;

    [SerializeField]
    private TMP_Text hpTxt;

    [SerializeField]
    private TMP_Text goldTxt;

    [SerializeField]
    private RemainsItemControl remainsItemManager;
    [SerializeField]
    private PotionItemControl potionItemManager;

    public void UpdatePlayerRemainsItemInfo()
    {
        for(int i = 0; i < BattleManager.Instance.Player.RoleData.Items.Count; i++)
        {
            if(BattleManager.Instance.Player.RoleData.Items[i].ItemType == EItemType.Remains)
            {
                remainsItemManager.AddRemainsItemIcon((RemainsItemData)BattleManager.Instance.Player.RoleData.Items[i]);
            }
        }
    }

    public void UpdatePlayerPotionItemInfo()
    {
        for (int i = 0; i < BattleManager.Instance.Player.RoleData.Items.Count; i++)
        {
            if (BattleManager.Instance.Player.RoleData.Items[i].ItemType == EItemType.Potion)
            {
                potionItemManager.AddPotion((PotionItemData)BattleManager.Instance.Player.RoleData.Items[i]);
            }
        }
    }

    public void SetNameTxt(string name)
    {
        nameTxt.text = name;
    }

    public void SetHpTxt(int hp, int maxHp)
    {
        hpTxt.text = hp + "/" + maxHp;
    }

    public void SetGoldTxt(int gold)
    {
        goldTxt.text = gold.ToString();
    }
}
