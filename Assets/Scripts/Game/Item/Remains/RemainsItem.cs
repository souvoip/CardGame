using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemainsItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image iconImg;
    [SerializeField]
    private TMP_Text countTxt;

    public RemainsItemData itemData;

    public void SetData(RemainsItemData item)
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
        iconImg.sprite = Resources.Load<Sprite>(ResourcesPaths.RemainsImgPath + item.IconPath);
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
        UIManager.Instance.holdDetailUI.ShowInfos(transform.position, tempInfoOffset, itemData.GetDetailInfo());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.holdDetailUI.Hide();
    }
}
