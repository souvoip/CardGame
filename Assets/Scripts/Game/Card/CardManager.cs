using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    /// 偶数手牌位置列表
    /// </summary>
    private List<float> rotPos_EvenNumber;
    /// <summary>
    /// 奇数手牌位置列表
    /// </summary>
    private List<float> rotPos_OddNumber;

    /// <summary>
    /// 最大手牌数量
    /// </summary>
    private int CardMaxCount = 8;

    private CardItem nowSelectItem;

    /// <summary>
    /// 当前鼠标指向的卡牌
    /// </summary>
    public CardItem NowSelectItem
    {
        get => nowSelectItem;
        set
        {
            if (nowSelectItem != value)
            {
                nowSelectItem = value;
                RefreshSelectItem(nowSelectItem);
            }
        }
    }

    public GameObject temporaryCard;

    /// <summary>
    /// 当前点击选中的卡牌
    /// </summary>
    private CardItem nowTaskItem;

    public ArrowEffectManager lineEffect;

    /// <summary>
    /// 当前选中敌人
    /// </summary>
    private EnemyRole nowSelectPlayer;

    private Vector3 temporaryCardStartPos;


    void Start()
    {
        InitCard();
    }

    /// <summary>
    /// 数据初始化
    /// </summary>
    public void InitCard()
    {
        int EvenNumber = CardMaxCount % 2 == 0 ? CardMaxCount : CardMaxCount - 1;
        int OddNumber = CardMaxCount % 2 == 0 ? CardMaxCount - 1 : CardMaxCount;
        rotPos_EvenNumber = InitRotPos(EvenNumber);
        rotPos_OddNumber = InitRotPos(OddNumber);
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
        //SelectItemDetection();
        CardUseEffect();
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
        text.Init(OnMouseMoveIn, OnMouseMoveOut, OnMouseCardDown);
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

        int TaskIndex = 0;
        //得到当前选中的卡牌下标
        bool isTaskIndex = GetTaskIndex(out TaskIndex);

        List<float> rotPos;
        int strtCount = 0;
        if (cardList.Count % 2 == 0)
        {
            rotPos = rotPos_EvenNumber;
            strtCount = rotPos_EvenNumber.Count / 2 - cardList.Count / 2;
        }
        else
        {
            rotPos = rotPos_OddNumber;
            strtCount = (rotPos_OddNumber.Count + 1) / 2 - (cardList.Count + 1) / 2;
        }
        for (int i = 0; i < cardList.Count; i++)
        {
            float shifting = 0;
            float indexNowNumber = 0.0065f;
            float Difference = TaskIndex - i;
            float absDifference = Difference > 0 ? 4 - Difference : 4 + Difference;
            if (absDifference < 0)
            {
                absDifference = 0;
            }

            if (isTaskIndex && TaskIndex != i)
            {
                shifting = (TaskIndex > i) ? indexNowNumber * absDifference : -indexNowNumber * absDifference;
            }

            cardList[i].RefreshData(rootPos, rotPos[strtCount + i] + shifting, size, i);
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
        // 测试 ==============
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCard();
        }
        // 测试 ==============

        if (Input.GetMouseButtonUp(0))
        {
            if (nowTaskItem != null)
            {
                if (IsDestoryCard())
                {
                    RemoveCard(nowTaskItem);
                    // 攻击测试
                    if (nowTaskItem.useType == EUseType.Directivity)
                    {
                        nowSelectPlayer?.ChangeHealth(-10);
                    }
                    // =====
                }
                else
                {
                    if (nowTaskItem.useType == EUseType.Directivity) { return; }
                    nowTaskItem.gameObject.SetActive(true);
                    NowSelectItem = null;
                    nowTaskItem = null;
                }

                nowSelectPlayer = null;
                nowCardState = ECardState.None;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (nowTaskItem != null)
            {
                nowTaskItem.gameObject.SetActive(true);
                NowSelectItem = null;
                nowTaskItem = null;
                nowSelectPlayer = null;
                nowCardState = ECardState.None;
            }
        }
    }

    /// <summary>
    /// 是否需要销毁卡牌
    /// </summary>
    /// <returns></returns>
    public bool IsDestoryCard()
    {
        if (nowSelectPlayer != null)
        {
            return true;
        }

        float dis = temporaryCardStartPos.y - temporaryCard.transform.position.y;
        float absDis = dis > 0 ? dis : -dis;
        return absDis > 2.6f;
    }


    /// <summary>
    /// 刷新当前选中的卡牌
    /// </summary>
    /// <param name="selectItem"></param>
    public void RefreshSelectItem(CardItem selectItem)
    {
        if (cardList == null)
        {
            return;
        }

        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].isSelect = cardList[i] == selectItem;
            if (cardList[i] == selectItem)
            {
                temporaryCard.gameObject.transform.position = cardList[i].gameObject.transform.position;
            }
        }
    }

    /// <summary>
    /// 得到当前选中的卡牌下标
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetTaskIndex(out int index)
    {
        index = 0;
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].isSelect)
            {
                index = i;
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 选中卡牌
    /// </summary>
    public void SelectCard(CardItem item)
    {
        nowTaskItem = item;
        nowTaskItem.gameObject.SetActive(false);
    }

    /// <summary>
    /// 选中对象
    /// </summary>
    public void SelectEnemy(EnemyRole enemy)
    {
        if (nowTaskItem == null)
        {
            nowSelectPlayer = null;
            return;
        }

        EUseType etype = nowTaskItem.useType;
        switch (etype)
        {
            case EUseType.NonDirectivity:
                break;
            case EUseType.Directivity:
                nowSelectPlayer = enemy;
                break;
        }
    }

    /// <summary>
    /// 设置卡牌使用特效
    /// </summary>
    public void CardUseEffect()
    {
        //如果当前没有选中卡牌，隐藏所有效果，并返回
        if (nowTaskItem == null)
        {
            temporaryCard.gameObject.SetActive(false);
            lineEffect.gameObject.SetActive(false);
            return;
        }

        temporaryCard.gameObject.SetActive(true);
        // 获取鼠标位置
        Vector3 mousePosition = Input.mousePosition;
        // 设置z坐标为摄像机近裁剪平面的位置
        mousePosition.z = Camera.main.nearClipPlane;
        //将屏幕上的鼠标位置转换为世界坐标系中的位置。
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 5f;
        Vector3 centPos = new Vector3(0, -2.9f, 4);
        bool isWaitAttack = false;
        if (nowTaskItem.useType == EUseType.Directivity)
        {
            if (worldPosition.y > -2.4f)
            {
                //攻击类型卡牌，拖拽超过基础高度标记为等待攻击 
                isWaitAttack = true;
            }
        }

        //如果是等待攻击状态将克隆卡牌移动至centPos否则跟随鼠标移动
        temporaryCard.gameObject.transform.position = Vector3.Lerp(temporaryCard.gameObject.transform.position,
            isWaitAttack ? centPos : worldPosition, Time.deltaTime * 15);
        //设置塞贝尔曲线起始点
        lineEffect.SetStartPos(isWaitAttack ? centPos : worldPosition);
        //设置攻击引导箭头颜色
        lineEffect.SetColor(nowSelectPlayer != null);
        //攻击引导箭头显示隐藏控制
        lineEffect.gameObject.SetActive(isWaitAttack);
    }

    private void OnMouseMoveIn(CardItem item)
    {
        if (nowCardState != ECardState.None) { return; }
        NowSelectItem = item;
    }
    private void OnMouseMoveOut()
    {
        NowSelectItem = null;
    }

    private void OnMouseCardDown(CardItem item)
    {
        if (nowTaskItem != null)
        {
            nowTaskItem.gameObject.SetActive(true);
            nowTaskItem = null;
        }

        temporaryCardStartPos = temporaryCard.transform.position;
        SelectCard(item);
        nowCardState = ECardState.Selecting;
    }

    private ECardState nowCardState = ECardState.None;

    private enum ECardState
    {
        None,
        Selecting,
        Selected,
    }
}

public enum ECardType
{
    /// <summary>
    /// 攻击
    /// </summary>
    Atk,
    /// <summary>
    /// 技能
    /// </summary>
    Skill,
    /// <summary>
    /// 能力
    /// </summary>
    Ability,
    /// <summary>
    /// 状态
    /// </summary>
    State,
    /// <summary>
    /// 其他
    /// </summary>
    Other
}

public enum EUseType
{
    /// <summary>
    /// 指向性
    /// </summary>
    Directivity,
    /// <summary>
    /// 非指向性
    /// </summary>
    NonDirectivity
}