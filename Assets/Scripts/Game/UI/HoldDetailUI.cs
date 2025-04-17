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

    [SerializeField]
    private float intervalX = 10;

    [SerializeField]
    private float maxHeight = 400;

    public void ShowInfos(string[] infos)
    {
        StartCoroutine(ShowInfosCoroutine(infos));
    }

    private IEnumerator ShowInfosCoroutine(string[] infos)
    {
        // 清理所有子对象
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        // 添加新的子对象
        for (int i = 0; i < infos.Length; i++)
        {
            GameObject go = Instantiate(infoItemPrefab, transform);
            go.GetComponent<InfoItem>().SetInfo(infos[i]);
            LayoutRebuilder.ForceRebuildLayoutImmediate(go.transform as RectTransform);
        }
        yield return null;
        float height = 0;
        float offsetX = 0;
        float offsetY = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(offsetX, offsetY, 0);
            offsetY -= transform.GetChild(i).GetComponent<RectTransform>().rect.height + intervalY;
            height += transform.GetChild(i).GetComponent<RectTransform>().rect.height + intervalY;
            if (height > maxHeight)
            {
                offsetX += transform.GetChild(i).GetComponent<RectTransform>().rect.width + intervalX;
                offsetY = 0;
                height = 0;
                transform.GetChild(i).localPosition = new Vector3(offsetX, offsetY, 0);
                offsetY -= transform.GetChild(i).GetComponent<RectTransform>().rect.height + intervalY;
                height += transform.GetChild(i).GetComponent<RectTransform>().rect.height + intervalY;
            }
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
            ShowInfos(new string[] { "大师傅士大夫\n阿三大苏打实打实的", "大师傅士大夫\n阿三大苏打实打实的", "大师傅士大夫\n阿三大苏打实打实的", "大师傅士大夫\n阿三大苏打实打实的", "大师傅士大夫\n阿三大苏打实打实的", "大师傅士\n 双方都是垫付多少粉丝地方让他我让他 双方都是巅峰赛的适当放松的方式的方式\n适当放松", "大师傅士大夫\n阿三大苏打实打实的", "大师傅士大夫\n阿三大苏打实打实的", "大师傅士大夫\n阿三大苏打实打实的", "大师傅士\n 双方都是垫付多少粉丝地方让他我让他 双方都是巅峰赛的适当放松的方式的方式\n适当放松" });
        }
    }
}
