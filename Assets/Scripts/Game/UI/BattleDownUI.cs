using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDownUI : MonoBehaviour
{
    [SerializeField]
    private Button turnOverBtn;
    [SerializeField]
    private Button drawCardBtn;
    [SerializeField]
    private Button discardCardBtn;
    [SerializeField]
    private Button costCardBtn;

    private void Awake()
    {
        turnOverBtn.onClick.AddListener(TurnOver);
        drawCardBtn.onClick.AddListener(() => ViewCard(ECardRegion.Draw));
        discardCardBtn.onClick.AddListener(() => ViewCard(ECardRegion.Discard));
        costCardBtn.onClick.AddListener(() => ViewCard(ECardRegion.Cost));
    }

    private void TurnOver()
    {
        if(TurnManager.TurnType == ETurnType.Player)
        {
            TurnManager.Instance.PlayerTurnEnd();
        }
    }

    private void ViewCard(ECardRegion region)
    {
        switch (region)
        {
            case ECardRegion.Draw:
                break;
            case ECardRegion.Discard:
                break;
            case ECardRegion.Cost:
                break;
        }
    }
}
