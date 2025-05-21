using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestUI : MonoBehaviour
{
    [SerializeField]
    private Transform root;

    [SerializeField]
    private Button restBtn;

    [SerializeField]
    private Button upgradeBtn;

    private void Awake()
    {
        restBtn.onClick.AddListener(OnClickRest);
        upgradeBtn.onClick.AddListener(OnClickUpgrade);
        Hide();
    }

    public void Show()
    {
        root.gameObject.SetActive(true);
    }

    public void Hide()
    {
        root.gameObject.SetActive(false);
    }

    /// <summary>
    /// 点击休息 回复30%最大生命值
    /// </summary>
    private void OnClickRest()
    {
        BattleManager.Instance.Player.ChangeAttribute(ERoleAttribute.HP,(int)(BattleManager.Instance.Player.GetAttributeValue(ERoleAttribute.MaxHP) * 0.3f));
        Hide();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnClickUpgrade()
    {
        Hide();
    }
}
