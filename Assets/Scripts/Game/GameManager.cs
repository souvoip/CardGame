using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // 加载数据
        BuffDataManager.Init();
        CardDataManager.Init();
        CharacterDataManager.Init();
        ItemDataManager.Init();
        GameEventDataManager.Init();
        BattleDataManager.Init();
    }



    // Test ==== 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            string str = MapManager.Instance.Save().ToString();
            Debug.Log("Save: " + str);
        }
    }
}
