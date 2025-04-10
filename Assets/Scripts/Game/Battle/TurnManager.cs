using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static event Action OnPlayerTurnStart;
    public static event Action OnPlayerTurnEnd;
    public static event Action OnEnemyTurnStart;
    public static event Action OnEnemyTurnEnd;

    public static TurnManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayerTurnStart()
    {
        OnPlayerTurnStart?.Invoke();
    }

    // 示例回合流程
    public void PlayerTurnEnd()
    {
        OnPlayerTurnEnd?.Invoke();
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        OnEnemyTurnStart?.Invoke();
        // 敌人行动逻辑...
        yield return new WaitForSeconds(1);
        OnEnemyTurnEnd?.Invoke();
    }
}