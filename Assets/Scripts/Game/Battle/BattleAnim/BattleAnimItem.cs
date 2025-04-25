using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimItem : MonoBehaviour
{
    private static string basePath = "Anim/";

    private BattleAnimData animData;

    public void Init(int id, Vector3 pos)
    {
        transform.position = pos;
        //animData = BattleAnimManager.Instance.GetAnimData(id);
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <returns> 动画时间 </returns>
    public float PlayAnim()
    {
        // 加载动画
        GameObject animObj = Instantiate(Resources.Load<GameObject>(basePath + animData.path), transform);
        animObj.transform.localPosition = Vector3.zero;
        return animData.time;
    }
    
}

public class BattleAnimData
{
    public int id;
    public string name;
    public string path;
    public float time;
}
