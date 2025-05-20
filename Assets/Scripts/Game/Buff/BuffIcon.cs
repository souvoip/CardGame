using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private TMP_Text stacksTxt;

    private int buffId;

    public int BuffId { get { return buffId; } }

    public void UpdateIcon(int bid, string path, int stacks)
    {
        buffId = bid;
        iconImage.sprite = Resources.Load<Sprite>(ResourcesPaths.BuffImgPath + path);
        stacksTxt.text = stacks.ToString();
    }

    public void UpdateStacks(int stacks)
    {
        stacksTxt.text = stacks.ToString();
    }
}
