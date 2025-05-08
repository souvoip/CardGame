using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainsItemControl : MonoBehaviour
{
    [SerializeField]
    private GameObject remainsItemPrefab;

    private List<RemainsItem> remainsItemIcons = new List<RemainsItem>();

    public void AddRemainsItemIcon(RemainsItemData itemData)
    {
        for(int i = 0; i < remainsItemIcons.Count; i++)
        {
            if(remainsItemIcons[i].itemData.ID == itemData.ID)
            {
                Debug.Log("RemainsItemIcon already exists");
                return;
            }
        }
        GameObject remainsItemIconObj = Instantiate(remainsItemPrefab, transform);
        RemainsItem remainsItemIcon = remainsItemIconObj.GetComponent<RemainsItem>();
        remainsItemIcon.SetData(itemData);
        remainsItemIcons.Add(remainsItemIcon);
    }
}
