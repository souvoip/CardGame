using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeUI : MonoBehaviour
{
    [SerializeField]
    private Transform root;
    [SerializeField]
    private GameObject prizeItemPrefab;
    [SerializeField]
    private Transform itemContent;
    [SerializeField]
    private Button closeBtn;

    private void Awake()
    {
        closeBtn.onClick.AddListener(Hide);
        Hide();
    }

    public void Show(List<PrizeItemData> prizeItems)
    {
        // 清理奖品
        for (int i = 0; i < itemContent.childCount; i++)
        {
            Destroy(itemContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < prizeItems.Count; i++)
        {
            GameObject item = Instantiate(prizeItemPrefab, itemContent);
            item.GetComponent<PrizeItem>().Init(prizeItems[i], OnClickPrize);
            item.gameObject.SetActive(true);
        }
        root.gameObject.SetActive(true);
    }

    public void Hide()
    {
        root.gameObject.SetActive(false);
    }

    private void OnClickPrize(PrizeItem item)
    {
        Destroy(item.gameObject);
        // 检查是否还有未领取的奖品
        TimerTools.Timer.FrameOnce(1, () =>
        {
            if(itemContent.childCount == 0)
            {
                // 以获取所有奖励，关闭页面
                Hide();
            }
        });
    }
}

