using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public CardUI cardChoiceOne;
    public CardUI cardChoiceTwo;


    private void Start()
    {
        transform.gameObject.SetActive(false);
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
