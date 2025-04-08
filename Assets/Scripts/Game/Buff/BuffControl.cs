using System.Collections.Generic;
using UnityEngine;

public class BuffControl : MonoBehaviour
{
    public List<BuffBase> activeBuffs = new List<BuffBase>();

    private CharacterBase character;

    private void Awake()
    {
        character = GetComponent<CharacterBase>();
    }

    // 应用新Buff
    public void ApplyBuff(BuffBase buff, int stracks)
    {
        // 检查是否存在同类Buff
        var existingBuff = activeBuffs.Find(b => b.buffID == buff.buffID);

        if (existingBuff != null)
        {
            existingBuff.AddStacks(stracks);
        }
        else
        {
            buff.Initialize(character);
            activeBuffs.Add(buff);
        }

        UpdateBuffUI();
    }

    // 回合结束处理
    public void OnTurnEnd()
    {
        foreach (var buff in activeBuffs.ToArray())
        {
            if (buff.duration > 0)
                buff.OnTurnEnd();
        }
    }

    // UI更新
    private void UpdateBuffUI()
    {
        //BuffUIManager.Instance.UpdateBuffs(activeBuffs);
    }
}