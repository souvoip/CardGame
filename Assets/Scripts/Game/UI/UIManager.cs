using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public CanvasGroup uiCanvasGroup;

    public BattleUI battleUI;

    public CardViewUI cardView;

    public HoldDetailUI holdDetailUI;

    public MapManager mapUI;

    public SelectCardUI selectCardUI;

    public GameTopUI gameTopUI;

    public PotionOptionUI potionOptionUI;

    public GameEventUI gameEventUI;

    public ShopUI shopUI;

    public RestUI restUI;

    public PrizeUI prizeUI;

    public GameWinUI winUI;

    public SceneTransition sceneTransitionUI;
    
    private bool isDisableUIInteraction = false;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        TimerTools.Timer.Once(0.3f, () =>
        {
            GameManager.Instance.StartGame();
            sceneTransitionUI.FadeOut();
        });
    }

    /// <summary>
    /// 禁用UI交互
    /// </summary>
    public void DisableUIInteraction()
    {
        if (isDisableUIInteraction) { return; }
        isDisableUIInteraction = true;
        uiCanvasGroup.interactable = false;
        uiCanvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 启用UI交互
    /// </summary>
    public void EnableUIInteraction()
    {
        if (!isDisableUIInteraction) { return; }
        isDisableUIInteraction = false;
        uiCanvasGroup.interactable = true;
        uiCanvasGroup.blocksRaycasts = true;
    }
}
