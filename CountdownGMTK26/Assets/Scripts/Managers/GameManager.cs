using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private TimerManager timer;
    [SerializeField] private DeckManager deck;
    [SerializeField] private HandUI handUI;

    private void Awake()
    {
        timer.OnGameOver += GameOver;
    }

    private void Start()
    {
        deck.BuildDeck();

        handUI.Refresh();
    }

    private void OnDestroy()
    {
        timer.OnGameOver -= GameOver;
    }

    public void PlayCard(CardInstance card)
    {
        CardEffectResolver.Apply(card, timer);

        deck.PlayCard(card);

        handUI.Refresh();
    }

    public void PlayerRefillHand()
    {
        if (!deck.CanRefill)
            return;

        deck.ReturnRemainingHandToDrawPile();

        deck.DrawHand();

        handUI.Refresh();
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");

        // TODO:
        // Disable card input
        // Show Game Over UI
        // Save High Score
    }
}