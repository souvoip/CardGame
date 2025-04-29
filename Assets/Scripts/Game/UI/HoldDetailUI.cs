using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldDetailUI : MonoBehaviour
{
    [SerializeField]
    private GameObject infoItemPrefab;

    [SerializeField]
    private CanvasGroup cg;

    [SerializeField]
    private float intervalX = 10;

    [SerializeField]
    private float intervalY = 10;

    [SerializeField]
    private float maxHeight = 400;

    private bool isShow = false;

    public void ShowInfos(Vector2 pos, Vector2 offset, List<DetailInfo> infos)
    {
        if(infos == null || infos.Count == 0) { return; }
        isShow = true;
        //gameObject.SetActive(true);
        float scale = Screen.width / 1920f;
        offset *= scale;
        StartCoroutine(ShowInfosCoroutine(pos, offset, infos));
    }

    private IEnumerator ShowInfosCoroutine(Vector2 pos, Vector2 offset, List<DetailInfo> infos)
    {
        transform.localScale = Vector3.one;
        // 清理所有子对象
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        // 添加新的子对象
        for (int i = 0; i < infos.Count; i++)
        {
            GameObject go = Instantiate(infoItemPrefab, transform);
            go.GetComponent<InfoItem>().SetInfo(infos[i]);
            LayoutRebuilder.ForceRebuildLayoutImmediate(go.transform as RectTransform);
        }
        yield return null;
        float height = 0;
        float offsetX = offset.x;
        float offsetY = offset.y;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(offsetX, offsetY, 0);
            offsetY -= transform.GetChild(i).GetComponent<RectTransform>().rect.height + intervalY;
            height += transform.GetChild(i).GetComponent<RectTransform>().rect.height + intervalY;
            if (height > maxHeight)
            {
                Debug.Log(transform.GetChild(i).GetComponent<RectTransform>().rect.width);
                Debug.Log(transform.GetChild(i).GetComponent<RectTransform>().rect.height);
                offsetX += transform.GetChild(i).GetComponent<RectTransform>().rect.width + intervalX;
                offsetY = 0;
                height = 0;
                transform.GetChild(i).localPosition = new Vector3(offsetX, offsetY, 0);
                offsetY -= transform.GetChild(i).GetComponent<RectTransform>().rect.height + intervalY;
                height += transform.GetChild(i).GetComponent<RectTransform>().rect.height + intervalY;
            }
        }
        // 计算调整UI位置，使其在屏幕内
        // Y轴
        float rHeight = 0;
        if (offsetX > offset.x) { rHeight = maxHeight; }
        else { rHeight = height; }
        // X轴
        float rWidth = offsetX + transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().rect.width;
        if (pos.x + rWidth > Screen.width)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localScale = new Vector3(-1, 1, 1);
                (transform.GetChild(i) as RectTransform).pivot = new Vector2(1, 1);
            }
        }

        Vector3 newPos = pos;
        if (pos.y - rHeight + offset.y < 0)
        {
            newPos.y -= pos.y - rHeight + offset.y;
        }
        transform.position = newPos;
        if (isShow)
        {
            cg.alpha = 1;
        }
    }

    public void Hide()
    {
        //gameObject.SetActive(false);
        isShow = false;
        cg.alpha = 0;
    }

    // Test =======
#if UNITY_EDITOR
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<DetailInfo> infos = new List<DetailInfo>();
            Sprite testSprite = Resources.Load<Sprite>("Image/BuffIcon/002");
            for (int i = 0; i < 10; i++)
            {
                DetailInfo info = new DetailInfo();
                info.Title = "测试" + i;
                info.Icon = testSprite;
                info.Description = "这是测试" + i;
                infos.Add(info);
            }
            ShowInfos(new Vector2(1600, 100), Vector2.zero, infos);
        }
    }
#endif
}
