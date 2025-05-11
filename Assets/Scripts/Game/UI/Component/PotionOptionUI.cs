using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionOptionUI : MonoBehaviour
{
    [SerializeField]
    private Transform root;
    [SerializeField]
    private Button closeBtn;
    [SerializeField]
    private Button useBtn;
    [SerializeField]
    private Button dropBtn;
    [SerializeField]
    private ArrowEffectManager lineEffect;

    private PotionItem potionItem;

    private void Start()
    {
        closeBtn.onClick.AddListener(Hide);
        useBtn.onClick.AddListener(OnUse);
        dropBtn.onClick.AddListener(OnDrop);

        closeBtn.gameObject.SetActive(false);
        root.localScale = Vector3.zero;

        lineEffect.gameObject.SetActive(false);

        HoldCancel.cancelAction += CancelLineEffect;
    }

    private void Update()
    {
        if (lineEffect.gameObject.activeSelf)
        {
            lineEffect.SetColor(BattleManager.Instance.nowSelectEnemy != null);
        }
        if(Input.GetMouseButtonDown(1) && lineEffect.gameObject.activeSelf)
        {
            lineEffect.gameObject.SetActive(false);
            potionItem = null;
        }
        else if(Input.GetMouseButtonDown(0) && lineEffect.gameObject.activeSelf && BattleManager.Instance.nowSelectEnemy != null)
        {
            potionItem?.Use(BattleManager.Instance.nowSelectEnemy);
            lineEffect.gameObject.SetActive(false);
            potionItem = null;
        }
    }

    private void OnDestroy()
    {
        HoldCancel.cancelAction -= CancelLineEffect;
    }

    public void Show(PotionItem potion, Vector2 pos)
    {
        potionItem = potion;
        closeBtn.gameObject.SetActive(true);
        // 显示按钮文本
        useBtn.GetComponentInChildren<TMP_Text>().text = potion.potionItemData.PotionType == EPotionType.Drink ? "饮用" : "投掷";

        if (TurnManager.TurnType == ETurnType.Player || (TurnManager.TurnType == ETurnType.NonBattle && potion.potionItemData.isMapUse))
        {
            useBtn.interactable = true;
        }
        else
        {
            useBtn.interactable = false;
        }

        // 显示动画
        root.position = pos;
        root.localScale = Vector3.zero;
        root.DOScale(Vector3.one, 0.25f);
    }

    public void Hide()
    {
        closeBtn.gameObject.SetActive(false);
        root.DOScale(Vector3.zero, 0.25f);
    }

    private void OnUse()
    {
        if (potionItem != null)
        {
            if (potionItem.potionItemData.UseType == EUseType.NonDirectivity)
            {
                potionItem.Use();
                potionItem = null;
            }
            else if (potionItem.potionItemData.UseType == EUseType.Directivity)
            {
                // 选择目标箭头
                lineEffect.gameObject.SetActive(true);
                //设置塞贝尔曲线起始点
                lineEffect.SetStartPos(potionItem.transform.position);
            }
        }
        Hide();
    }

    private void OnDrop()
    {
        if (potionItem != null)
        {
            potionItem.Drop();
        }
        Hide();
    }

    private void CancelLineEffect()
    {
        lineEffect.gameObject.SetActive(false);
        potionItem = null;
    }
}
