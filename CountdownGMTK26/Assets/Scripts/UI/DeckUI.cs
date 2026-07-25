using TMPro;
using UnityEngine;

public class DeckUI : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;

    [Header("UI Text")]
    [SerializeField] private TMP_Text drawText;
    [SerializeField] private TMP_Text discardText;
    [SerializeField] private TMP_Text handText;


    private void Start()
    {
        deckManager.OnDeckChanged += Refresh;

        Refresh();
    }

    private void OnDestroy()
    {
        deckManager.OnDeckChanged -= Refresh;
    }


    private void Refresh()
    {
        drawText.text = $"{deckManager.DrawCount} Drawn";
        discardText.text = $"{deckManager.DiscardCount} Played";
        handText.text = $"{deckManager.HandCount} in Hand";
    }
}