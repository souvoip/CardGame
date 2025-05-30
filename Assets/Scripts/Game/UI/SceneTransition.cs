using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public float transitionTime = 1f; // 转场时长

    [SerializeField]
    private CanvasGroup canvasGroup;

    public void Start()
    {
        //canvasGroup.alpha = 0f;
    }

    public void SceneChange(UIBase nextScene, System.Action callback = null)
    {
        canvasGroup.blocksRaycasts = true;
        // 隐藏当前场景, 禁用点击事件
        canvasGroup.DOFade(1, transitionTime).OnComplete(() =>
        {
            nextScene.Show();
            canvasGroup.DOFade(0, transitionTime).OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = false;
                callback?.Invoke();
            });
        });
    }

    public void FadeIn(System.Action callback = null)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, transitionTime).OnComplete(() => { callback?.Invoke(); });
    }

    public void FadeOut(System.Action callback = null)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, transitionTime).OnComplete(() => { callback?.Invoke(); canvasGroup.blocksRaycasts = false; });
    }
}