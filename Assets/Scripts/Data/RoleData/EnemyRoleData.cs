using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRoleData", menuName = "Data/Character/EnemyRoleData")]
public class EnemyRoleData : RoleData
{
    [SerializeReference]
    public List<EnemyDoAction> Actions = new List<EnemyDoAction>();

    /// <summary>
    /// 行动是否随机，如果是，则随机选择一个行动
    /// </summary>
    [SerializeField]
    [Header("行动是否随机")]
    private bool ActionIsRandom;

    private int currentActionIndex = 0;

    [SerializeReference]
    public EnemyDoAction EditAction;

    public void InitActions(CharacterBase self)
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].self = self;
        }
    }

    public EnemyDoAction GetEnemyAction()
    {
        if (ActionIsRandom)
        {
            return Actions[UnityEngine.Random.Range(0, Actions.Count)];
        }
        else
        {
            return Actions[currentActionIndex++ % Actions.Count];
        }
    }
}

