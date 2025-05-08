using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItemControl : MonoBehaviour
{
    public List<PotionSlot> potionSlots = new List<PotionSlot>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            potionSlots.Add(transform.GetChild(i).GetComponent<PotionSlot>());
        }
    }

    /// <summary>
    /// 添加药剂
    /// </summary>
    /// <param name="potionID"> 要添加的药剂ID </param>
    /// <returns> 如果成功添加了药水，则为True，如果库存已满，则为false </returns>
    public bool AddPotion(int potionID)
    {

        return false;
    }
}
