using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static event Action OnStartBattle;
    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;
    public static event Action OnEnemyTurnStart;
    public static event Action OnEnemyTurnEnd;
    public static event Action OnBattleVictory;
    public static ETurnType TurnType;
    public static TurnManager Instance;
    private static int currentTurnCount = 0;

    public static int CurrentTurnCount
    {
        get
        {
            return currentTurnCount;
        }
        set
        {
            currentTurnCount = value;
            UIManager.Instance.battleUI.SetTurnNumber(currentTurnCount);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        TurnType = ETurnType.NonBattle;
    }

    public void StartBattle()
    {
        TurnType = ETurnType.Player;
        CurrentTurnCount = 0;
        OnStartBattle?.Invoke();
    }

    public void PlayerTurnStart()
    {
        Debug.Log("Player Turn Start");
        CurrentTurnCount++;
        TurnType = ETurnType.Player;
        OnPlayerTurnStart?.Invoke();
    }

    // 敌人回合流程
    public void PlayerTurnEnd()
    {
        Debug.Log("Player Turn End");
        OnPlayerTurnEnd?.Invoke();
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn Start");
        TurnType = ETurnType.Enemy;
        OnEnemyTurnStart?.Invoke();
        // 敌人行动逻辑...
        for (int i = 0; i < BattleManager.Instance.EnemyRoles.Count; i++)
        {
            BattleManager.Instance.EnemyRoles[i].DoAction();
        }

        yield return new WaitForSeconds(1);
        OnEnemyTurnEnd?.Invoke();
        Debug.Log("Enemy Turn End");
        yield return new WaitForSeconds(1);
        PlayerTurnStart(); // 回到玩家回合
    }

    public void BattleVictory()
    {
        OnBattleVictory?.Invoke();
        TurnType = ETurnType.NonBattle;
    }
}

public enum ETurnType
{
    Player,
    Enemy,
    NonBattle,
}