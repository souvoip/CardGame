using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimItem : MonoBehaviour
{
    private static string basePath = "Anim/";

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <returns> 动画时间 </returns>
    public float PlayAnim(Vector3 pos, BattleAnimData animData)
    {
        transform.position = pos;
        // 加载动画
        GameObject animObj = Instantiate(Resources.Load<GameObject>(basePath + animData.path), transform);
        animObj.transform.localPosition = Vector3.zero;
        TimerTools.Timer.Once(animData.time, () => Destroy(gameObject));
        return animData.time;
    }
    
}

[Serializable]
public class BattleAnimData
{
    public string path;
    public float time;
}
