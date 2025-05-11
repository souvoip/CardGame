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

    private bool isDisableUIInteraction = false;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void DisableUIInteraction()
    {
        if (isDisableUIInteraction) { return; }
        isDisableUIInteraction = true;
        uiCanvasGroup.interactable = false;
        uiCanvasGroup.blocksRaycasts = false;
    }

    // ∆Ù”√UIΩªª•
    public void EnableUIInteraction()
    {
        if (!isDisableUIInteraction) { return; }
        isDisableUIInteraction = false;
        uiCanvasGroup.interactable = true;
        uiCanvasGroup.blocksRaycasts = true;
    }
}
