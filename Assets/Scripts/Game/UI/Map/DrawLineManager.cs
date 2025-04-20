using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager
{
    /// <summary>
    /// 刷新地图路径数据
    /// </summary>
    public void RefreshLineData(List<MapLayerItem> layerList)
    {
        if (layerList == null)
        {
            return;
        }
        int MaxCount = layerList.Count;
        for (int i = 0; i < layerList.Count; i++)
        {
            //上层
            MapLayerItem TopItem = i == MaxCount - 1 ? null : layerList[i + 1];
            //当前层
            MapLayerItem nowItem = layerList[i];
            nowItem.RefreshTopLineData(TopItem == null ? null : TopItem.MapItemDic);
        }
        for (int i = 0; i < layerList.Count; i++)
        {
            // 当前层
            MapLayerItem nowItem = layerList[i];
            // 下层
            MapLayerItem DownItem = i == 0 ? null : layerList[i - 1];
            if (DownItem != null)
            {
                nowItem.TopLineAdditional(DownItem.MapItemDic);
            }
        }
    }

    public void DrawLine(List<MapLayerItem> layerList, Transform lineObj, Transform lineNode)
    {
        if (layerList == null)
        {
            return;
        }

        for (int i = 0; i < layerList.Count; i++)
        {
            // 当前层
            MapLayerItem nowItem = layerList[i];

            foreach (var item in nowItem.MapItemDic)
            {
                item.Value.DrawLine(lineObj, lineNode);
            }
        }
    }

    public void ClearLine(Transform lineNode)
    {
        for (int i = 0; i < lineNode.childCount; i++)
        {
            GameObject.Destroy(lineNode.GetChild(i).gameObject);
        }
    }
}
