using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEventUI : MonoBehaviour
{
    private static string ImgPath = "Image/Event/";

    [SerializeField]
    private Transform root;

    [SerializeField]
    private TMP_Text titleTxt;

    [SerializeField]
    private TMPPartialShake contentTxt;

    [SerializeField]
    private Image eventImg;

    [SerializeField]
    private GameObject choiceItemPrefab;

    [SerializeField]
    private Transform choiceItemParent;

    [SerializeField]
    private GameEventData tempData;

    private GameEventData eventData;



    private void Start()
    {
        // 测试
        Show(tempData);
    }

    public void Show(GameEventData eventData)
    {
        this.eventData = eventData;
        titleTxt.text = eventData.EventName;
        NextStory(eventData.StartNode);
        root.gameObject.SetActive(true);
    }

    public void Hide()
    {
        root.gameObject.SetActive(false);
    }

    public void NextStory(GameEventNode story)
    {
        if(story == null)
        {
            // 事件结束，关闭页面
            Hide();
            return;
        }

        contentTxt.SetShakingText(story.StoryText);
        eventImg.sprite = Resources.Load<Sprite>(ImgPath + story.ImgPath);
        //清除所有选项
        for (int i = choiceItemParent.childCount - 1; i >= 0; i--)
        {
            Destroy(choiceItemParent.GetChild(i).gameObject);
        }
        //添加选项
        foreach (var choice in story.Choices)
        {
            var choiceItem = Instantiate(choiceItemPrefab, choiceItemParent);
            choiceItem.GetComponent<EventChoiceItem>().Init(choice, NextStory);
            choiceItem.gameObject.SetActive(true);
        }
        // 触发进入节点事件
        story.EnterThisNode();
    }

    public void NextStory(int storyIndex)
    {
        NextStory(eventData.AllNodes.Find(node => node.NodeIndex == storyIndex));
    }
}
