using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isLoadGame = false;

    public static GameManager Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        // 加载数据
        BuffDataManager.Init();
        CardDataManager.Init();
        CharacterDataManager.Init();
        ItemDataManager.Init();
        GameEventDataManager.Init();
        BattleDataManager.Init();
    }

    // Test ==== 
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        SaveData();
    //    }
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        LoadData();
    //    }
    //}

    public void StartGame()
    {
        if (isLoadGame)
        {
            LoadGame();
        }
        else
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        BattleManager.Instance.CardManager.InitGameCardData();
        BattleManager.Instance.Player.Init();
        MapManager.Instance.CreatorMap();
    }

    private void LoadGame()
    {
        var data = LoadData();
        BattleManager.Instance.CardManager.InitGameCardData(data.GetField("CardData"));
        BattleManager.Instance.Player.Load(data.GetField("PlayerData"));
        MapManager.Instance.Load(data.GetField("MapData"));

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

    private JSONObject LoadData()
    {
        return SaveManager.LoadData();
    }

    private void OnApplicationQuit()
    {
        if(BattleManager.Instance == null) { return; }
        SaveData();
    }
}
