using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotionSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PotionItem potionItem;

    public bool isEmpty = true;

    public void AddPotion(int potionId, GameObject potionPrefab)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
