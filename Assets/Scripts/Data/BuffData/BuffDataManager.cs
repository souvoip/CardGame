using System;
using System.Collections.Generic;
using UnityEngine;

public static class BuffDataManager
{
    public static List<BuffBase> buffs = new List<BuffBase>();

    public static void Init()
    {
        buffs.AddRange(Resources.LoadAll<BuffBase>(ResourcesPaths.BuffDataPath));
    }

    public static BuffBase GetBuff(int id)
    {
        foreach (var b in buffs) { if (b.buffID == id) return GameObject.Instantiate(b); }
        return null;
    }

    public static string GetBuffName(int id)
    {
        foreach (var b in buffs) { if (b.buffID == id) return b.displayName; }
        return "未找到指定Buff";
    }
}
