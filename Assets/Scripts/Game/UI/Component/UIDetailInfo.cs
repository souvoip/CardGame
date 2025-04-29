using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetailInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private List<DetailInfo> detailInfo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.ShowInfos(transform.position, offset, detailInfo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.Hide();
    }
}
