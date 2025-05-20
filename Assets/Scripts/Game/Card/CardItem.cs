using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItem : MonoBehaviour, IPointerExitHandler, IPointerDownHandler, IPointerEnterHandler
{
    private static Vector3 discardPos = new Vector3(9.78f, -5.86f, 0);

    #region component
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
    #endregion
    [SerializeField]
    private Material dissolveMat;

    private static float dissolveTime = 1.2f;

    private bool isPlayAnim = false;

    private CardBase cardData;
    public CardBase CardData { get => cardData; }

    /// <summary>  
    /// 卡牌扇形展开中心点  
    /// </summary>  
    private Vector3 root;
    /// <summary>  
    /// 展开角度  
    /// </summary>  
    private float rot;
    /// <summary>  
    /// 展开半径  
    /// </summary> 
    private float size;
    /// <summary>  
    /// 动画速度  
    /// </summary>  
    [SerializeField]
    private float animSpeed = 10;
    /// <summary>  
    /// 高度值（决定卡牌层级）  
    /// </summary> 
    private float zPos = 0;
    /// <summary>  
    /// 当前卡牌是否被选中  
    /// </summary> 
    public bool isSelect;

    public ECardType attackType;

    public EUseType useType => cardData.UseType;

    private UnityAction<CardItem> onMouseMoveIn;

    private UnityAction onMouseMoveOut;

    private UnityAction<CardItem> onMouseDown;

    public void RefreshData(Vector3 root, float rot, float size, float zPos)
    {
        this.root = root;
        this.rot = rot;
        this.size = size;
        this.zPos = -zPos * 0.02f;
    }
    // Update is called once per frame  
    void Update()
    {
        if(isPlayAnim) { return; }
        SetPos();
    }

    public void Init(CardBase cardData, UnityAction<CardItem> onMouseMoveIn, UnityAction onMouseMoveOut, UnityAction<CardItem> onMouseDown)
    {
        this.cardData = cardData;
        this.onMouseMoveIn = onMouseMoveIn;
        this.onMouseMoveOut = onMouseMoveOut;
        this.onMouseDown = onMouseDown;

        EventCenter.GetInstance().AddEventListener(EventNames.CHARACTER_BUFF_UPDATA, UpdateDesc);
    }

    private void OnDestroy()
    {
        EventCenter.GetInstance().RemoveEventListener(EventNames.CHARACTER_BUFF_UPDATA, UpdateDesc);
    }

    public void SetPos()
    {
        //选中卡牌半径增加  
        float radius = isSelect ? size + 0.2f : size;
        //选中卡牌层级提高，使卡牌展示位于所有手牌之上  
        float selectZ = isSelect ? this.zPos - 0.1f : this.zPos;
        //选中卡牌旋转归零  
        float rotZ = isSelect ? 0 : GameTools.GetAngleInDegrees(root, transform.position);
        // 选中卡牌大小提高成1.25倍
        float scale = isSelect ? 1.25f : 1f;
        //设置卡牌位置
        float x = root.x + Mathf.Cos(rot) * radius;
        float y = isSelect ? -3.4f : root.y + Mathf.Sin(rot) * radius;
        if (isSelect)
        {
            transform.position = new Vector3(x, y, root.z + selectZ);
            transform.localScale = new Vector3(scale, scale, scale);
            Quaternion rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, rotZ));
            transform.rotation = rotationQuaternion;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, root.z + selectZ), Time.deltaTime * animSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale, scale, scale), Time.deltaTime * animSpeed);
            Quaternion rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, rotZ));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationQuaternion, Time.deltaTime * animSpeed * 30);
        }
    }

    public void UpdateData()
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

    public void UpdateDesc()
    {
        descTxt.text = cardData.GetDesc();
    }

    /// <summary>
    /// 播放卡牌消失动画
    /// </summary>
    /// <param name="overAction"></param>
    public void PlayDissolveAnim(Vector3 pos, Action<CardItem> overAction = null)
    {
        transform.position = pos;
        isPlayAnim = true;
        gameObject.SetActive(true);
        StartCoroutine(DissolveAnimCoroutine(overAction));
    }
    /// <summary>
    /// 播放卡牌移动到弃牌库动画
    /// </summary>
    public void PlayMoveToDiscardAnim(Vector3 pos, Action<CardItem> overAction = null)
    {
        transform.position = pos;
        //transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        isPlayAnim = true;
        transform.DOMove(discardPos, 0.5f).OnComplete(() =>
        {
            overAction?.Invoke(this);
        });
    }

    //public void OnPointerMove(PointerEventData eventData)
    //{
    //    onMouseMoveIn?.Invoke(this);
    //}

    [SerializeField]
    private Vector2 tempOffset;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isPlayAnim) { return; }
        onMouseMoveIn?.Invoke(this);
        StartCoroutine(ShowCardDetailInfoCoroutine());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPlayAnim) { return; }
        onMouseMoveOut?.Invoke();
        TimerTools.Timer.FrameOnce(1, () =>
        {
            UIManager.Instance.holdDetailUI.Hide();
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlayAnim) { return; }
        onMouseDown?.Invoke(this);
        UIManager.Instance.holdDetailUI.Hide();
    }

    private IEnumerator ShowCardDetailInfoCoroutine()
    {
        yield return null;
        // 显示提示
        Vector3 spos = Camera.main.WorldToScreenPoint(transform.position);
        UIManager.Instance.holdDetailUI.ShowInfos(spos, tempOffset, cardData.GetDetailInfos());
    }

    private IEnumerator DissolveAnimCoroutine(Action<CardItem> overAction)
    {
        var imgs = transform.GetComponentsInChildren<Image>();
        var dissolves = Instantiate(dissolveMat);
        foreach (var img in imgs)
        {
            img.material = dissolves;
        }
        yield return null;
        float dt = dissolveTime;
        float value;
        while (dt > 0)
        {
            dt -= Time.deltaTime;
            value = dt / dissolveTime;
            dissolves.SetFloat("_Progress", dt / dissolveTime);
            nameTxt.alpha = value;
            feeTxt.alpha = value;
            typeTxt.alpha = value;
            descTxt.alpha = value;
            yield return null;
        }
        overAction?.Invoke(this);
    }
}

