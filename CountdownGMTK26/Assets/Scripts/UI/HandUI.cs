using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handParent;


    private Dictionary<CardInstance, CardUI> activeCards = new();


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
        foreach (CardInstance card in currentHand)
        {
            if (!activeCards.ContainsKey(card))
            {
                GameObject obj =
                    Instantiate(
                        cardPrefab,
                        handParent);


                CardUI ui =
                    obj.GetComponent<CardUI>();


                ui.InitializeCard(
                    card,
                    gameManager);


                activeCards.Add(card, ui);
            }
        }
    }
}