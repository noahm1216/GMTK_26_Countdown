using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private TimerManager timer;
    [SerializeField] private DeckManager deck;
    [SerializeField] private HandUI handUI;
    [SerializeField] private UpgradeUI upgradeUI;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ModifierManager modifierManager;
   

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

        // if multiple card effects, call them here
        CardEffectResolver.Apply(card, timer, modifierManager, deck);

        deck.PlayCard(card);

        handUI.Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) { print("LOAD UPGRADE MENU"); GrabUpgradeOptions(); }
    }

    public void GrabUpgradeOptions()
    {
        if (!modifierManager || !deck) { Debug.LogWarning("CANNOT GRAB UPGRADE OPTIONS"); return; }

        CardData option1 = deck.ReturnCardFromAllCards(EffectAlignment.Positive);
        CardModifierBase option2 = modifierManager.ReturnModiferFromAllModifiers(EffectAlignment.Positive);
        if (option2 != null) print($"GOT MODIFIER: {option2.modName} - {option2.description}");

        if (upgradeUI) upgradeUI.gameObject.SetActive(true);
        // create or populate reference to card options
        if (upgradeUI && option1) upgradeUI.UpdateCardInformation(new CardInstance(option1), this);
        if (upgradeUI && option2) upgradeUI.UpdateRuneInformation(option2, this);

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