using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Managers")]
    public TimerManager timer { get; private set; }
    public DeckManager deck { get; private set; }
    public HandUI handUI { get; private set; }
    public UpgradeUI upgradeUI { get; private set; }
    public ScoreManager scoreManager { get; private set; }
    public ModifierManager modifierManager { get; private set; }
   

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    public bool GameActive { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        // refs
        timer = TimerManager.Instance;
        deck = DeckManager.Instance;
        handUI = HandUI.Instance;
        upgradeUI = UpgradeUI.Instance;
        scoreManager = ScoreManager.Instance;
        modifierManager = ModifierManager.Instance;        

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

    public void PlayCard(CardInstance card, CardUI ui)
    {
        if (!GameActive)
            return;

        // One-time card
        if (card.Data.removeAfterPlay)
        {
            ui.AnimateDestroy(() =>
            {
                FinishPlayingCard(card);
            });

            return;
        }

        // Normal card
        ui.AnimateToDiscard(
            handUI.DiscardPilePosition,
            () =>
            {
                FinishPlayingCard(card);
            });
    }

    private void FinishPlayingCard(CardInstance card)
    {
        bool ignored = deck.PlayCard(card);


        if (!ignored)
        {
            CardEffectResolver.Apply(
                card,
                timer,
                modifierManager,
                deck);
        }

        deck.PlayCard(card);

        handUI.RemoveCard(card);

        handUI.Refresh();
    }

    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Alpha0))  GrabUpgradeOptions(); 
    }

    public void GrabUpgradeOptions()
    {
        if (!modifierManager || !deck) { Debug.LogWarning("CANNOT GRAB UPGRADE OPTIONS"); return; }

        CardData option1 = deck.ReturnCardFromAllCards(EffectAlignment.Positive);
        CardModifierBase option2 = modifierManager.ReturnModiferFromAllModifiers(EffectAlignment.Positive);
        if (option2 != null) print($"GOT MODIFIER: {option2.modName} - {option2.description}");

        
        // create or populate reference to card options
        if (upgradeUI && option1) upgradeUI.UpdateCardInformation(new CardInstance(option1), this);
        if (upgradeUI && option2) upgradeUI.UpdateRuneInformation(option2, this);
        if (upgradeUI) upgradeUI.ShowCanvas(true);

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