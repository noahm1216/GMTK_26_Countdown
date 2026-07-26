using System;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    public List<CardData> allCards = new List<CardData>();

    [Header("Starting Deck")]
    [SerializeField]
    private List<CardData> startingDeck = new List<CardData>();


    public List<CardInstance> Hand { get; private set; } = new List<CardInstance>();

    private readonly List<CardInstance> drawPile = new List<CardInstance>();
    private readonly List<CardInstance> discardPile = new List<CardInstance>();
    private int deleteCounter = 0;
    private int ignoreCounter = 0;


    private const int HAND_SIZE = 7;
    private const int REFILL_THRESHOLD = 3;

    public event Action OnDeckChanged;

    public int DrawCount => drawPile.Count;
    public int HandCount => Hand.Count;
    public int DiscardCount => discardPile.Count;

    public bool CanRefill => Hand.Count <= REFILL_THRESHOLD;

    public void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }


    /// <summary>
    /// This function returns a single card from the list of all cards that are in the game.
    /// </summary>
    /// <param name="_alignment"> which pool of cards to pull from</param>
    /// <param name="_id"> if we want a specific index, we can ask for it here otherwise -1 = random</param>
    /// <param name="_repeatOkay"> if we dont mind a duplicate, or no dupes</param>
    /// <returns></returns>
    public CardData ReturnCardFromAllCards(EffectAlignment _alignment, int _id = -1, bool _repeatOkay = true)
    {
        //print("Return Card From All Cards List");
        CardData cardToReturn = null;
        if (_id > -1 && _id <= allCards.Count) // grab a specific card
        {
            if (!startingDeck.Contains(allCards[_id]) || startingDeck.Contains(allCards[_id]) && _repeatOkay)
                cardToReturn = allCards[_id];
        }

        if (cardToReturn == null)
        {
            List<CardData> possibleCards = new List<CardData>();
            for (int i = 0; i < allCards.Count; i++)
            {
                if (allCards[i].alignment == _alignment)
                {
                    if (!startingDeck.Contains(allCards[i]) || startingDeck.Contains(allCards[i]) && _repeatOkay)
                        possibleCards.Add(allCards[i]);
                }
            }
            cardToReturn = possibleCards[UnityEngine.Random.Range(0, possibleCards.Count)];
        }

        return cardToReturn;
    }

    public void AddCardToDeck(CardData _card)
    {
        drawPile.Add(new CardInstance(_card));
    }


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

    public void QueueDeleteCards(int amount)
    {
        deleteCounter += amount;

        Debug.Log($"Delete queued. Next {deleteCounter} played cards will be removed.");
    }


    public bool PlayCard(CardInstance card)
    {
        if (!Hand.Contains(card))
            return false;

        Hand.Remove(card);

        bool ignored = false;


        if (ignoreCounter > 0 &&
            card.Data.effect != CardEffectType.IgnoreNextCard)
        {
            ignoreCounter--;

            ignored = true;

            Debug.Log(
                $"{card.Data.cardName} effect ignored.");
        }

        // One-time-use cards always remove themselves.
        if (card.Data.removeAfterPlay)
        {
            Debug.Log($"{card.Data.cardName} removed permanently.");
        }

        // Delete effect should NEVER consume itself.
        else if (
            deleteCounter > 0 &&
            card.Data.effect != CardEffectType.DeleteNextPlayedCards)
        {
            deleteCounter--;

            Debug.Log($"{card.Data.cardName} permanently deleted. Remaining deletes: {deleteCounter}");
        }

        // Normal behavior
        else
        {
            discardPile.Add(card);
        }

        if (drawPile.Count == 0 && Hand.Count == 0)
        {
            ReshuffleDiscard();
        }

        NotifyDeckChanged();
        return ignored;

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

    public void QueueIgnoreCard(int amount)
    {
        ignoreCounter += amount;

        Debug.Log($"Next {ignoreCounter} card effect(s) ignored.");
    }


    private void NotifyDeckChanged()
    {
        OnDeckChanged?.Invoke();
    }
}