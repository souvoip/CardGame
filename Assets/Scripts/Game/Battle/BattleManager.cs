using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public PlayerRole Player;

    public List<EnemyRole> EnemyRoles;

    public CardManager CardManager;

    public TurnManager TurnManager;

    

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void StartBattle(List<EnemyRole> enemyRoles)
    {
        EnemyRoles = enemyRoles;
    }

    public void TestBattle()
    {
        TurnManager.Instance.PlayerTurnStart();
    }
}
