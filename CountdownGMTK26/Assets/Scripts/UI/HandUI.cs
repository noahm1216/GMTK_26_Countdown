using UnityEngine;

public class HandUI : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handParent;

    public void Refresh()
    {
        foreach (Transform child in handParent)
            Destroy(child.gameObject);

        foreach (CardInstance card in deckManager.Hand)
        {
            GameObject obj = Instantiate(cardPrefab, handParent);

            CardUI ui = obj.GetComponent<CardUI>();

            ui.Initialize(card, gameManager);
        }
    }
}