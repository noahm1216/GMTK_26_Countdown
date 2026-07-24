using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Image artworkImage;
    [SerializeField] private Button button;

    private CardInstance card;
    private GameManager gameManager;

    public void Initialize(CardInstance cardInstance, GameManager gm)
    {
        card = cardInstance;
        gameManager = gm;

        cardNameText.text = card.Data.cardName;
        cardNameText.color = card.Data.textColor;
        descriptionText.text = card.Data.description;
        valueText.text = card.Data.value.ToString();

        if (artworkImage != null)
            artworkImage.sprite = card.Data.artwork;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        gameManager.PlayCard(card);
    }
}
