using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Starting Deck")]
    [SerializeField]
    private List<CardData> startingDeck = new();

    public List<CardInstance> Hand { get; private set; } = new();

    private readonly List<CardInstance> drawPile = new();
    private readonly List<CardInstance> discardPile = new();

    private const int HAND_SIZE = 7;
    private const int REFILL_THRESHOLD = 3;

    public event Action OnDeckChanged;

    public int DrawCount => drawPile.Count;
    public int HandCount => Hand.Count;
    public int DiscardCount => discardPile.Count;

    public bool CanRefill => Hand.Count <= REFILL_THRESHOLD;

    public void BuildDeck()
    {
        Hand.Clear();
        drawPile.Clear();
        discardPile.Clear();

        foreach (CardData card in startingDeck)
        {
            drawPile.Add(new CardInstance(card));
        }

        Shuffle(drawPile);

        DrawHand();

        Debug.Log("Deck Built");

        NotifyDeckChanged();
    }

    public void PlayCard(CardInstance card)
    {
        if (!Hand.Contains(card))
            return;

        Hand.Remove(card);
        discardPile.Add(card);

        // Only reshuffle once every card has been played.
        if (drawPile.Count == 0 && Hand.Count == 0)
        {
            ReshuffleDiscard();
        }

        NotifyDeckChanged();
    }

    /// <summary>
    /// Returns every unplayed card back into the draw pile.
    /// Used when the player presses Refill.
    /// </summary>
    public void ReturnRemainingHandToDrawPile()
    {
        drawPile.AddRange(Hand);
        Hand.Clear();

        Shuffle(drawPile);

        NotifyDeckChanged();
    }

    /// <summary>
    /// Draws up to 7 cards.
    /// If the draw pile runs out, the player simply has fewer cards.
    /// </summary>
    public void DrawHand()
    {
        while (Hand.Count < HAND_SIZE && drawPile.Count > 0)
        {
            Hand.Add(drawPile[0]);
            drawPile.RemoveAt(0);
        }

        NotifyDeckChanged();
    }

    private void ReshuffleDiscard()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();

        Shuffle(drawPile);

        Debug.Log("New deck cycle started.");
    }

    private void Shuffle(List<CardInstance> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void NotifyDeckChanged()
    {
        OnDeckChanged?.Invoke();
    }
}