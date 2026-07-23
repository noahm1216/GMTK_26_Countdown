using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TimerManager timer;
    [SerializeField] private DeckManager deck;
    [SerializeField] private HandUI handUI;

    private void Start()
    {
        handUI.Refresh();
    }

    public void PlayCard(CardInstance card)
    {
        CardEffectResolver.Apply(card, timer);

        deck.PlayCard(card);

        handUI.Refresh();
    }

    private void Awake()
    {
        timer.OnGameOver += GameOver;
    }

    private void OnDestroy()
    {
        timer.OnGameOver -= GameOver;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }
}