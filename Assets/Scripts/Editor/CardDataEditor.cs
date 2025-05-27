using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AtkCard))]
public class AtkCardEditor : Editor
{
    private ECardActionType currentActionType;
    private CardBase cardData;

    private void OnEnable()
    {
        cardData = target as CardBase;
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

        if (GUILayout.Button("将当前数据保存到等级中"))
        {
            if(cardData.LevelDatas == null) { cardData.LevelDatas = new List<CardLevelData>(); }
            CardLevelData cardLevelData = new CardLevelData();
            cardLevelData.UName = cardData.Name;
            cardLevelData.UDesc = cardData.Desc;
            cardLevelData.UFee = cardData.Fee;
            cardLevelData.UPrice = cardData.Price;
            cardLevelData.UCardActions = new List<CardAction>();
            foreach (var action in cardData.CardActions)
            {
                cardLevelData.UCardActions.Add(action.Clone() as CardAction);
            }
            cardLevelData.UCardAnimData = new BattleAnimData() { path = cardData.CardAnimData.path , time = cardData.CardAnimData.time };
            cardLevelData.UImagePath = cardData.ImagePath;
            cardData.LevelDatas.Add(cardLevelData);
            cardData.MaxLevel = cardData.LevelDatas.Count;
        }
    }
}


[CustomEditor(typeof(SkillCard))]
public class SkillCardEditor : AtkCardEditor
{
    
}


[CustomEditor(typeof(AbilityCard))]
public class AbilityCardEditor : AtkCardEditor
{
    
}


[CustomEditor(typeof(StateCard))]
public class StateCardEditor : AtkCardEditor
{
   
}
