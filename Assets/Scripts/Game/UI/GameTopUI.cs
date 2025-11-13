using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTopUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameTxt;

    [SerializeField]
    private TMP_Text hpTxt;

    [SerializeField]
    private TMP_Text goldTxt;

    [SerializeField] private Button backMainMenuBtn;
    [SerializeField] private Button cardViewBtn;

    [SerializeField]
    private RemainsItemControl remainsItemManager;
    [SerializeField]
    private PotionItemControl potionItemManager;

    private void Awake()
    {
        backMainMenuBtn.onClick.AddListener(() => { GameManager.Instance.ReturnMainMenu(); });
        cardViewBtn.onClick.AddListener(() => { UIManager.Instance.cardView.Show(BattleManager.Instance.CardManager.PlayerAllCards, true, ECardViewOpenMode.Normal); });
    }

    public void UpdatePlayerRemainsItemInfo()
    {
        remainsItemManager.Clear();
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
        potionItemManager.Clear();
        for (int i = 0; i < BattleManager.Instance.Player.RoleData.Items.Count; i++)
        {
            if (BattleManager.Instance.Player.RoleData.Items[i].ItemType == EItemType.Potion)
            {
                potionItemManager.AddPotion((PotionItemData)BattleManager.Instance.Player.RoleData.Items[i]);
            }
        }
    }

    public void AddItemInfo(ItemDataBase item)
    {
        if(item.ItemType == EItemType.Remains)
        {
            remainsItemManager.AddRemainsItemIcon((RemainsItemData)item);
        }
        else if(item.ItemType == EItemType.Potion)
        {
            potionItemManager.AddPotion((PotionItemData)item);
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
