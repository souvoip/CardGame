using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static DetailInfo DetailInfo;

    private PotionItem potionItem;

    public bool isEmpty = true;

    [SerializeField]
    private Vector2 tempInfoOffset;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private static DetailInfo GetDefaultDetailInfo()
    {
        if(DetailInfo == null)
        {
            DetailInfo = new DetailInfo();
            DetailInfo.Title = "药水槽";
            DetailInfo.Description = "使用药水使自身获得各种增益效果或对敌方施加负面效果";
        }
        return DetailInfo;
    }

    public void AddPotion(PotionItemData potion, GameObject potionPrefab)
    {
        potionItem = Instantiate(potionPrefab, transform).GetComponent<PotionItem>();
        potionItem.Init(potion, ClearPotion);
        isEmpty = false;
    }

    public void RemovePotion()
    {
        if (isEmpty) { return; }
        Destroy(potionItem.gameObject);
        isEmpty = true;
        potionItem = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 显示药水信息，如果为空显示默认信息
        UIManager.Instance.holdDetailUI.ShowInfos(transform.position, tempInfoOffset, isEmpty ? GetDefaultDetailInfo() : potionItem.potionItemData.GetDetailInfo());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.Hide();
    }

    private void OnClick()
    {
        if (isEmpty || potionItem.potionItemData.UseType == EUseType.CannotUse) { return; }
        UIManager.Instance.potionOptionUI.Show(potionItem, transform.position + Vector3.up * -80);
    }

    private void ClearPotion()
    {
        isEmpty = true;
    }

    
}
