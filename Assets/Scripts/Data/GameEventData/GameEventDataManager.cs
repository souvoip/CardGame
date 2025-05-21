using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventDataManager
{
    public static List<GameEventData> events = new List<GameEventData>();

    public static void Init()
    {
        events.AddRange(Resources.LoadAll<GameEventData>(ResourcesPaths.GameEventDataPath));
    }

    public static GameEventData GetEvent(int id)
    {
        foreach (GameEventData eventData in events)
        {
            if (eventData.ID == id) { return eventData; }
        }
        return null;
    }

    public static GameEventData GetRandomEvent()
    {
        return events[Random.Range(0, events.Count)];
    }
}
