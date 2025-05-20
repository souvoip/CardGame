using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopCardItem : ViewCardItem, IBuyItem
{
    /// <summary>
    /// 价格文本
    /// </summary>
    [SerializeField]
    private TMP_Text priceTxt;

    /// <summary>
    /// 商品是否打折
    /// </summary>
    private bool isDiscount = false;
    /// <summary>
    /// 最终价格
    /// </summary>
    private int finalPrice = 0;

    private Action<IBuyItem> buyAction;

    public void InitData(CardBase cardData, Action<IBuyItem> onClick = null, bool isDiscount = false, int priceOffset = 0)
    {
        base.InitData(cardData, null);
        buyAction = onClick;
        this.isDiscount = isDiscount;
        finalPrice = (int)((cardData.Price + priceOffset) * (isDiscount ? 0.5f : 1f));
        UpdatePrice();
    }

    public void UpdatePrice()
    {
        // 显示价格
        if (CanBuy() && isDiscount)
        {
            priceTxt.color = Color.green;
        }
        else if(!CanBuy())
        {
            priceTxt.color = Color.red;
        }
        else
        {
            priceTxt.color = Color.white;
        }
        priceTxt.text = finalPrice.ToString();
    }

    public void Buy()
    {
        BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.Gold, -finalPrice);
        BattleManager.Instance.CardManager.AddBattleCard(cardData);
        // 移除商品
        Destroy(gameObject);
    }

    public bool CanBuy()
    {
        if(BattleManager.Instance.Player.RoleData.Gold >= finalPrice)
        {
            return true;
        }
        return false;
    }

    protected override void OnClick()
    {
        buyAction?.Invoke(this);
    }
}
