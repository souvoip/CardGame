using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTopUI : MonoBehaviour
{
    [SerializeField]
    private RemainsItemControl remainsItemManager;




    public void UpdatePlayerRemainsItemInfo()
    {
        for(int i = 0; i < BattleManager.Instance.Player.RoleData.RemainsItems.Count; i++)
        {
            remainsItemManager.AddRemainsItemIcon(BattleManager.Instance.Player.RoleData.RemainsItems[i]);
        }
    }


}
