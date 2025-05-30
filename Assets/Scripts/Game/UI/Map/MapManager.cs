using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour, ISaveLoad
{
    public static MapManager Instance;
    public int CurrentLayer = 0;
    public MapItemBase CurrentMapItem;
    [SerializeField]
    private int MaxLayer = 15;

    //[SerializeField]
    private int startNodeNumber = 3;

    private Transform mapItemPrefab;
    private Transform linePrefab;
    private Transform lineNode;
    private Transform nodeRegion;

    private List<MapLayerItem> layerList;
    private Dictionary<int, MapLayerItem> layerDic;

    private DrawLineManager dlm;

    private void Awake()
    {
        dlm = new DrawLineManager();
        Instance = this;
        nodeRegion = transform.Find("Map/Viewport/NodeRegion");
        mapItemPrefab = nodeRegion.Find("MapItemBase");
        linePrefab = nodeRegion.Find("Line");
        lineNode = nodeRegion.Find("LinesNode");
        //CreatorMap();
    }

    private void Start()
    {
        //CreatorMap();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        UpdateMapStateInfo();
        ResetMapShowRegion();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void CreatorMap()
    {
        nodeRegion.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 300 * (MaxLayer + 1));
        dlm.ClearLine(lineNode);
        startNodeNumber = Random.Range(2, 5);
        InitData();
        dlm.RefreshLineData(layerList);
        dlm.DrawLine(layerList, linePrefab, lineNode);
        UpdateMapStateInfo();
    }

    private void LoadMap()
    {
        nodeRegion.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 300 * (MaxLayer + 1));
        dlm.ClearLine(lineNode);
        dlm.RefreshLineData(layerList);
        dlm.DrawLine(layerList, linePrefab, lineNode);
        UpdateMapStateInfo();
    }

    public void InitData()
    {
        if (layerDic == null)
        {
            layerDic = new Dictionary<int, MapLayerItem>();
        }
        if (layerList == null)
        {
            layerList = new List<MapLayerItem>();
        }
        for (int i = 0; i < MaxLayer; i++)
        {
            ELayerType eLayerType = i == 0 ? ELayerType.Start : i == MaxLayer - 1 ? ELayerType.End : ELayerType.Cent;
            if (layerDic.ContainsKey(i))
            {
                layerDic[i].InitData(i, eLayerType, startNodeNumber, mapItemPrefab);
            }
            else
            {
                MapLayerItem item = new MapLayerItem();
                item.InitData(i, eLayerType, startNodeNumber, mapItemPrefab);
                layerDic.Add(i, item);
                layerList.Add(item);
            }
        }
        if (MaxLayer < layerList.Count)
        {
            for (int i = MaxLayer; i < layerList.Count; i++)
            {
                layerList[i].Clear();
            }
        }
    }

    public void UpdateMapStateInfo()
    {
        foreach (var item in layerList)
        {
            foreach (var map in item.MapItemDic)
            {
                map.Value.UpdateState();
            }
        }
    }

    public void DisableLayerNode(int layer)
    {
        foreach (var item in layerList[layer].MapItemDic)
        {
            item.Value.State = EMapState.Disable;
        }
    }

    public void ResetMapShowRegion()
    {
        nodeRegion.position = new Vector3(nodeRegion.position.x, Mathf.Max(-300 * CurrentLayer, Screen.height - 300 * (MaxLayer + 1)), nodeRegion.position.z);
    }

    /// <summary>
    /// 获取随机的地图类型
    /// </summary>
    /// <returns></returns>
    public EMapItemType GetRandomMapItemType()
    {
        int random = Random.Range(0, (int)EMapItemType.EnumEnd);
        if (random < (int)EMapItemType.Event)
        {
            return EMapItemType.Event;
        }
        else if (random < (int)EMapItemType.Battle)
        {
            return EMapItemType.Battle;
        }
        else if (random < (int)EMapItemType.HardBattle)
        {
            return EMapItemType.HardBattle;
        }
        else if (random < (int)EMapItemType.Shop)
        {
            return EMapItemType.Shop;
        }
        else if (random < (int)EMapItemType.Treasures)
        {
            return EMapItemType.Treasures;
        }
        else
        {
            return EMapItemType.Restore;
        }
    }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("CurrentLayer", CurrentLayer);
        JSONObject layerDicDatas = JSONObject.Create(JSONObject.Type.ARRAY);
        foreach (var item in layerDic)
        {
            JSONObject layerDicData = JSONObject.Create();
            layerDicData.AddField("Key", item.Key);
            layerDicData.AddField("Value", item.Value.Save());
            layerDicDatas.Add(layerDicData);
        }
        data.AddField("LayerDic", layerDicDatas);
        return data;
        //throw new System.NotImplementedException();
    }

    public void Load(JSONObject data)
    {
        if (layerDic == null)
        {
            layerDic = new Dictionary<int, MapLayerItem>();
            layerList = new List<MapLayerItem>();
        }
        else
        {
            for (int i = 0; i < layerList.Count; i++)
            {
                layerList[i].Clear();
            }
            layerDic.Clear();
            layerList.Clear();
        }

        CurrentLayer = (int)data.GetField("CurrentLayer").i;
        JSONObject layerDicDatas = data.GetField("LayerDic");
        for (int i = 0; i < layerDicDatas.Count; i++)
        {
            JSONObject layerDicData = layerDicDatas[i];
            MapLayerItem mapLayerItem = new MapLayerItem();
            mapLayerItem.LoadData(layerDicData.GetField("Value"), mapItemPrefab);
            layerDic.Add(mapLayerItem.Layer, mapLayerItem);
            layerList.Add(mapLayerItem);
        }

        LoadMap();

        ResetMapShowRegion();
        //throw new System.NotImplementedException();
    }
}
