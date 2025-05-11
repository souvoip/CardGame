using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldCancel : MonoBehaviour, IPointerEnterHandler
{
    public static event Action cancelAction;

    public void OnPointerEnter(PointerEventData eventData)
    {
        cancelAction?.Invoke();
    }
}
