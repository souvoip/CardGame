using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapItemBase : MonoBehaviour
{
    protected Image mapItemImage;

    private Coroutine enableAnimCoroutine = null;

    public EMapItemType Type;

    public EMapState State;

    public int Layer;

    public List<MapItemBase> NextItemList = new List<MapItemBase>();

    public List<MapItemBase> PrevItems = new List<MapItemBase>();

    private static float screenScale;

    public static float ScreenScale
    {
        get
        {
            if (screenScale == 0)
            {
                screenScale = 1920f / Screen.width;
            }
            return screenScale;
        }
    }

    public virtual void Init(EMapItemType itemType, EMapState state, int layer)
    {
        mapItemImage = transform.GetComponent<Image>();
        Type = itemType;
        State = state;
        Layer = layer;

        transform.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (State == EMapState.Enable)
        {
            OnEnterEffect();
            MapManager.Instance.UpdateMapStateInfo();
            MapManager.Instance.CurrentLayer = Layer;
            MapManager.Instance.CurrentMapItem = this;
            EnterEvent();
        }
    }

    protected virtual void EnterEvent()
    {
        // 默认设置为战斗场景
        BattleManager.Instance.StartBattleTest();
    }

    public virtual void OnEnterEffect()
    {
        MapManager.Instance.DisableLayerNode(Layer);
        State = EMapState.Current;
        foreach (var item in PrevItems)
        {
            if (item.State == EMapState.Current)
            {
                item.State = EMapState.Over;
            }
            else
            {
                item.State = EMapState.Disable;
            }
        }
        foreach (var item in NextItemList)
        {
            item.State = EMapState.Enable;
        }
    }

    public MapItemBase GetNearestDistanceItem(Dictionary<int, MapItemBase> comparelayerDic)
    {
        if (comparelayerDic == null || comparelayerDic.Count == 0) { return null; }

        MapItemBase nearNode = null;
        float minDistance = float.MaxValue;

        foreach (var item in comparelayerDic)
        {
            if (nearNode == null)
            {
                nearNode = item.Value;
                minDistance = Vector2.Distance(transform.position, item.Value.transform.position);
                continue;
            }
            float distance = Vector2.Distance(transform.position, item.Value.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearNode = item.Value;
            }
        }
        return nearNode;
    }

    public void RefreshUI(string value, int nowLayer)
    {
    }

    public void UpdateState()
    {
        switch (State)
        {
            case EMapState.Current:
                //mapItemImage.color = Color.blue;
                StopEnableAnim();
                mapItemImage.color = Color.cyan;
                break;
            case EMapState.Disable:
                //mapItemImage.color = Color.gray;
                StopEnableAnim();
                break;
            case EMapState.Enable:
                //mapItemImage.color = Color.white;
                enableAnimCoroutine = StartCoroutine(nameof(EnableAnimCoroutine));
                break;
            case EMapState.Over:
                //mapItemImage.color = Color.yellow;
                StopEnableAnim();
                mapItemImage.color = Color.green;
                break;
            default:
                break;
        }
    }

    private void StopEnableAnim()
    {
        if (enableAnimCoroutine != null)
        {
            StopCoroutine(nameof(EnableAnimCoroutine));
            StartCoroutine(nameof(ResetScaleCoroutine));
        }
    }

    public void DrawLine(Transform lineObj, Transform lineNode)
    {
        foreach (var item in NextItemList)
        {
            Transform line = Instantiate(lineObj, lineNode);
            float dis = Vector2.Distance(transform.position, item.transform.position);
            line.eulerAngles = new Vector3(0, 0, transform.GetAngleBetweenObjects(item.transform));
            line.localScale = new Vector3(dis * ScreenScale, 1, 1);
            line.position = transform.position;
            line.gameObject.SetActive(true);
        }
    }


    protected IEnumerator EnableAnimCoroutine()
    {
        float scale = 1;
        float speed = 0.2f;
        float add = 1;
        while (true)
        {
            scale += speed * Time.deltaTime * add;
            if (scale > 1.2f || scale < 0.9f)
            {
                add = -add;
            }
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }

    protected IEnumerator ResetScaleCoroutine()
    {
        float scale = transform.localScale.x;
        float speed = 5f;
        while (scale != 1)
        {
            scale = Mathf.Lerp(scale, 1, speed * Time.deltaTime);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }

}

public enum EMapItemType
{
    Boss = 0,        // 0
    Event = 25,      // 0 - 25
    Battle = 60,     // 25 - 60
    HardBattle = 75, // 60 - 75
    Shop = 80,       // 75 - 80
    Treasures = 85,  // 80 - 85
    Restore = 100,    // 85 - 100


    EnumEnd
}

public enum EMapState
{
    Current,
    Disable,
    Enable,
    Over
}
