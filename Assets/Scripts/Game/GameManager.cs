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
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }

    private void SaveData()
    {
        JSONObject data = JSONObject.Create();
        data.AddField("MapData", MapManager.Instance.Save());
        data.AddField("CardData", BattleManager.Instance.CardManager.Save());
        data.AddField("PlayerData", BattleManager.Instance.Player.Save());
        SaveManager.SaveData(data.Print(false));
        Debug.Log("Save Data: " + data.Print(false));
    }

    private void LoadData()
    {
        JSONObject data = SaveManager.LoadData();
        MapManager.Instance.Load(data.GetField("MapData"));
        BattleManager.Instance.CardManager.Load(data.GetField("CardData"));
        BattleManager.Instance.Player.Load(data.GetField("PlayerData"));
    }
}
