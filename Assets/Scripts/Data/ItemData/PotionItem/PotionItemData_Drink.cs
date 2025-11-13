using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionItemData_Drink", menuName = "Data/Item/Potion/PotionItemData_Drink")]
public class PotionItemData_Drink : PotionItemData
{
    public ERoleAttribute Attribute;

    public int ChangeValue;

    public override void UseItem()
    {
        // 对自己使用
        BattleManager.Instance.Player.ChangeAttribute(Attribute, ChangeValue);
    }

    public override void UseItem(CharacterBase target)
    {
    }
}
