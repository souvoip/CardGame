using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTopUI : MonoBehaviour
{
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
}
