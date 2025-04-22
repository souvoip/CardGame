using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntention : MonoBehaviour
{
    [SerializeField]
    private Image intentImg;
    [SerializeField]
    private TMP_Text intentText;

    public void ShowIntention(ActionInfo info)
    {
        intentImg.sprite = info.icon;
        intentText.text = info.text;
    }
}
