using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardViewUI : MonoBehaviour
{
    [SerializeField]
    private GameObject viewCardPrefab;
    [SerializeField]
    private Transform viewCardContent;
    [SerializeField]
    private Button backBtn;

    private List<ViewCardItem> items = new List<ViewCardItem>();

    private void Awake()
    {
        backBtn.onClick.AddListener(Hide);
    }

    /// <summary>
    /// 显示牌库
    /// </summary>
    /// <param name="cards">卡牌库</param>
    /// <param name="isOrder">是否按照卡牌库顺序显示</param>
    public void Show(List<CardBase> cards, bool isOrder = false, ECardViewOpenMode mode = ECardViewOpenMode.Normal)
    {
        Action<ViewCardItem> cardAct = null;

        switch (mode)
        {
            case ECardViewOpenMode.Normal:
                break;
            case ECardViewOpenMode.Upgrade:
                cardAct = UpgradeCardAct;
                break;
            case ECardViewOpenMode.Remove:
                cardAct = RemoveCardAct;
                break;
        }

        gameObject.SetActive(true);
        for (int i = 0; i < cards.Count; i++)
        {
            if (items.Count <= i)
            {
                GameObject go = Instantiate(viewCardPrefab, viewCardContent);
                ViewCardItem item = go.GetComponent<ViewCardItem>();
                item.InitData(cards[i], cardAct);
                items.Add(item);
            }
            else
            {
                items[i].InitData(cards[i]);
                items[i].gameObject.SetActive(true);
            }
        }
        for (int i = cards.Count; i < items.Count; i++)
        {
            items[i].gameObject.SetActive(false);
        }
        if (!isOrder)
        {
            // 根据牌的ID显示，不改变牌库顺序
            items.Sort((a, b) => a.cardData.ID.CompareTo(b.cardData.ID));
            for (int i = 0; i < items.Count; i++)
            {
                items[i].transform.SetSiblingIndex(i);
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        UIManager.Instance.holdDetailUI.Hide();
    }

    private void UpgradeCardAct(ViewCardItem item)
    {
        item.cardData.UpgradeCard();
        Hide();
    }

    private void RemoveCardAct(ViewCardItem item)
    {
        BattleManager.Instance.CardManager.RemoveBattleCard(item.cardData);
        // 关闭界面
        Hide();
    }

}

public enum ECardViewOpenMode
{
    Normal,     // 普通浏览
    Upgrade,    // 升级卡牌
    Remove,     // 移除卡牌
}
