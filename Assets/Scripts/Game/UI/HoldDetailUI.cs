using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldDetailUI : MonoBehaviour
{
    [SerializeField]
    private GameObject infoItemPrefab;

    [SerializeField]
    private float intervalY = 10;

    public void ShowInfos(string[] infos)
    {
        // 清理所有子对象
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        // 添加新的子对象
        float offsetY = 0;
        for (int i = 0; i < infos.Length; i++)
        {
            GameObject go = Instantiate(infoItemPrefab, transform);
            go.transform.localPosition = new Vector3(0, offsetY, 0);
            go.GetComponent<InfoItem>().SetInfo(infos[i]);
            LayoutRebuilder.ForceRebuildLayoutImmediate(go.transform as RectTransform);
            offsetY += go.GetComponent<RectTransform>().rect.height + intervalY;
        }

    }

    public void Hide()
    {

    }

    // Test =======
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowInfos(new string[] { "大师傅士大夫：阿三大苏打实打实的", "大师傅士大夫：阿三大苏打实打实的", "大师傅士大夫：阿三大苏打实打实的" });
        }
    }
}
