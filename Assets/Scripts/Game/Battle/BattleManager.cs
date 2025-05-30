using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public PlayerRole Player;

    public List<EnemyRole> EnemyRoles;

    public Transform EnemySpawnParent;

    public CardManager CardManager;

    public TurnManager TurnManager;

    public EnemyRole nowSelectEnemy;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject damageNumberPrefab;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public void StartBattle(BattleData data)
    {
        foreach (var enemy in EnemyRoles)
        {
            Destroy(enemy.gameObject);
        }
        EnemyRoles.Clear();

        for (int i = 0; i < data.Enemies.Count; i++)
        {
            var enemy = Instantiate(enemyPrefab, EnemySpawnParent).GetComponent<EnemyRole>();
            enemy.Init();
            EnemyRoles.Add(enemy);
            enemy.transform.position = data.Enemies[i].Position;
            enemy.SetEnemyData(data.Enemies[i].EnemyID);
        }
        CardManager.InitBattleCardData();

        TimerTools.Timer.Once(0.5f, () =>
        {
            TurnManager.StartBattle();
            TurnManager.PlayerTurnStart();
            UIManager.Instance.mapUI.Hide();
        });
    }

    public void StartBattle(int BattleID)
    {
        StartBattle(BattleDataManager.GetBattle(BattleID));
    }

    public void StartBattleTest()
    {
        StartBattle(BattleDataManager.GetRandomBattle());
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
            TurnManager.BattleVictory();
            // 判断是否是最后一场战斗，TODO：功能未完成（先直接退出）
            if (UIManager.Instance.mapUI.CurrentMapItem.Type == EMapItemType.Boss)
            {
                Application.Quit();
                return;
            }
            // 生成奖励
            List<PrizeItemData> prizes = new List<PrizeItemData>();
            if (UIManager.Instance.mapUI.CurrentMapItem.Type == EMapItemType.Battle)
            {
                // 普通战斗奖励
                prizes.Add(new PrizeItemData(EPrizeType.Gold, Random.Range(20, 35)));
                prizes.Add(new PrizeItemData(EPrizeType.Card, 3));
            }
            else if (UIManager.Instance.mapUI.CurrentMapItem.Type == EMapItemType.HardBattle)
            {
                // 困难战斗奖励
                prizes.Add(new PrizeItemData(EPrizeType.Gold, Random.Range(30, 50)));
                prizes.Add(new PrizeItemData(EPrizeType.Card, 3));
                // 需要随机物品
                prizes.Add(new PrizeItemData(EPrizeType.Item, 2));
            }
            else if (UIManager.Instance.mapUI.CurrentMapItem.Type == EMapItemType.Boss)
            {
                // BOSS战斗奖励
                prizes.Add(new PrizeItemData(EPrizeType.Gold, Random.Range(50, 100)));
                prizes.Add(new PrizeItemData(EPrizeType.Card, 3));
                // 需要随机物品
                prizes.Add(new PrizeItemData(EPrizeType.Item, 2));
            }
            UIManager.Instance.prizeUI.Show(prizes);
            UIManager.Instance.mapUI.Show();
        }
        else
        {
            // 先直接退出游戏 TODO：功能未完成
            Application.Quit();
        }
    }

    public void ShowDamageNumber(int damage, Vector3 position)
    {
        if (damage == 0) { return; }
        var damageNumber = Instantiate(damageNumberPrefab, transform).GetComponent<DamageNumber>();
        damageNumber.transform.position = position;
        damageNumber.Init(damage);
    }
}
