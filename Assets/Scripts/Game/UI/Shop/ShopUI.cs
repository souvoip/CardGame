using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private Transform root;

    [SerializeField]
    private List<Transform> cardSlots;

    [SerializeField]
    private List<Transform> remainsSlots;

    [SerializeField]
    private List<Transform> potionSlots;

    [SerializeField]
    private Button removeCardBtn;

    [SerializeField]
    private Button exitBtn;

    [SerializeField]
    private GameObject shopCardPrefab;

    [SerializeField]
    private GameObject shopItemPrefab;


    /// <summary>
    /// 商品打折率
    /// </summary>
    [SerializeField]
    private float discountRate = 0.1f;

    [SerializeField]
    private Vector2Int priceOffset = new Vector2Int(-10, 10);
    /// <summary>
    /// 可以移除卡牌数量
    /// </summary>
    private int removeCardCount = 1;

    private int removeCardPrice = 50;

    public float animationDuration = 0.5f; // 动画持续时间
    public Ease easeType = Ease.OutBack;    // 缓动效果

    private RectTransform rectTransform;
    private Vector2 originalPosition;       // 弹窗的原始位置（屏幕中心）

    private List<IBuyItem> shopItems = new List<IBuyItem>();

    private void Start()
    {
        rectTransform = root.GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        // 初始化时将弹窗移到屏幕下方
        rectTransform.anchoredPosition = new Vector2(0, -GetOffscreenY());
        removeCardBtn.onClick.AddListener(RemoveCard);
        exitBtn.onClick.AddListener(Hide);
    }

    public void Show()
    {
        ShowAnim();
        shopItems.Clear();
        // 重置移除卡牌数量
        removeCardCount = 1;
        // 随机商品
        // 卡牌
        for (int i = 0; i < cardSlots.Count; i++)
        {
            // 清理物品
            if (cardSlots[i].childCount > 0) { Destroy(cardSlots[i].GetChild(0).gameObject); }
            // 创建新物品
            GameObject item = Instantiate(shopCardPrefab, cardSlots[i]);
            item.GetComponent<ShopCardItem>().InitData(CardDataManager.GetRandomShopCard(), BuyItem, Random.Range(0f, 1f) < discountRate, Random.Range(priceOffset.x, priceOffset.y));
            shopItems.Add(item.GetComponent<ShopCardItem>());
        }

        // 物品
        for (int i = 0; i < remainsSlots.Count; i++)
        {
            // 清理物品
            if (remainsSlots[i].childCount > 0) { Destroy(remainsSlots[i].GetChild(0).gameObject); }
            // 创建新物品
            GameObject item = Instantiate(shopItemPrefab, remainsSlots[i]);
            item.GetComponent<ShopItem>().InitData(ItemDataManager.GetRandomShopRemainsItem(), BuyItem, Random.Range(0f, 1f) < discountRate, Random.Range(priceOffset.x, priceOffset.y));
            shopItems.Add(item.GetComponent<ShopItem>());
        }
        // 药水
        for (int i = 0; i < potionSlots.Count; i++)
        {
            // 清理物品
            if (potionSlots[i].childCount > 0) { Destroy(potionSlots[i].GetChild(0).gameObject); }
            // 创建新物品
            GameObject item = Instantiate(shopItemPrefab, potionSlots[i]);
            item.GetComponent<ShopItem>().InitData(ItemDataManager.GetRandomShopPotionItem(), BuyItem, Random.Range(0f, 1f) < discountRate, Random.Range(priceOffset.x, priceOffset.y));
            shopItems.Add(item.GetComponent<ShopItem>());
        }
    }

    public void Hide()
    {
        HideAnim();
    }

    public void RemoveCard()
    {
        if (removeCardCount > 0 && BattleManager.Instance.Player.RoleData.Gold >= removeCardPrice)
        {
            removeCardCount--;
            UIManager.Instance.cardView.Show(BattleManager.Instance.CardManager.PlayerAllCards, true, ECardViewOpenMode.Remove);
        }
    }

    // 显示弹窗（从下方移动到中心）
    public void ShowAnim()
    {
        gameObject.SetActive(true);
        rectTransform.DOAnchorPos(originalPosition, animationDuration)
            .SetEase(easeType);
    }

    // 隐藏弹窗（从中心移动到下方）
    public void HideAnim()
    {
        rectTransform.DOAnchorPos(new Vector2(0, -GetOffscreenY()), animationDuration)
            .SetEase(easeType)
            .OnComplete(() => gameObject.SetActive(false));
    }

    // 计算屏幕下方的Y坐标
    private float GetOffscreenY()
    {
        // 弹窗高度的一半 + 屏幕高度的一半，确保完全移出屏幕
        return rectTransform.rect.height / 2 + Screen.height / 2;
    }

    private void BuyItem(IBuyItem item)
    {
        if (item.CanBuy())
        {
            item.Buy();
            // 更新显示
            for (int i = 0; i < shopItems.Count; i++)
            {
                shopItems[i].UpdatePrice();
            }
        }
    }

}
