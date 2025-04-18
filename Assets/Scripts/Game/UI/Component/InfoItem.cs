using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text titleTxt;

    [SerializeField]
    private Image iconImg;

    [SerializeField]
    private TMP_Text contentTxt;

    public void SetInfo(DetailInfo info)
    {
        titleTxt.text = info.Title;
        if(info.Icon != null)
        {
            iconImg.gameObject.SetActive(true);
            iconImg.sprite = info.Icon;
        }
        else
        {
            iconImg.gameObject.SetActive(false);
        }
        contentTxt.text = info.Description;
    }
}
