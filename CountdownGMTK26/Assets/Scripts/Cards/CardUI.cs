using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public bool isUpgrade;

    [Header("UI")]
    [SerializeField] private TMP_Text cardNameText;
    [SerializeField] private TMP_Text cardNameTextOutline;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text descriptionTextOutline;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Image artworkImage;
    [SerializeField] private Button button;

    [Header("Card Movement")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float arcHeight = 80f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float bounceAmount = 15f;
    [SerializeField] private float bounceSpeed = 12f;

    private bool isMoving;
    private Vector3 startPosition;
    private float moveProgress;

    private CardInstance card;
    private CardModifierBase mod;
    private GameManager gameManager;

    private CardAnimation animation;

    private Vector3 targetPosition;
    private Quaternion targetRotation;



    
    private void Update()
        {
            if (isMoving)
            {
                MoveAlongArc();
            }
        }
   

    public void InitializeCard(CardInstance cardInstance, GameManager gm)
    {
        card = cardInstance;
        gameManager = gm;

        cardNameText.text = card.Data.cardName;
        cardNameText.color = card.Data.textColor;
        descriptionText.text = card.Data.description;
        descriptionTextOutline.text = card.Data.description;
        valueText.text = card.Data.value.ToString();

        if (artworkImage != null)
            artworkImage.sprite = card.Data.artwork;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);

        animation = GetComponent<CardAnimation>();

        if (animation != null)
        {
            //animation.AnimateEnter();
        }
    }

    public void InitializeMod(CardModifierBase cardMod, GameManager gm)
    {
        mod = cardMod;
        gameManager = gm;

        cardNameText.text = cardMod.modName;
        cardNameTextOutline.text = cardMod.modName;
        cardNameText.color = cardMod.textColor;
        descriptionText.text = cardMod.description;
        descriptionTextOutline.text = cardMod.description;
        //valueText.text = cardMod.value.ToString();

        if (artworkImage != null)
            artworkImage.sprite = cardMod.artwork;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        if (isUpgrade)
        {
            if (mod)
            {
                gameManager.modifierManager.AddModifierToOwned(mod);
                gameManager.ShowModsAchieved();
            }
            else if (card.Data)
                gameManager.deck.AddCardToDeck(card.Data);

            gameManager.upgradeUI.ShowCanvas(false); // return to playing
            TimerManager.Instance.StartTimer();
        }
        else
            gameManager.PlayCard(card, this);
    }

    public void AnimateToDiscard(Vector3 target, System.Action onFinished)
    {
        animation.AnimateExit(target, onFinished);
    }

    public void AnimateToDrawPile(Vector3 target, System.Action onFinished)
    {
        animation.AnimateExit(target, onFinished);
    }

    public void AnimateDestroy(System.Action onFinished)
    {
        animation.AnimateDestroy(onFinished);
    }

    public void MoveTo(Vector3 position, float rotation)
    {
        targetPosition = position;
        targetRotation = Quaternion.Euler(0, 0, rotation);

        startPosition = transform.localPosition;

        moveProgress = 0;
        isMoving = true;
    }

    private void MoveAlongArc()
    {
        moveProgress += Time.deltaTime * moveSpeed;


        float t = Mathf.Clamp01(moveProgress);


        // Smooth acceleration/deceleration
        float smoothT = Mathf.SmoothStep(0, 1, t);


        Vector3 position =
            Vector3.Lerp(
                startPosition,
                targetPosition,
                smoothT);


        // Add arc
        position.y +=
            Mathf.Sin(t * Mathf.PI) * arcHeight;


        transform.localPosition = position;


        transform.localRotation =
            Quaternion.Lerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed);


        if (t >= 1)
        {
            StartCoroutine(LandingBounce());
            isMoving = false;
        }
    }

    public void SetInstantPosition(Vector3 worldPosition)
    {
        RectTransform rect = transform as RectTransform;

        rect.position = worldPosition;

        targetPosition = rect.localPosition;
    }

    private System.Collections.IEnumerator LandingBounce()
{
    Vector3 start = transform.localPosition;

    Vector3 peak =
        start + Vector3.up * bounceAmount;


    float timer = 0;


    while(timer < 1)
    {
        timer += Time.deltaTime * bounceSpeed;


        transform.localPosition =
            Vector3.Lerp(
                peak,
                start,
                timer);


        yield return null;
    }


    transform.localPosition = start;
}
}
