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


        if (card.Data.removeAfterPlay)
        {
            // Permanently removed from the game
            Debug.Log($"{card.Data.cardName} removed permanently.");
        }
        else
        {
            // Normal cards go to Played Cards
            discardPile.Add(card);
        }


        // Entire deck cycle completed
        if (drawPile.Count == 0 && Hand.Count == 0)
        {
            ReshuffleDiscard();
        }


        NotifyDeckChanged();
    }


    /// <summary>
    /// Refill behavior:
    /// Normal cards return to Draw Pile.
    /// One-time cards move to Played Cards.
    /// </summary>
    public void ReturnRemainingHandToDrawPile()
    {
        foreach (CardInstance card in Hand)
        {
            if (card.Data.removeAfterPlay)
            {
                // One-time cards are stored in Played Cards
                discardPile.Add(card);

                Debug.Log($"{card.Data.cardName} moved to Played Cards.");
            }
            else
            {
                // Normal cards return to Draw Pile
                drawPile.Add(card);
            }
        }


        Hand.Clear();

        Shuffle(drawPile);

        NotifyDeckChanged();
    }


    /// <summary>
    /// Draw up to 7 cards.
    /// If Draw Pile runs out, player gets fewer cards.
    /// Played Cards only return when the cycle is complete.
    /// </summary>
    public void DrawHand()
    {
        while (Hand.Count < HAND_SIZE)
        {
            // No cards left to draw
            if (drawPile.Count == 0)
            {
                // Only reshuffle after a full cycle
                if (Hand.Count == 0 && discardPile.Count > 0)
                {
                    ReshuffleDiscard();
                }
                else
                {
                    break;
                }
            }


            if (drawPile.Count == 0)
                break;


            Hand.Add(drawPile[0]);
            drawPile.RemoveAt(0);
        }


        NotifyDeckChanged();
    }


    private void ReshuffleDiscard()
    {
        if (discardPile.Count == 0)
            return;


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

            (list[i], list[randomIndex]) =
                (list[randomIndex], list[i]);
        }
    }


    private void NotifyDeckChanged()
    {
        OnDeckChanged?.Invoke();
    }
}