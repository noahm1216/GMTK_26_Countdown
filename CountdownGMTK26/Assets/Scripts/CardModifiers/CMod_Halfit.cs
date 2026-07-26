

public class CMod_Halfit: CardModifierBase
{
    public override float OnCardSelected(CardData _cardData, float _timeCur = -1)
    {
        if (timesUsableInGame < timesApplied || timesUsableInGame == -1) // checks if card is used too many times
        {
            if (_cardData) // checks if there was data sent 
            {
                // if condition is met
                timesApplied++; return _cardData.value * .5f; // return value

            }
        }
        return base.OnCardSelected(_cardData, _timeCur); // otherwise call base value
    }
}
