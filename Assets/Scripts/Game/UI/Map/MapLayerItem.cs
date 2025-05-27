using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class MapLayerItem : ISaveLoad
{
    private static int xRandom = 50;
    private static int yRandom = 45;
    private static int ySize = 300;
    private static int xSize = 400;
    private static int entranceMaxCount = 4;

    public int Layer;

    public Dictionary<int, MapItemBase> MapItemDic;

    public ELayerType LayerType;

    public void InitData(int layer, ELayerType layerType, int startNodeNum, Transform mapItemPrefab)
    {
        Layer = layer;
        LayerType = layerType;

        int creatorCount = 0;

        switch (LayerType)
        {
            case ELayerType.Start:
                creatorCount = startNodeNum;
                break;
            case ELayerType.Cent:
                creatorCount = Random.Range(2, startNodeNum * 2);
                break;
            case ELayerType.End:
                creatorCount = 1;
                break;
        }

        InitLayerDic(creatorCount, mapItemPrefab);
    }
    public void InitLayerDic(int Count, Transform rootObj)
    {
        if (MapItemDic == null)
        {
            MapItemDic = new Dictionary<int, MapItemBase>();
        }
        else
        {
            Clear();
        }
        for (int i = 0; i < Count; i++)
        {
            MapItemBase nowItem = CreatorMap(rootObj);
            nowItem.gameObject.SetActive(true);
            nowItem.Init(EMapItemType.Battle, Layer == 0 ? EMapState.Enable : EMapState.Disable, Layer);
            //int xR = Random.Range(-xRandom, xRandom);
            int yR = Random.Range(-yRandom, yRandom);
            if (Layer == 0)
            {
                yR = 0;
            }
            float xPos = GetPosX(i, Count);
            float addendPosY = ELayerType.End == LayerType ? 70 : 0;
            nowItem.transform.localPosition = new Vector3(xPos, Layer * ySize + yR + addendPosY + 100, 0);
            nowItem.RefreshUI(Layer + "_" + i, Layer);
            MapItemDic.Add(i, nowItem);
        }

    }

    private MapItemBase CreatorMap(Transform rootObj)
    {
        Transform obj = GameObject.Instantiate(rootObj, rootObj.parent);

        MapItemBase nowItem = null;
        if(LayerType == ELayerType.End)
        {
            nowItem = obj.AddComponent<MapItem_Boss>();
        }
        else
        {
            EMapItemType type = MapManager.Instance.GetRandomMapItemType();
            if(LayerType == ELayerType.Start)
            {
                type = Random.Range(0, 100) < 60 ? EMapItemType.Battle : EMapItemType.Event;
            }
            
            switch (type)
            {
                case EMapItemType.Battle:
                    nowItem = obj.AddComponent<MapItem_Battle>();
                    break;
                case EMapItemType.Event:
                    nowItem = obj.AddComponent<MapItem_Event>();
                    break;
                case EMapItemType.HardBattle:
                    nowItem = obj.AddComponent<MapItem_HardBattle>();
                    break;
                case EMapItemType.Treasures:
                    nowItem = obj.AddComponent<MapItem_Treasures>();
                    break;
                case EMapItemType.Shop:
                    nowItem = obj.AddComponent<MapItem_Shop>();
                    break;
                case EMapItemType.Restore:
                    nowItem = obj.AddComponent<MapItem_Restore>();
                    break;
            }
        }

        return nowItem;
    }

    private float GetPosX(int nowIndix, float Count)
    {

        float onePos = entranceMaxCount * xSize / (Count + 1);
        float pos = (nowIndix + 1) * onePos;
        int xR = Random.Range(-xRandom, xRandom);
        return pos + xR;
    }

    public void Clear()
    {
        foreach (var item in MapItemDic)
        {
            GameObject.Destroy(item.Value.gameObject);
        }
        MapItemDic.Clear();
    }

    /// <summary>
    /// 刷新上层数据列表
    /// </summary>
    /// <param name="ComparelayerDic"></param>
    public void RefreshTopLineData(Dictionary<int, MapItemBase> ComparelayerDic)
    {
        if (MapItemDic == null) { return; }
        foreach (var item in MapItemDic)
        {
            MapItemBase nowItem = item.Value;
            nowItem.NextItemList.Clear();
            //获取距离此节点最近的对象
            MapItemBase topItem = nowItem.GetNearestDistanceItem(ComparelayerDic);
            if (topItem != null)
            {
                topItem.PrevItems.Add(nowItem);
                nowItem.NextItemList.Add(topItem);
            }
        }

    }

    /// <summary>
    /// 断路检索
    /// </summary>
    /// <param name="ComparelayerDic"></param>
    public void TopLineAdditional(Dictionary<int, MapItemBase> ComparelayerDic)
    {
        if (MapItemDic == null) { return; }
        foreach (var item in MapItemDic)
        {
            MapItemBase nowItem = item.Value;
            if (nowItem.PrevItems == null || nowItem.PrevItems.Count == 0)
            {
                //获取距离此节点最近的对象
                MapItemBase downItem = nowItem.GetNearestDistanceItem(ComparelayerDic);
                if (downItem != null)
                {
                    nowItem.PrevItems.Add(downItem);
                    downItem.NextItemList.Add(nowItem);
                }
            }
        }
    }

    public JSONObject Save()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("Layer", Layer);
        JSONObject mapItemDicDatas = JSONObject.Create(JSONObject.Type.ARRAY);
        foreach (var item in MapItemDic)
        {
            JSONObject mapItemData = JSONObject.Create();
            mapItemData.AddField("Key", item.Key);
            mapItemData.AddField("Value", item.Value.Save());
            mapItemDicDatas.Add(mapItemData);
        }
        data.AddField("MapItemDic", mapItemDicDatas);
        data.AddField("LayerType", (int)LayerType);
        return data;
    }

    public void Load(JSONObject data)
    {
        Layer = (int)data.GetField("Layer").i;
        LayerType = (ELayerType)data.GetField("LayerType").i;
        JSONObject mapItemDicDatas = data.GetField("MapItemDic");
    }
}

public enum ELayerType
{
    Start,
    Cent,
    End,
}
