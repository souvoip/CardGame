using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour, ISaveLoad
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
    /// 玩家本局游戏所拥有的卡牌
    /// </summary>
    private List<CardBase> playerAllCards;
    public List<CardBase> PlayerAllCards { get => playerAllCards; }
    /// <summary>
    /// 战斗中抽卡区
    /// </summary>
    private List<CardBase> drawRegionCards;
    public List<CardBase> DrawRegionCards { get => drawRegionCards; }
    /// <summary>
    /// 战斗中手牌区
    /// </summary>
    private List<CardBase> handRegionCards;
    /// <summary>
    /// 战斗中弃牌区
    /// </summary>
    private List<CardBase> discardRegionCards;
    public List<CardBase> DiscardRegionCards { get => discardRegionCards; }
    /// <summary>
    /// 战斗中消耗区
    /// </summary>
    private List<CardBase> costRegionCards;
    public List<CardBase> CostRegionCards { get => costRegionCards; }
    /// <summary>
    /// 战斗中移除区
    /// </summary>
    private List<CardBase> removeRegionCards;

    [SerializeField]
    private TMP_Text drawCountTxt;
    [SerializeField]
    private TMP_Text discardCountTxt;
    [SerializeField]
    private TMP_Text costCountTxt;

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
    // private EnemyRole nowSelectEnemy;
    // public EnemyRole NowSelectEnemy { get => nowSelectEnemy; }

    private Vector3 temporaryCardStartPos;


    void Start()
    {
        InitCard();

        AddEvents();
    }

    private void OnDestroy()
    {
        RemoveEvents();
    }

    public void InitGameCardData(JSONObject data = null)
    {
        if (data != null)
        {
            Load(data);
            return;
        }

        #region Test
        playerAllCards = new List<CardBase>();
        for (int i = 0; i < 5; i++)
        {
            playerAllCards.Add(CardDataManager.GetCard(1));
            playerAllCards.Add(CardDataManager.GetCard(101));
        }
        playerAllCards.Add(CardDataManager.GetCard(-1));
        playerAllCards.Add(CardDataManager.GetCard(2));
        playerAllCards.Add(CardDataManager.GetCard(102));
        playerAllCards.Add(CardDataManager.GetCard(201));
        playerAllCards.Add(CardDataManager.GetCard(301));
        //InitBattleCardData();

        //TimerTools.Timer.Once(0.1f, () =>
        //{
        //    BattleManager.Instance.TestBattle();
        //});
        #endregion
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

        if (drawRegionCards != null)
        {
            drawCountTxt.text = drawRegionCards.Count.ToString();
            discardCountTxt.text = discardRegionCards.Count.ToString();
            costCountTxt.text = costRegionCards.Count.ToString();
        }
    }

    /// <summary>
    /// 战斗开始，初始化卡牌
    /// </summary>
    public void InitBattleCardData()
    {
        CardMaxCount = BattleManager.Instance.Player.RoleData.MaxCardCount;
        if (cardList != null && cardList.Count > 0)
        {
            for (int i = cardList.Count - 1; i >= 0; i--)
            {
                Destroy(cardList[i].gameObject);
                RemoveCard(cardList[i]);
            }
        }

        drawRegionCards = new List<CardBase>();
        for (int i = 0; i < playerAllCards.Count; i++)
        {
            drawRegionCards.Add(Instantiate(playerAllCards[i]));
        }
        drawRegionCards.ShuffleList();
        handRegionCards = new List<CardBase>();
        discardRegionCards = new List<CardBase>();
        costRegionCards = new List<CardBase>();
        removeRegionCards = new List<CardBase>();
    }

    private void AddEvents()
    {
        EventCenter<CardExtract>.GetInstance().AddEventListener(EventNames.EXTRACT_CARD, OnExtractCard);
        TurnManager.OnPlayerTurnEnd += PlayerTurnEnd;
        TurnManager.OnPlayerTurnStart += TurnStart;
    }

    private void RemoveEvents()
    {
        EventCenter<CardExtract>.GetInstance().RemoveEventListener(EventNames.EXTRACT_CARD, OnExtractCard);
        TurnManager.OnPlayerTurnEnd -= PlayerTurnEnd;
        TurnManager.OnPlayerTurnEnd -= TurnStart;
    }

    private void TurnStart()
    {
        // 抽卡 TODO: 需要完善,抽取固有卡牌, 每回抽取的卡牌数量
        int reduce = 0;
        if (TurnManager.CurrentTurnCount == 1)
        {
            reduce = DrawFixedCards();
        }
        for (int i = 0; i < BattleManager.Instance.Player.RoleData.DrawCardCount - reduce; i++)
        {
            DrawCards(EExtractMode.Order, EExtractCardType.All);
        }
    }


    private void PlayerTurnEnd()
    {
        // 丢弃卡牌
        for (int i = cardList.Count - 1; i >= 0; i--)
        {
            if ((cardList[i].CardData.Features & ECardFeatures.Hold) == ECardFeatures.Hold)
            {
                break;
            }
            if ((cardList[i].CardData.Features & ECardFeatures.Void) == ECardFeatures.Void)
            {
                costRegionCards.Add(cardList[i].CardData);
            }
            else
            {
                discardRegionCards.Add(cardList[i].CardData);
            }
            handRegionCards.Remove(cardList[i].CardData);
            cardList[i].PlayMoveToDiscardAnim(cardList[i].transform.position, (item) => { Destroy(item.gameObject); });
            Destroy(cardList[i].gameObject);
            RemoveCard(cardList[i]);
        }
    }

    /// <summary>
    /// 抽卡 TODO : 需要完善，抽取固有卡牌以及指定类型卡牌
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="cardType"></param>
    private void DrawCards(EExtractMode mode, EExtractCardType cardType)
    {
        if (cardList.Count > CardMaxCount) { return; }
        if (drawRegionCards.Count == 0)
        {
            if (!ShuffleCards()) { return; }
        }
        CardBase cardTemp;
        switch (mode)
        {
            case EExtractMode.Order:
                cardTemp = drawRegionCards[0];
                drawRegionCards.RemoveAt(0);
                handRegionCards.Add(cardTemp);
                AddCard(cardTemp);
                break;
            case EExtractMode.Random:
                cardTemp = drawRegionCards[UnityEngine.Random.Range(0, drawRegionCards.Count)];
                drawRegionCards.Remove(cardTemp);
                handRegionCards.Add(cardTemp);
                AddCard(cardTemp);
                break;
            case EExtractMode.Specified:
                break;
        }
    }

    /// <summary>
    /// 抽取固有卡牌
    /// </summary>
    /// <returns> 抽取的数量 </returns>
    private int DrawFixedCards()
    {
        int count = 0;
        CardBase cardTemp;
        for (int i = 0; i < drawRegionCards.Count; i++)
        {
            if ((drawRegionCards[i].Features & ECardFeatures.Fixed) == ECardFeatures.Fixed)
            {
                cardTemp = drawRegionCards[i];
                drawRegionCards.RemoveAt(i);
                handRegionCards.Add(cardTemp);
                AddCard(cardTemp);
                count++;
                if (count >= BattleManager.Instance.Player.RoleData.DrawCardCount)
                {
                    break;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// 添加卡牌
    /// </summary>
    public void AddCard(CardBase card)
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
        text.Init(card, OnMouseMoveIn, OnMouseMoveOut, OnMouseCardDown);
        text.UpdateData();
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
        //Destroy(item.gameObject);
    }

    /// <summary>
    /// 玩家操作检测
    /// </summary>
    public void TaskItemDetection()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (nowTaskItem != null)
            {
                if (IsDestoryCard())
                {
                    if (nowTaskItem.useType == EUseType.Directivity)
                    {
                        nowTaskItem.CardData.UseCard(BattleManager.Instance.nowSelectEnemy);
                    }
                    else
                    {
                        nowTaskItem.CardData.UseCard();
                    }
                    UseCardOver(nowTaskItem);
                    RemoveCard(nowTaskItem);
                    nowTaskItem = null;
                }
                else
                {
                    return;
                    //if (nowTaskItem.useType == EUseType.Directivity) { return; }
                    //nowTaskItem.gameObject.SetActive(true);
                    //NowSelectItem = null;
                    //nowTaskItem = null;
                }

                //nowSelectEnemy = null;
                nowCardState = ECardState.None;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            CancelSelect();
        }
    }

    /// <summary>
    /// 是否需要销毁卡牌
    /// </summary>
    /// <returns></returns>
    public bool IsDestoryCard()
    {
        // 卡牌能否使用
        if (!nowTaskItem.CardData.IsCanUse()) { return false; }

        if (BattleManager.Instance.nowSelectEnemy != null)
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

    public void AddBattleCard(CardBase card)
    {
        playerAllCards.Add(card);
    }

    public void RemoveBattleCard(CardBase card)
    {
        Debug.Log("RemoveBattleCard");
        Debug.Log(playerAllCards.Remove(card));
    }

    /// <summary>
    /// 选中对象
    /// </summary>
    //public void SelectEnemy(EnemyRole enemy)
    //{
    //    nowSelectEnemy = enemy;
    //    return;
    //    //if (nowTaskItem == null)
    //    //{
    //    //    nowSelectEnemy = null;
    //    //    return;
    //    //}

    //    //EUseType etype = nowTaskItem.useType;
    //    //switch (etype)
    //    //{
    //    //    case EUseType.NonDirectivity:
    //    //        break;
    //    //    case EUseType.Directivity:
    //    //        break;
    //    //}
    //    //nowTaskItem.UpdateDesc();
    //    //temporaryCard.GetComponent<TempCardItem>().UpdateDesc(nowTaskItem.CardData);
    //}

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
            UIManager.Instance.EnableUIInteraction();
            return;
        }

        temporaryCard.gameObject.SetActive(true);
        temporaryCard.GetComponent<TempCardItem>().UpdateData(nowTaskItem.CardData);
        // 获取鼠标位置
        Vector3 mousePosition = Input.mousePosition;
        // 设置z坐标为摄像机近裁剪平面的位置
        mousePosition.z = Camera.main.nearClipPlane;
        //将屏幕上的鼠标位置转换为世界坐标系中的位置。
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 5f;
        Vector3 centPos = new Vector3(0, -2.9f, 4);
        bool isWaitAttack = false;
        // 判断当前卡牌能否使用，不能使用则取消选中
        if (worldPosition.y > -2.4f)
        {
            if (!nowTaskItem.CardData.IsCanUse())
            {
                CancelSelect();
                return;
            }
        }

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
        lineEffect.SetStartPos(isWaitAttack ? Camera.main.WorldToScreenPoint(centPos) : worldPosition);
        //设置攻击引导箭头颜色
        lineEffect.SetColor(BattleManager.Instance.nowSelectEnemy != null);
        //攻击引导箭头显示隐藏控制
        lineEffect.gameObject.SetActive(isWaitAttack);
        UIManager.Instance.DisableUIInteraction();
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


    private void OnExtractCard(CardExtract ce)
    {
        for (int i = 0; i < ce.Count; i++)
        {
            if (ce.origin == ECardRegion.Draw)
            {
                if (ce.target == ECardRegion.Hand)
                {
                    DrawCards(ce.mode, ce.cardType);
                }
            }
        }
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    /// <returns></returns>
    private bool ShuffleCards()
    {
        if (drawRegionCards.Count == 0)
        {
            if (discardRegionCards.Count == 0) { return false; }
            // TODO: 将弃牌堆中对卡牌移动到抽牌堆
            drawRegionCards = discardRegionCards;
            discardRegionCards = new List<CardBase>();
            drawRegionCards.ShuffleList();
        }
        return true;
    }

    private void UseCardOver(CardItem cardItem)
    {
        handRegionCards.Remove(cardItem.CardData);
        if (cardItem.CardData.CardType == ECardType.Ability)
        {
            removeRegionCards.Add(cardItem.CardData);
            cardItem.PlayDissolveAnim(temporaryCard.transform.position, (item) => { Destroy(item.gameObject); });
        }
        else if ((cardItem.CardData.Features & ECardFeatures.Cost) == ECardFeatures.Cost)
        {
            // 移动到消耗堆
            costRegionCards.Add(cardItem.CardData);
            cardItem.PlayDissolveAnim(temporaryCard.transform.position, (item) => { Destroy(item.gameObject); });
        }
        else
        {
            // 移动到弃牌堆
            discardRegionCards.Add(cardItem.CardData);
            cardItem.PlayMoveToDiscardAnim(temporaryCard.transform.position, (item) => { Destroy(item.gameObject); });
        }
    }

    private void CancelSelect()
    {
        if (nowTaskItem != null)
        {
            nowTaskItem.gameObject.SetActive(true);
            NowSelectItem = null;
            nowTaskItem = null;
            //nowSelectEnemy = null;
            nowCardState = ECardState.None;
        }
    }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create(JSONObject.Type.ARRAY);
        foreach (var item in playerAllCards)
        {
            data.Add(item.Save());
        }
        return data;
    }

    public void Load(JSONObject data)
    {
        if (playerAllCards == null)
        {
            playerAllCards = new List<CardBase>();
        }
        else
        {
            playerAllCards.Clear();
        }
        for (int i = 0; i < data.Count; i++)
        {
            JSONObject cd = data[i];
            CardBase card = CardDataManager.GetCard((int)cd.GetField("ID").i);
            card.Load(cd);
            playerAllCards.Add(card);
        }
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
    /// 诅咒
    /// </summary>
    Curse
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
    NonDirectivity,
    /// <summary>
    /// 无法使用
    /// </summary>
    CannotUse
}
