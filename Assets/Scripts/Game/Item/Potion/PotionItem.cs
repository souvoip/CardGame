using System;
using UnityEngine;
using UnityEngine.UI;

public class PotionItem : MonoBehaviour
{
    public PotionItemData potionItemData;

    private Action clearPotionAction;

    public void Init(PotionItemData potion, Action clearPotionAction)
    {
        potionItemData = potion;
        this.clearPotionAction = clearPotionAction;
        GetComponent<Image>().sprite = Resources.Load<Sprite>(ResourcesPaths.PotionImgPath + potionItemData.IconPath);
    }

    public void Use()
    {
        potionItemData.UseItem();
        clearPotionAction?.Invoke();
        Destroy(gameObject);
    }

    public void Use(CharacterBase target)
    {
        potionItemData.UseItem(target);
        clearPotionAction?.Invoke();
        Destroy(gameObject);
    }

    public void Drop()
    {
        clearPotionAction?.Invoke();
        Destroy(gameObject);
    }
}
