using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class FrameWorkEditor : Editor {

    [MenuItem("FrameWork/Create/SoundManage")]
    public static void CreatSever_SoundManage() {
        Transform[] allTObj = FindObjectsByType<Transform>(FindObjectsSortMode.None);
        List<SoundManager> allhm = new List<SoundManager>();
        for (int i = 0; i < allTObj.Length; i++) {
            SoundManager temp =  allTObj[i].GetComponent<SoundManager>();
            if (temp != null) { allhm.Add(temp); }
        }

        if (allhm.Count > 1) {
            Debug.LogError("场景中存在多个SoundManager");
            for (int i = 0; i < allhm.Count; i++)
            {
                Debug.LogError("SoundManager对象名称：" + allhm[i].gameObject.name);
            }
        } else if(allhm.Count <= 0) {
            GameObject http = new GameObject("SoundManager");
            http.AddComponent<SoundManager>();
            Debug.LogError("已成功创建一个名为 SoundManager 的 SoundManager 管理对象 ");
        } else { 
            Debug.LogError("场景有 SoundManager 对象");
        }
    }
}
