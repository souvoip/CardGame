using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public PlayerRole Player;

    public List<EnemyRole> EnemyRoles;

    public Transform[] EnemySpawnPoint;

    public CardManager CardManager;

    public TurnManager TurnManager;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject damageNumberPrefab;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void StartBattle(int[] enemyIDs)
    {
        foreach (var enemy in EnemyRoles)
        {
            Destroy(enemy.gameObject);
        }
        EnemyRoles.Clear();

        for (int i = 0; i < enemyIDs.Length; i++)
        {
            var enemy = Instantiate(enemyPrefab, EnemySpawnPoint[i]).GetComponent<EnemyRole>();
            EnemyRoles.Add(enemy);
            enemy.transform.localPosition = Vector3.zero;
            enemy.SetEnemyData(enemyIDs[i]);
        }

        TimerTools.Timer.Once(0.5f, () =>
        {
            TurnManager.StartBattle();
            TurnManager.PlayerTurnStart();
            UIManager.Instance.mapUI.Hide();
        });
    }

    public void StartBattleTest()
    {
        // 清理
        foreach (var enemy in EnemyRoles)
        {
            Destroy(enemy.gameObject);
        }
        EnemyRoles.Clear();
        
        int enemyCount = Random.Range(1, 3);
        for (int i = 0; i < enemyCount; i++)
        {
            var enemy = Instantiate(enemyPrefab, EnemySpawnPoint[i]).GetComponent<EnemyRole>();
            EnemyRoles.Add(enemy);
            enemy.transform.localPosition = Vector3.zero;
            enemy.SetEnemyData(101);
        }
        CardManager.InitBattleCardData();

        TimerTools.Timer.Once(0.5f, () =>
        {
            TurnManager.StartBattle();
            TurnManager.PlayerTurnStart();
            UIManager.Instance.mapUI.Hide();
        });
    }

    public void TestBattle()
    {
        TurnManager.PlayerTurnStart();
    }

    public void EnemyDie(EnemyRole role)
    {
        EnemyRoles.Remove(role);
        Destroy(role.gameObject);
        if (EnemyRoles.Count == 0)
        {
            BattleOver(true);
        }
    }

    public void PlayerDie()
    {
        BattleOver(false);
    }

    public void BattleOver(bool isWin)
    {
        if (isWin)
        {
            // 清理玩家在战斗中获取的buff
            Player.ClearBattleBuff();
            // 胜利
            UIManager.Instance.selectCardUI.Show(CardDataManager.GetRandomCardIds(3), () =>
            {
                UIManager.Instance.mapUI.Show();
            });
        }
        else
        {
            // 先直接退出游戏 TODO：功能未完成
            Application.Quit();
        }
    }

    public void ShowDamageNumber(int damage, Vector3 position)
    {
        var damageNumber = Instantiate(damageNumberPrefab, transform).GetComponent<DamageNumber>();
        damageNumber.transform.position = position;
        damageNumber.Init(damage);
    }
}
