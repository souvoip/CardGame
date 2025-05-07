using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainsItemManager : MonoBehaviour
{
    [SerializeField]
    private GameObject remainsItemPrefab;

    private List<RemainsItemIcon> remainsItemIcons = new List<RemainsItemIcon>();

    public void AddRemainsItemIcon(RemainsItem itemData)
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
        RemainsItemIcon remainsItemIcon = remainsItemIconObj.GetComponent<RemainsItemIcon>();
        remainsItemIcon.SetData(itemData);
        remainsItemIcons.Add(remainsItemIcon);
    }
}
