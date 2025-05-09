using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public BattleUI battleUI;

    public CardViewUI cardView;

    public HoldDetailUI holdDetailUI;

    public MapManager mapUI;

    public SelectCardUI selectCardUI;

    public GameTopUI gameTopUI;

    public PotionOptionUI potionOptionUI;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
}
