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
        serializedObject.Update();
        // 绘制类型选择下拉菜单
        DrawTypeSelector();

        // 绘制具体属性
        DrawActionProperties();
        AddActionBtn();

        // 绘制可交互的动作列表
        DrawInteractiveActionList();

        // 绘制选中动作的详细信息
        DrawSelectedActionDetails();

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
            EEnemyActionType.GiveCard => new EnemyGiveCardAction(),
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

    private void DrawInteractiveActionList()
    {
        EditorGUILayout.LabelField("Actions List", EditorStyles.boldLabel);

        for (int i = 0; i < actionsProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // 显示条目内容
            var action = actionsProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(action, new GUIContent($"Action {i}"), false);

            // 选择按钮
            if (GUILayout.Button("Select", GUILayout.Width(60)))
            {
                selectedIndexProperty.intValue = i;
            }

            // 删除按钮
            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                actionsProperty.DeleteArrayElementAtIndex(i);
                selectedIndexProperty.intValue = -1;
            }

            EditorGUILayout.EndHorizontal();
        }

        // 添加新动作按钮
        if (GUILayout.Button("Add New Action"))
        {
            actionsProperty.arraySize++;
            selectedIndexProperty.intValue = actionsProperty.arraySize - 1;
        }
    }

    private void DrawSelectedActionDetails()
    {
        if (selectedIndexProperty.intValue < 0 ||
            selectedIndexProperty.intValue >= actionsProperty.arraySize)
            return;

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Selected Action Details", EditorStyles.boldLabel);

        var selectedAction = actionsProperty.GetArrayElementAtIndex(selectedIndexProperty.intValue);

        // 遍历并绘制所有子属性
        var iterator = selectedAction.Copy();
        var enterChildren = true;
        while (iterator.NextVisible(enterChildren))
        {
            EditorGUILayout.PropertyField(iterator, true);
            enterChildren = false;
        }
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