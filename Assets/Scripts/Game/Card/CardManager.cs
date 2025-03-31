using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    /// <summary>  
    /// 卡牌起始位置  
    /// </summary>  
    public Vector3 rootPos = new Vector3(0, -33.5f, 20);
    /// <summary>  
    /// 卡牌对象  
    /// </summary>  
    public GameObject cardItem;
    /// <summary>  
    /// 扇形半径  
    /// </summary>  
    public float size = 30f;
    /// <summary>  
    /// 卡牌出现最大位置  
    /// </summary>  
    private float minPos = 1.415f;
    /// <summary>  
    /// 卡牌出现最小位置  
    /// </summary>  
    private float maxPos = 1.73f;
    /// <summary>  
    /// 手牌列表  
    /// </summary>  
    private List<CardItem> cardList;
    /// <summary>  
    /// 手牌位置  
    /// </summary>  
    private List<float> rotPos;
    /// <summary>  
    /// 最大手牌数量  
    /// </summary>  
    private int CardMaxCount = 8;

    void Start()
    {
        InitCard();
    }    /// <summary>  
         /// 数据初始化  
         /// </summary>  
    public void InitCard()
    {
        rotPos = InitRotPos(CardMaxCount);
    }
    /// <summary>  
    /// 初始化位置  
    /// </summary>  
    /// <param name="count"></param>    
    /// <param name="interval"></param>    
    /// <returns></returns>    
    public List<float> InitRotPos(int count)
    {
        List<float> rotPos = new List<float>();
        float interval = (maxPos - minPos) / count;
        for (int i = 0; i < count; i++)
        {
            float nowPos = maxPos - interval * i;
            rotPos.Add(nowPos);
        }
        return rotPos;
    }
    // Update is called once per frame  
    void Update()
    {
        TaskItemDetection();
        RefereshCard();
    }
    /// <summary>  
    /// 添加卡牌  
    /// </summary>  
    public void AddCard()
    {
        if (cardList == null)
        {
            cardList = new List<CardItem>();
        }
        if (cardList.Count >= CardMaxCount)
        {
            Debug.Log("手牌数量上限");
            return;
        }
        GameObject item = Instantiate(cardItem, this.transform);
        CardItem text = item.GetComponent<CardItem>();
        text.RefreshData(rootPos, 0, 0, 0);
        cardList.Add(text);
    }
    /// <summary>  
    /// 手牌状态刷新  
    /// </summary>  
    public void RefereshCard()
    {
        if (cardList == null)
        {
            return;
        }
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].RefreshData(rootPos, rotPos[i], size, i);
        }
    }
    /// <summary>  
    /// 销毁卡牌  
    /// </summary>  
    public void RemoveCard()
    {
        if (cardList == null)
        {
            return;
        }
        CardItem item = cardList[cardList.Count - 1];
        cardList.Remove(item);
        Destroy(item.gameObject);
    }
    /// <summary>  
    /// 销毁卡牌  
    /// </summary>  
    /// <param name="item"></param>    
    public void RemoveCard(CardItem item)
    {
        if (cardList == null)
        {
            return;
        }
        cardList.Remove(item);
        Destroy(item.gameObject);
    }
    private Vector3 oldmousePosition;

    /// <summary>  
    /// 玩家操作检测  
    /// </summary>  
    public void TaskItemDetection()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCard();
        }
    }
}

