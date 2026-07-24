using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private TimerManager timer;
    [SerializeField] private DeckManager deck;
    [SerializeField] private HandUI handUI;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    public bool GameActive { get; private set; }

    private void Awake()
    {
        timer.OnGameOver += GameOver;
    }

    private void Start()
    {
        StartGame();
    }

    private void OnDestroy()
    {
        timer.OnGameOver -= GameOver;
    }

    private void StartGame()
    {
        GameActive = true;

        gameOverPanel.SetActive(false);

        deck.BuildDeck();

        handUI.Refresh();

        scoreManager.StartScore();
    }

    public void PlayCard(CardInstance card)
    {
        if (!GameActive)
            return;

        CardEffectResolver.Apply(card, timer);

        deck.PlayCard(card);

        handUI.Refresh();
    }

    public void PlayerRefillHand()
    {
        if (!GameActive)
            return;

        if (!deck.CanRefill)
            return;

        deck.ReturnRemainingHandToDrawPile();

        deck.DrawHand();

        handUI.Refresh();
    }

    private void GameOver()
    {
        GameActive = false;

        scoreManager.StopScore();

        gameOverPanel.SetActive(true);

        Debug.Log("Game Over!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}