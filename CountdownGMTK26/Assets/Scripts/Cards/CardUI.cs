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
    private CardModifierBase mod;
    private GameManager gameManager;

    private CardAnimation animation;

    public void InitializeCard(CardInstance cardInstance, GameManager gm)
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

        animation = GetComponent<CardAnimation>();

        if (animation != null)
        {
            animation.AnimateEnter();
        }
    }

    public void InitializeMod(CardModifierBase cardMod, GameManager gm)
    {
        mod = cardMod;
        gameManager = gm;

        cardNameText.text = cardMod.modName;
        cardNameText.color = cardMod.textColor;
        descriptionText.text = cardMod.description;
        //valueText.text = cardMod.value.ToString();

        if (artworkImage != null)
            artworkImage.sprite = cardMod.artwork;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        gameManager.PlayCard(card);
    }
}
