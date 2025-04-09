using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffControl : MonoBehaviour
{
    public GameObject buffIconPrefab;

    public Transform buffIconContainer;

    public List<BuffBase> activeBuffs = new List<BuffBase>();

    private CharacterBase character;

    private List<BuffIcon> buffIcons = new List<BuffIcon>();

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
            buff.Initialize(character, stracks);
            activeBuffs.Add(buff);
        }

        UpdateBuffUI(buff.buffID);
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

    // UI更新(更新所有Buff)
    private void UpdateBuffUI()
    {
        // 使用副本遍历避免集合修改异常
        var currentActiveBuffs = activeBuffs.ToList();
        var activeBuffIds = new HashSet<int>();

        foreach (var buff in currentActiveBuffs)
        {
            activeBuffIds.Add(buff.buffID); // 收集所有活跃ID
            var bi = buffIcons.Find(b => b.BuffId == buff.buffID);
            if (bi != null)
            {
                bi.UpdateStacks(buff.currentStacks);
            }
            else
            {
                BuffIcon newIcon = Instantiate(buffIconPrefab, buffIconContainer).GetComponent<BuffIcon>();
                newIcon.UpdateIcon(buff.buffID, buff.iconPath, buff.currentStacks);
                buffIcons.Add(newIcon);
            }
        }
        // 移除不存在于activeBuffIds中的图标
        for (int i = buffIcons.Count - 1; i >= 0; i--)
        {
            if (!activeBuffIds.Contains(buffIcons[i].BuffId))
            {
                Destroy(buffIcons[i].gameObject);
                buffIcons.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// UI更新(更新指定Buff)
    /// </summary>
    /// <param name="buffId"></param>
    private void UpdateBuffUI(int buffId)
    {
        var buff = activeBuffs.Find(b => b.buffID == buffId);
        if (buff != null)
        {
            var bi = buffIcons.Find(b => b.BuffId == buffId);
            if (bi != null)
            {
                bi.UpdateStacks(buff.currentStacks);
            }
            else
            {
                BuffIcon newIcon = Instantiate(buffIconPrefab, buffIconContainer).GetComponent<BuffIcon>();
                newIcon.UpdateIcon(buff.buffID, buff.iconPath, buff.currentStacks);
                buffIcons.Add(newIcon);
            }
        }
    }
}