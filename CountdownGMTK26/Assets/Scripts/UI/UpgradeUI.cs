using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance { get; private set; }
    public GameObject uiHolder;
    public CardUI cardChoiceOne;
    public CardUI cardChoiceTwo;


    public void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        ShowCanvas(false);
    }

    public void ShowCanvas(bool _show)
    {
        uiHolder.SetActive(_show);
    }

    public void UpdateCardInformation(CardInstance _cardInstance, GameManager _gm)
    {
        if(cardChoiceOne) cardChoiceOne.InitializeCard(_cardInstance, _gm);
        
    }

    public void UpdateRuneInformation(CardModifierBase _cardMod, GameManager _gm)
    {
        if (cardChoiceTwo) cardChoiceTwo.InitializeMod(_cardMod, _gm);
    }
}
