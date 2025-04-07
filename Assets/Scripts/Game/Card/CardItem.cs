using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItem : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler, IPointerDownHandler
{
    #region component
    [SerializeField]
    private Image backgroundImg;
    [SerializeField]
    private Image cardImg;
    [SerializeField]
    private TMP_Text typeTxt;
    [SerializeField]
    private TMP_Text descTxt;
    #endregion

    private CardBase cardData;

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

    public EUseType useType;

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
        SetPos();
    }
        
    public void Init(UnityAction<CardItem> onMouseMoveIn, UnityAction onMouseMoveOut, UnityAction<CardItem> onMouseDown)
    {
        this.onMouseMoveIn = onMouseMoveIn;
        this.onMouseMoveOut = onMouseMoveOut;
        this.onMouseDown = onMouseDown;
    }

    public void SetPos()
    {
        //选中卡牌半径增加  
        float radius = isSelect ? size + 0.2f : size;
        //选中卡牌层级提高，使卡牌展示位于所有手牌之上  
        float selectZ = isSelect ? this.zPos - 0.1f : this.zPos;
        //选中卡牌旋转归零  
        float rotZ = isSelect ? 0 : GetAngleInDegrees(root, transform.position);
        // 选中卡牌大小提高成1.25倍
        float scale = isSelect ? 1.25f : 1f;
        //设置卡牌位置  
        float x = root.x + Mathf.Cos(rot) * radius;
        float y = isSelect ? -3.4f : root.y + Mathf.Sin(rot) * radius;
        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, root.z + selectZ), Time.deltaTime * animSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(scale, scale, scale), Time.deltaTime * animSpeed);
        Quaternion rotationQuaternion = Quaternion.Euler(new Vector3(0, 0, rotZ));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationQuaternion, Time.deltaTime * animSpeed * 30);
    }

    /// <summary>  
    /// 获取两个向量之间的弧度值0-2π  
    /// </summary>    /// <param name="positionA">点A坐标</param>  
    /// <param name="positionB">点B坐标</param>  
    /// <returns></returns>
    public static float GetAngleInDegrees(Vector3 positionA, Vector3 positionB)
    {
        // 计算从A指向B的向量  
        Vector3 direction = positionB - positionA;
        // 将向量标准化  
        Vector3 normalizedDirection = direction.normalized;
        // 计算夹角的弧度值  
        float dotProduct = Vector3.Dot(normalizedDirection, Vector3.up);
        float angleInRadians = Mathf.Acos(dotProduct);

        //判断夹角的方向：通过计算一个参考向量与两个物体之间的叉乘，可以确定夹角是顺时针还是逆时针方向。这将帮助我们将夹角的范围扩展到0到360度。  
        Vector3 cross = Vector3.Cross(normalizedDirection, Vector3.up);
        if (cross.z > 0)
        {
            angleInRadians = 2 * Mathf.PI - angleInRadians;
        }
        // 将弧度值转换为角度值  
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
        return angleInDegrees;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onMouseMoveIn?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseMoveOut?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onMouseDown?.Invoke(this);
    }
}

