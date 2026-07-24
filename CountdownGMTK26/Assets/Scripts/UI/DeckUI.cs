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
        drawText.text = $"Draw: {deckManager.DrawCount}";
        discardText.text = $"Discard: {deckManager.DiscardCount}";
        handText.text = $"Hand: {deckManager.HandCount}";
    }
}