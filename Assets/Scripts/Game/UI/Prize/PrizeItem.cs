using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizeItem : MonoBehaviour
{
    [SerializeField]
    private Image prizeImg;
    [SerializeField]
    private TMP_Text desText;

    private PrizeItemData prizeData;

    private Action<PrizeItem> onClick;

    public void Init(PrizeItemData prizeData, Action<PrizeItem> onClick)
    {
        this.prizeData = prizeData;
        this.onClick = onClick;
        switch (prizeData.prizeTypel)
        {
            case EPrizeType.Gold:
                desText.text = "获得" + prizeData.Value + "枚金币";
                break;
            case EPrizeType.Card:
                desText.text = "选择一张卡牌";
                break;
            case EPrizeType.Item:
                desText.text = "获得 " + ItemDataManager.GetItemName(prizeData.Value);
                break;
        }
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // 获得对应奖励
        switch (prizeData.prizeTypel)
        {
            case EPrizeType.Gold:
                BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.Gold, prizeData.Value);
                onClick?.Invoke(this);
                break;
            case EPrizeType.Card:
                // 显示选择卡牌UI界面
                UIManager.Instance.selectCardUI.Show(CardDataManager.GetRandomCardIds(prizeData.Value), (isSelect) =>
                {
                    if (isSelect)
                    {
                        onClick?.Invoke(this);
                    }
                });
                //BattleManager.Instance.Player.AddCard(prizeData.Value);
                break;
            case EPrizeType.Item:
                BattleManager.Instance.Player.AddItem(prizeData.Value);
                onClick?.Invoke(this);
                break;
        }
    }
}


public class PrizeItemData
{
    public EPrizeType prizeTypel;

    /// <summary>
    /// 奖励数量,或者是随机卡牌数量，获取的道具ID
    /// </summary>
    public int Value;

    public PrizeItemData(EPrizeType type, int v)
    {
        prizeTypel = type;
        Value = v;
    }
}


public enum EPrizeType
{
    Gold,  // 金币
    Card,  // 卡牌
    Item, // 道具
}