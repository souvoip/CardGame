using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AtkCard))]
public class AtkCardEditor : Editor
{
    private ECardActionType currentActionType;
    private AtkCard cardData;

    private void OnEnable()
    {
        cardData = target as AtkCard;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(20);
        GUILayout.Label("EDIT ACTIONS ======================");
        serializedObject.Update();

        DrawTypeSelector();
        AddActionBtn();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTypeSelector()
    {
        EditorGUI.BeginChangeCheck();

        // 获取当前类型或默认值
        currentActionType = (ECardActionType)EditorGUILayout.EnumPopup(
            "Action Type",
            currentActionType
        );

        if (EditorGUI.EndChangeCheck())
        {
        }
    }

    private void AddActionBtn()
    {
        if (GUILayout.Button("Add Action"))
        {
            if (cardData.CardActions == null) { cardData.CardActions = new List<CardAction>(); }
            switch (currentActionType)
            {
                case ECardActionType.Damage:
                    cardData.CardActions.Add(new CardDamageAction());
                    break;
                case ECardActionType.Defend:
                    cardData.CardActions.Add(new CardDefendAction());
                    break;
                case ECardActionType.Buff:
                    cardData.CardActions.Add(new CardBuffAction());
                    break;
                case ECardActionType.CardExtract:
                    cardData.CardActions.Add(new CardExtractAction());
                    break;

            }
        }
    }
}


[CustomEditor(typeof(SkillCard))]
public class SkillCardEditor : Editor
{
    private ECardActionType currentActionType;
    private SkillCard cardData;

    private void OnEnable()
    {
        cardData = target as SkillCard;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(20);
        GUILayout.Label("EDIT ACTIONS ======================");
        serializedObject.Update();

        DrawTypeSelector();
        AddActionBtn();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTypeSelector()
    {
        EditorGUI.BeginChangeCheck();

        // 获取当前类型或默认值
        currentActionType = (ECardActionType)EditorGUILayout.EnumPopup(
            "Action Type",
            currentActionType
        );

        if (EditorGUI.EndChangeCheck())
        {
        }
    }

    private void AddActionBtn()
    {
        if (GUILayout.Button("Add Action"))
        {
            if (cardData.CardActions == null) { cardData.CardActions = new List<CardAction>(); }
            switch (currentActionType)
            {
                case ECardActionType.Damage:
                    cardData.CardActions.Add(new CardDamageAction());
                    break;
                case ECardActionType.Defend:
                    cardData.CardActions.Add(new CardDefendAction());
                    break;
                case ECardActionType.Buff:
                    cardData.CardActions.Add(new CardBuffAction());
                    break;
                case ECardActionType.CardExtract:
                    cardData.CardActions.Add(new CardExtractAction());
                    break;

            }
        }
    }
}


[CustomEditor(typeof(AbilityCard))]
public class AbilityCardEditor : Editor
{
    private ECardActionType currentActionType;
    private AbilityCard cardData;

    private void OnEnable()
    {
        cardData = target as AbilityCard;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(20);
        GUILayout.Label("EDIT ACTIONS ======================");
        serializedObject.Update();

        DrawTypeSelector();
        AddActionBtn();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTypeSelector()
    {
        EditorGUI.BeginChangeCheck();

        // 获取当前类型或默认值
        currentActionType = (ECardActionType)EditorGUILayout.EnumPopup(
            "Action Type",
            currentActionType
        );

        if (EditorGUI.EndChangeCheck())
        {
        }
    }

    private void AddActionBtn()
    {
        if (GUILayout.Button("Add Action"))
        {
            if (cardData.CardActions == null) { cardData.CardActions = new List<CardAction>(); }
            switch (currentActionType)
            {
                case ECardActionType.Damage:
                    cardData.CardActions.Add(new CardDamageAction());
                    break;
                case ECardActionType.Defend:
                    cardData.CardActions.Add(new CardDefendAction());
                    break;
                case ECardActionType.Buff:
                    cardData.CardActions.Add(new CardBuffAction());
                    break;
                case ECardActionType.CardExtract:
                    cardData.CardActions.Add(new CardExtractAction());
                    break;

            }
        }
    }
}


[CustomEditor(typeof(StateCard))]
public class StateCardEditor : Editor
{
    private ECardActionType currentActionType;
    private StateCard cardData;

    private void OnEnable()
    {
        cardData = target as StateCard;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(20);
        GUILayout.Label("EDIT ACTIONS ======================");
        serializedObject.Update();

        DrawTypeSelector();
        AddActionBtn();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTypeSelector()
    {
        EditorGUI.BeginChangeCheck();

        // 获取当前类型或默认值
        currentActionType = (ECardActionType)EditorGUILayout.EnumPopup(
            "Action Type",
            currentActionType
        );

        if (EditorGUI.EndChangeCheck())
        {
        }
    }

    private void AddActionBtn()
    {
        if (GUILayout.Button("Add Action"))
        {
            if (cardData.CardActions == null) { cardData.CardActions = new List<CardAction>(); }
            switch (currentActionType)
            {
                case ECardActionType.Damage:
                    cardData.CardActions.Add(new CardDamageAction());
                    break;
                case ECardActionType.Defend:
                    cardData.CardActions.Add(new CardDefendAction());
                    break;
                case ECardActionType.Buff:
                    cardData.CardActions.Add(new CardBuffAction());
                    break;
                case ECardActionType.CardExtract:
                    cardData.CardActions.Add(new CardExtractAction());
                    break;

            }
        }
    }
}
