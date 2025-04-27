using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCardUI : MonoBehaviour
{
    [SerializeField]
    private GameObject viewCardPrefab;
    [SerializeField]
    private Transform viewCardContent;
    [SerializeField]
    private Button jumpBtn;

    private Action hideAction;

    private void Awake()
    {
        jumpBtn.onClick.AddListener(JumpCardSelect);
    }

    public void Show(int[] cardIds, Action hideAction = null)
    {
        gameObject.SetActive(true);
        // 清空
        foreach (Transform child in viewCardContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < cardIds.Length; i++)
        {
            GameObject card = Instantiate(viewCardPrefab, viewCardContent);
            var vci = card.GetComponent<ViewCardItem>();
            vci.InitData(CardDataManager.GetCard(cardIds[i]), GetCard);
        }
        this.hideAction = hideAction;
    }

    public void Hide()
    {
        hideAction?.Invoke();
        gameObject.SetActive(false);
    }

    private void JumpCardSelect()
    {
        Hide();
    }

    private void GetCard(ViewCardItem vci)
    {
        // 获取卡牌
        Debug.Log("获取卡牌：" + vci.cardData.Name);
        BattleManager.Instance.CardManager.AddBattleCard(vci.cardData);
        Hide();
    }
}
