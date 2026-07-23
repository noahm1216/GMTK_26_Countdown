using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private TimerManager timer;
    [SerializeField] private DeckManager deck;
    [SerializeField] private HandUI handUI;
    [SerializeField] private ScoreManager scoreManager;

    private void Awake()
    {
        timer.OnGameOver += GameOver;
    }

    private void Start()
    {
        deck.BuildDeck();

        handUI.Refresh();
        
        scoreManager.StartScore();
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
        scoreManager.StopScore();

        Debug.Log($"Final Score: {scoreManager.CurrentScore}");

        // TODO:
        // Disable card input
        // Show Game Over UI
        // Save High Score
    }
}