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

    // 当前正在编辑的动作索引
    public int SelectedActionIndex = -1;

    [SerializeReference]
    public EnemyDoAction EditAction;

    public void InitActions(CharacterBase self)
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].self = self;
        }
    }

    public EnemyDoAction GetRandomAction()
    {
        return Actions[UnityEngine.Random.Range(0, Actions.Count)];
    }
}

