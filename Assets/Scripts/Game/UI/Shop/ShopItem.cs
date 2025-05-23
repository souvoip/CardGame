using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ShopItem : MonoBehaviour, IBuyItem, IPointerEnterHandler, IPointerExitHandler
{
    public ItemDataBase itemData;
    [SerializeField]
    private Image iconImg;
    [SerializeField]
    private TMP_Text priceTxt;
    [SerializeField]
    private Vector2 tempInfoOffset;

    /// <summary>
    /// 商品是否打折
    /// </summary>
    private bool isDiscount = false;
    /// <summary>
    /// 最终价格
    /// </summary>
    private int finalPrice = 0;

    private Action<IBuyItem> buyAction;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void InitData(ItemDataBase item, Action<IBuyItem> onClick = null, bool isDiscount = false, int priceOffset = 0)
    {
        itemData = item;
        switch (item.ItemType)
        {
            case EItemType.Remains:
                iconImg.sprite = Resources.Load<Sprite>(ResourcesPaths.RemainsImgPath + item.IconPath);
                break;
            case EItemType.Potion:
                iconImg.sprite = Resources.Load<Sprite>(ResourcesPaths.PotionImgPath + item.IconPath);
                break;
        }
        buyAction = onClick;

        this.isDiscount = isDiscount;
        finalPrice = (int)((itemData.Price + priceOffset) * (isDiscount ? 0.5f : 1f));
        UpdatePrice();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.ShowInfos(transform.position, tempInfoOffset, itemData.GetDetailInfo());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.Hide();
    }

    public void Buy()
    {
        BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.Gold, -finalPrice);
        BattleManager.Instance.Player.AddItem(itemData.ID);
        // 移除商品
        Destroy(gameObject);
    }

    public bool CanBuy()
    {
        if (BattleManager.Instance.Player.RoleData.Gold >= finalPrice)
        {
            return true;
        }
        return false;
    }

    public void UpdatePrice()
    {
        // 显示价格
        if (CanBuy() && isDiscount)
        {
            priceTxt.color = Color.green;
        }
        else if (!CanBuy())
        {
            priceTxt.color = Color.red;
        }
        else
        {
            priceTxt.color = Color.white;
        }
        priceTxt.text = finalPrice.ToString();
    }

    private void OnClick()
    {
        buyAction?.Invoke(this);
    }
}
