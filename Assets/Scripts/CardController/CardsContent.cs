using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsContent : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    private List<CardController> cards = new List<CardController>();

    private float cardRotationOffset = -5f;

    private void Awake()
    {
        CreateCards();
        UpdateCardRotation();
    }

    private void CreateCards()
    {
        for (int i = 0; i < 10; i++)
        {
            var card = Instantiate(cardPrefab, transform);
            var cc = card.GetComponent<CardController>();
            cards.Add(cc);
            cc.SetAction(SelectedCard, UnselectedCard);
        }
    }

    private void UpdateCardRotation()
    {
        int cardCount = cards.Count;
        float middleIndex = (cardCount - 1) / 2f; // 计算中间位置

        for (int i = 0; i < cardCount; i++)
        {
            float rotationZ = (i - middleIndex) * cardRotationOffset;
            cards[i].transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        }
    }

    /// <summary>
    /// 选中卡片，将卡片置于最上层
    /// </summary>
    /// <param name="card"></param>
    private void SelectedCard(CardController card)
    {
        card.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 取消选中卡片, 将卡片恢复到原来的位置
    /// </summary>
    /// <param name="card"></param>
    private void UnselectedCard(CardController card)
    {
        card.transform.SetSiblingIndex(cards.IndexOf(card));
    }
}
