using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyRoleData))]
public class EnemyRoleDataEditor : Editor
{
    private SerializedProperty editActionProperty;
    private EEnemyActionType currentActionType;

    private SerializedProperty actionsProperty;
    private SerializedProperty selectedIndexProperty;

    private EnemyRoleData enemyRoleData;

    private void OnEnable()
    {
        actionsProperty = serializedObject.FindProperty("Actions");
        selectedIndexProperty = serializedObject.FindProperty("SelectedActionIndex");
        editActionProperty = serializedObject.FindProperty("EditAction");
        enemyRoleData = target as EnemyRoleData;
        UpdateCurrentActionType();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(20);
        GUILayout.Label("EDIT ACTIONS ======================");

        serializedObject.Update();
        // 绘制类型选择下拉菜单
        DrawTypeSelector();

        // 绘制具体属性
        DrawActionProperties();
        AddActionBtn();


        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTypeSelector()
    {
        EditorGUI.BeginChangeCheck();

        // 获取当前类型或默认值
        var newType = (EEnemyActionType)EditorGUILayout.EnumPopup(
            "Action Type",
            currentActionType
        );

        if (EditorGUI.EndChangeCheck())
        {
            ChangeActionType(newType);
            UpdateCurrentActionType();
        }
    }

    private void DrawActionProperties()
    {
        if (editActionProperty.managedReferenceValue == null) return;

        // 使用子类的真实类型获取序列化属性
        var iterator = editActionProperty.Copy();
        var enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            if (iterator.name == "ActionType") continue; // 跳过类型字段
            EditorGUILayout.PropertyField(iterator, true);
            enterChildren = false;
        }
    }

    private void ChangeActionType(EEnemyActionType newType)
    {
        // 根据类型创建对应实例
        object newAction = newType switch
        {
            EEnemyActionType.Attack => new EnemyAttackAction(),
            EEnemyActionType.GetAesist => new EnemyGetAesistAction(),
            EEnemyActionType.GetBuff => new EnemyGetBuffAction(),
            EEnemyActionType.GiveBuff => new EnemyGiveBuffAction(),
            EEnemyActionType.Summon => new EnemySummonAction(),
            EEnemyActionType.GiveCards => new EnemyGiveCardAction(),
            _ => throw new ArgumentOutOfRangeException()
        };

        editActionProperty.managedReferenceValue = newAction;
        currentActionType = newType;
    }

    private void UpdateCurrentActionType()
    {
        currentActionType = (editActionProperty.managedReferenceValue as EnemyDoAction)?.ActionType
                             ?? EEnemyActionType.Attack;
    }

    private void AddActionBtn()
    {
        if (GUILayout.Button("Add Current EditAction"))
        {
            if (enemyRoleData.Actions == null) { enemyRoleData.Actions = new List<EnemyDoAction>(); }
            enemyRoleData.Actions.Add(enemyRoleData.EditAction);
            enemyRoleData.EditAction = new EnemyAttackAction();
        }
    }
}