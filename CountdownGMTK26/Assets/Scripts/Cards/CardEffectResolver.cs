using UnityEngine;

public static class CardEffectResolver
{
    public static void Apply(CardInstance card, TimerManager timer, ModifierManager modifiers)
    {
        float modifierValue = 0;
        if (modifiers) modifierValue = modifiers.CheckModsCardPlayed(card.Data, timer.CurrentTime);

        switch (card.Data.effect)
        {
            case CardEffectType.AddTime:
                timer.AddTime(card.Data.value + modifierValue);
                break;

            case CardEffectType.RemoveTime:
                timer.RemoveTime(card.Data.value + modifierValue);
                break;

            case CardEffectType.MultiplyTime:
                timer.MultiplyTime(card.Data.value + modifierValue);
                break;

            case CardEffectType.DivideTime:
                timer.DivideTime(card.Data.value + modifierValue);
                break;

            case CardEffectType.IncreaseDrainRate:
                timer.IncreaseDrain(card.Data.value + modifierValue);
                break;

            case CardEffectType.DecreaseDrainRate:
                timer.DecreaseDrain(card.Data.value + modifierValue);
                break;

            case CardEffectType.FreezeTime:
                timer.Freeze(card.Data.value + modifierValue);
                break;
        }
    }
}
