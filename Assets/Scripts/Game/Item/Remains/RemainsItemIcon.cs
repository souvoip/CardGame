using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemainsItemIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static string remainsIconPath = "Image/Item/Remains/";

    [SerializeField]
    private Image iconImg;
    [SerializeField]
    private TMP_Text countTxt;

    public RemainsItem itemData;

    public void SetData(RemainsItem item)
    {
        itemData = item;
        countTxt.gameObject.SetActive(false);
        //if(count == 0)
        //{
        //    countTxt.gameObject.SetActive(false);
        //}
        //else
        //{
        //    countTxt.gameObject.SetActive(true);
        //    countTxt.text = count.ToString();
        //}
        iconImg.sprite = Resources.Load<Sprite>(remainsIconPath + item.IconPath);
    }

    public void UpdateCount(int count)
    {
        if (count == 0)
        {
            countTxt.gameObject.SetActive(false);
        }
        else
        {
            countTxt.gameObject.SetActive(true);
            countTxt.text = count.ToString();
        }
    }
    [SerializeField]
    private Vector2 tempInfoOffset;
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.ShowInfos(transform.position, tempInfoOffset, itemData.GetDetailinfo());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.Hide();
    }
}
