using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardDataManager
{
    private static string cardDataPath = "CardData/CardData.json";

    private static JSONObject cardData;

    public static List<AtkCard> atkCards = new List<AtkCard>();

    public static void Init()
    {
        TextAsset cardDataText = Resources.Load<TextAsset>(cardDataPath);
        cardData = JSONObject.Create(cardDataText.text);
        for (int i = 0; i < cardData.Count; i++)
        {
            if((ECardType)cardData[i].GetField("Type").i == ECardType.Atk)
            {
                atkCards.Add(new AtkCard(cardData[i]));
            }
        }
    }



}
