

public class CMod_Empty : CardModifierBase
{
    public override float OnCardSelected(CardData _cardData, float _timeCur = -1)
    {
        if (timesUsableInGame < timesApplied || timesUsableInGame == -1) // checks if card is used too many times
        {
    
        }
        return base.OnCardSelected(_cardData, _timeCur); // otherwise call base value
    }
}
