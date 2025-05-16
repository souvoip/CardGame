using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventChoiceItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TMP_Text choiceTxt;

    private GameEventChoice choiceData;

    private Action<GameEventNode> clickAction;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Init(GameEventChoice data, Action<GameEventNode> action)
    {
        choiceData = data;
        choiceTxt.text = choiceData.ChoiceText;
        clickAction = action;
    }

    private void OnClick()
    {
        // 触发点击效果
        choiceData.SelectThisChoice();
        // 执行点击事件
        if(choiceData.NextNodes == null || choiceData.NextNodes.Count == 0)
        {
            clickAction(null);
            return;
        }
        int allRatio = 0;
        for(int i = 0; i < choiceData.NextNodes.Count; i++)
        {
            allRatio += choiceData.NextNodes[i].RandomRatio;
        }
        int randomValue = UnityEngine.Random.Range(0, allRatio);
        // 随机选择下一个节点
        int addRatio = 0;
        for(int i = 0; i < choiceData.NextNodes.Count; i++)
        {
            if(randomValue < choiceData.NextNodes[i].RandomRatio + addRatio)
            {
                clickAction(choiceData.NextNodes[i].NextNode);
                break;
            }
            addRatio += choiceData.NextNodes[i].RandomRatio;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.2f);
    }
}
