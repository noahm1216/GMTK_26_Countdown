using UnityEngine;

public static class CardEffectResolver
{
    public static void Apply(CardInstance card, TimerManager timer)
    {
        switch (card.Data.effect)
        {
            case CardEffectType.AddTime:
                timer.AddTime(card.Data.value);
                break;

            case CardEffectType.RemoveTime:
                timer.RemoveTime(card.Data.value);
                break;

            case CardEffectType.MultiplyTime:
                timer.MultiplyTime(card.Data.value);
                break;

            case CardEffectType.DivideTime:
                timer.DivideTime(card.Data.value);
                break;

            case CardEffectType.IncreaseDrainRate:
                timer.IncreaseDrain(card.Data.value);
                break;

            case CardEffectType.DecreaseDrainRate:
                timer.DecreaseDrain(card.Data.value);
                break;
        }
    }
}
