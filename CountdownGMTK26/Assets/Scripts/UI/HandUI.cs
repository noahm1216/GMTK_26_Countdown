using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    public static HandUI Instance { get; private set; }

    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handParent;

    [SerializeField] private RectTransform drawPilePoint;
    [SerializeField] private RectTransform discardPilePoint;

    private Dictionary<CardInstance, CardUI> activeCards = new();

    [Header("Hand Layout")]
    [SerializeField] private float cardSpacing = 220f;
    [SerializeField] private float animationSpeed = 10f;
    [SerializeField] private float maxRotation = 8f;


    public void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void Refresh()
    {
        List<CardInstance> currentHand = deckManager.Hand;


        // Remove cards no longer in hand
        List<CardInstance> cardsToRemove = new();


        foreach (var pair in activeCards)
        {
            if (!currentHand.Contains(pair.Key))
            {
                Destroy(pair.Value.gameObject);
                cardsToRemove.Add(pair.Key);
            }
        }


        foreach (CardInstance card in cardsToRemove)
        {
            activeCards.Remove(card);
        }


        // Add new cards
        // Add new cards
        foreach (CardInstance card in currentHand)
        {
            if (!activeCards.ContainsKey(card))
            {
                GameObject obj = Instantiate(cardPrefab, handParent);

                CardUI ui = obj.GetComponent<CardUI>();

                ui.InitializeCard(card, gameManager);

                // Spawn at draw pile
                ui.SetInstantPosition(drawPilePoint.position);

                activeCards.Add(card, ui);
            }
        }

        // Update every card once
        UpdateCardPositions();
    }

    public Vector3 DrawPilePosition => drawPilePoint.position;

    public Vector3 DiscardPilePosition => discardPilePoint.position;

    public void RemoveCard(CardInstance card)
    {
        if (activeCards.TryGetValue(card, out CardUI ui))
        {
            activeCards.Remove(card);
            Destroy(ui.gameObject);
        }

        UpdateCardPositions();
    }

    private void UpdateCardPositions()
    {
        int count = deckManager.Hand.Count;

        if (count == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            CardInstance instance = deckManager.Hand[i];

            if (!activeCards.TryGetValue(instance, out CardUI card))
                continue;

            float x =
                (i - (count - 1) * 0.5f) * cardSpacing;

            float t =
                count == 1
                ? .5f
                : (float)i / (count - 1);

            float rotation =
                Mathf.Lerp(-maxRotation, maxRotation, t);

            card.MoveTo(
                new Vector3(x, 0, 0),
                rotation);
        }
    }
}