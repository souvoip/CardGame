using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private TMP_Text apTxt;

    private void Awake()
    {
        turnOverBtn.onClick.AddListener(TurnOver);
        drawCardBtn.onClick.AddListener(() => ViewCard(ECardRegion.Draw));
        discardCardBtn.onClick.AddListener(() => ViewCard(ECardRegion.Discard));
        costCardBtn.onClick.AddListener(() => ViewCard(ECardRegion.Cost));

        EventCenter<int, int>.GetInstance().AddEventListener(EventNames.CHANGE_AP, ChangeAp);
    }

    private void OnDestroy()
    {
        EventCenter<int, int>.GetInstance().RemoveEventListener(EventNames.CHANGE_AP, ChangeAp);
    }

    private void ChangeAp(int currentAp, int maxAp)
    {
        apTxt.text = currentAp + "/" + maxAp;
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
                if(BattleManager.Instance.CardManager.DrawRegionCards.Count > 0)
                {
                    UIManager.Instance.cardView.Show(BattleManager.Instance.CardManager.DrawRegionCards);
                }
                else
                {
                    Debug.Log("DrawRegionCards is empty");
                }
                break;
            case ECardRegion.Discard:
                if (BattleManager.Instance.CardManager.DiscardRegionCards.Count > 0)
                {
                    UIManager.Instance.cardView.Show(BattleManager.Instance.CardManager.DiscardRegionCards);
                }
                else
                {
                    Debug.Log("DiscardRegionCards is empty");
                }
                break;
            case ECardRegion.Cost:
                if (BattleManager.Instance.CardManager.CostRegionCards.Count > 0)
                {
                    UIManager.Instance.cardView.Show(BattleManager.Instance.CardManager.CostRegionCards);
                }
                else
                {
                    Debug.Log("CostRegionCards is empty");
                }
                break;
        }
    }
}
