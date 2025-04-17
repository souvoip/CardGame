using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txt;

    public void SetInfo(string info)
    {
        txt.text = info;
    }
}
