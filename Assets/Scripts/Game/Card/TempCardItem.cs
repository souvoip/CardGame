using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempCardItem : MonoBehaviour
{
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

    public void UpdateData(CardBase cardData)
    {
        nameTxt.text = cardData.Name;
        if (cardData.Fee >= 0)
        {
            feeTxt.transform.parent.gameObject.SetActive(true);
            feeTxt.text = cardData.Fee.ToString();
        }
        else
        {
            feeTxt.transform.parent.gameObject.SetActive(false);
        }
        cardImg.sprite = Resources.Load<Sprite>(ResourcesPaths.CardImgPath + cardData.ImagePath);
        typeTxt.text = cardData.GetCardTypeeString();
        descTxt.text = cardData.GetDesc();
    }

    public void UpdateDesc(CardBase cardData)
    {
        descTxt.text = cardData.GetDesc();
    }
}
