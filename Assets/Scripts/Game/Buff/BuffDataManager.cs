using System.Collections.Generic;
using UnityEngine;

public static class BuffDataManager
{
    public static List<BuffBase> buffs = new List<BuffBase>();
    
    public static void Init()
    {
        var bs = Resources.LoadAll<BuffBase>("Data/Buff");
        foreach (var b in bs) { buffs.Add(b);}
        Debug.Log("Load Buffs: " + bs.Length);
    }

    public static BuffBase GetBuff(int id)
    {
        foreach (var b in buffs) { if (b.buffID == id) return GameObject.Instantiate(b); }
        return null;
    }
}
