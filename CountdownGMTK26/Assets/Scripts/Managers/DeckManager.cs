using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField]
    private List<CardData> startingDeck = new();

    public List<CardInstance> Hand { get; private set; } = new();

    private List<CardInstance> drawPile = new();
    private List<CardInstance> discardPile = new();

    private const int HAND_SIZE = 7;
    private const int REFILL_THRESHOLD = 3;

    void Start()
    {
        BuildDeck();
    }

    void BuildDeck()
    {
        drawPile.Clear();

        foreach (CardData card in startingDeck)
            drawPile.Add(new CardInstance(card));

        Shuffle(drawPile);

        DrawToHand(HAND_SIZE);

        Debug.Log($"Starting Hand: {Hand.Count}");
    }

    public void PlayCard(CardInstance card)
    {
        Hand.Remove(card);
        discardPile.Add(card);

        if (Hand.Count <= REFILL_THRESHOLD)
            RefillHand();
    }

    void ApplyEffect(CardInstance card, TimerManager timer)
    {
        switch (card.Data.effect)
        {
            case CardEffectType.AddTime:
                timer.AddTime(card.Data.value);
                break;

            case CardEffectType.RemoveTime:
                timer.RemoveTime(card.Data.value);
                break;

            case CardEffectType.MultiplyTime:
                timer.MultiplyTime(card.Data.value);
                break;

            case CardEffectType.DivideTime:
                timer.DivideTime(card.Data.value);
                break;

            case CardEffectType.IncreaseDrainRate:
                timer.IncreaseDrain(card.Data.value);
                break;

            case CardEffectType.DecreaseDrainRate:
                timer.DecreaseDrain(card.Data.value);
                break;
        }

        Debug.Log($"Played {card.Data.cardName}");
    }

    void DrawToHand(int targetSize)
    {
        while (Hand.Count < targetSize)
        {
            if (drawPile.Count == 0)
                ReshuffleDiscard();

            if (drawPile.Count == 0)
                return;

            Hand.Add(drawPile[0]);
            drawPile.RemoveAt(0);
        }
    }

    void RefillHand()
    {
        drawPile.AddRange(Hand);

        Hand.Clear();

        Shuffle(drawPile);

        DrawToHand(HAND_SIZE);

        Debug.Log("Hand Refilled");
    }

    void ReshuffleDiscard()
    {
        drawPile.AddRange(discardPile);

        discardPile.Clear();

        Shuffle(drawPile);

        Debug.Log("Deck Reshuffled");
    }

    void Shuffle(List<CardInstance> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int random = Random.Range(0, i + 1);

            (list[i], list[random]) = (list[random], list[i]);
        }
    }
}
