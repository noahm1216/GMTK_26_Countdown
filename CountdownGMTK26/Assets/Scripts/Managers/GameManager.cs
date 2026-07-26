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
    public ModifierUIManager modifierUiManager { get; private set; }
   

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject startPanel;

    public bool GameActive { get; private set; }

    private EffectAlignment lastAlignmentUpgradeCard = EffectAlignment.Negative;
    private EffectAlignment lastAlignmentUpgradeMod = EffectAlignment.Negative;

    private void Start()
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
        modifierUiManager = ModifierUIManager.Instance;

        timer.OnGameOver += GameOver;
        
        startPanel.SetActive(true);
        ShowModsAchieved();
    }

    // private void Start()
    // {
    //     startPanel.SetActive(true);
    //     ShowModsAchieved();
    // }

    private void OnDestroy()
    {
        timer.OnGameOver -= GameOver;
    }

    public void StartGame()
    {

        startPanel.SetActive(false);
        
        GameActive = true;

        gameOverPanel.SetActive(false);

        deck.BuildDeck();

        handUI.Refresh();

        scoreManager.StartScore();

        timer.StartTimer();
    }

    public void PlayCard(CardInstance card, CardUI ui)
    {
        if (!GameActive)
            return;

        CheckCardEffectAndApplyGradient(card);

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

    private void CheckCardEffectAndApplyGradient(CardInstance card)
    {
        if (card.Data.effect == CardEffectType.FreezeTime)
        {
            Debug.Log("Freeze time" + timer.freezeTimer);
            timer.timerUI.FlashColorGradient(timer.timerUI.freezeGradient, card.Data.value);
        }
        if (card.Data.effect == CardEffectType.AddTime)
        {
            timer.timerUI.FlashColorGradient(timer.timerUI.addGradient, timer.timerUI.flashGradientHoldTime);
        }
        if (card.Data.effect == CardEffectType.MultiplyTime)
        {
            timer.timerUI.FlashColorGradient(timer.timerUI.addGradient, timer.timerUI.flashGradientHoldTime);
        }
        if (card.Data.effect == CardEffectType.RemoveTime)
        {
            timer.timerUI.FlashColorGradient(timer.timerUI.subtractGradient, timer.timerUI.flashGradientHoldTime);
        }
        if (card.Data.effect == CardEffectType.DivideTime)
        {
            timer.timerUI.FlashColorGradient(timer.timerUI.subtractGradient, timer.timerUI.flashGradientHoldTime);
        }
        if (card.Data.effect == CardEffectType.DecreaseDrainRate)
        {
            timer.timerUI.FlashColorGradient(timer.timerUI.purpleGradient, timer.timerUI.flashGradientHoldTime);
        }
        if (card.Data.effect == CardEffectType.IncreaseDrainRate)
        {
            timer.timerUI.FlashColorGradient(timer.timerUI.orangeGradient, timer.timerUI.flashGradientHoldTime);
        }
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

    public void ShowModsAchieved()
    {
        if (!modifierUiManager) { Debug.LogError("Missing Mod-UI-Manager"); return; }

        modifierUiManager.UpdateModIconsList(modifierManager.ownedModifiers);
    }

    public EffectAlignment CycleAlignment(EffectAlignment _oldAlignment)
    {
        switch (_oldAlignment)
        {
            case EffectAlignment.Neutral:
                _oldAlignment = EffectAlignment.Positive;
                break;
            case EffectAlignment.Negative:
                _oldAlignment = EffectAlignment.Positive;
                break;           
            case EffectAlignment.Positive:
                _oldAlignment = EffectAlignment.Negative;
                break;
            default:
                _oldAlignment = EffectAlignment.Positive;
                break;
        }

        return _oldAlignment;
    }

    public void GrabUpgradeOptions()
    {
        if (!modifierManager || !deck) { Debug.LogWarning("CANNOT GRAB UPGRADE OPTIONS"); return; }

        lastAlignmentUpgradeCard = CycleAlignment(lastAlignmentUpgradeCard); // cycle the card/mod types we'll see
        lastAlignmentUpgradeMod = CycleAlignment(lastAlignmentUpgradeMod);

        CardData option1 = deck.ReturnCardFromAllCards(lastAlignmentUpgradeCard); // grab the cards from the pool
        CardModifierBase option2 = modifierManager.ReturnModiferFromAllModifiers(lastAlignmentUpgradeMod);       
        
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