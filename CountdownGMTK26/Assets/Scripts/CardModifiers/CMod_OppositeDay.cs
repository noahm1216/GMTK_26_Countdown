

public class CMod_Opposite : CardModifierBase
{
    public override float OnCardSelected(CardData _cardData, float _timeCur = -1)
    {
        if (timesUsableInGame < timesApplied || timesUsableInGame == -1) // checks if card is used too many times
        {
            if (_cardData) // checks if there was data sent 
            {
                if (_cardData.effect == CardEffectType.AddTime) // if condition is met
                { timesApplied++; return _cardData.value * -1f; } // return value

                else if (_cardData.effect == CardEffectType.RemoveTime) ;
                { timesApplied++; return _cardData.value * -1f; }

            }
        }
        return base.OnCardSelected(_cardData, _timeCur); // otherwise call base value
    }
}
