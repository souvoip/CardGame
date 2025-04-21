using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewCardItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region component
    [SerializeField]
    private TMP_Text nameTxt;
    [SerializeField]
    private TMP_Text feeTxt;
    [SerializeField]
    private Image backgroundImg;
    [SerializeField]
    private Image cardImg;
    [SerializeField]
    private TMP_Text typeTxt;
    [SerializeField]
    private TMP_Text descTxt;
    #endregion

    [SerializeField]
    private Vector2 tempOffset;

    [SerializeField]
    private float scale = 1.2f;

    public CardBase cardData;

    public void UpdateData(CardBase cardData)
    {
        this.cardData = cardData;
        nameTxt.text = cardData.Name;
        if (cardData.Fee > 0)
        {
            feeTxt.transform.parent.gameObject.SetActive(true);
            feeTxt.text = cardData.Fee.ToString();
        }
        else
        {
            feeTxt.transform.parent.gameObject.SetActive(false);
        }
        cardImg.sprite = Resources.Load<Sprite>(CardItem.baseCardImgPath + cardData.ImagePath);
        typeTxt.text = cardData.GetCardTypeeString();
        descTxt.text = cardData.Desc;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.ShowInfos(transform.position, tempOffset, cardData.GetDetailInfos());
        transform.localScale = scale * Vector3.one;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.Hide();
        transform.localScale = Vector3.one;
    }
}
