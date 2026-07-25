using UnityEngine;

public static class CardEffectResolver
{
    public static void Apply(
        CardInstance card,
        TimerManager timer,
        ModifierManager modifierManager,
        DeckManager deck)
       
    {
        float value = card.Data.value;

        if (modifierManager != null)
        {
            float modifiedValue =
                modifierManager.CheckModsCardPlayed(card.Data, timer.CurrentTime);

            if (modifiedValue != -1)
                value = modifiedValue;
        }

        switch (card.Data.effect)
        {
            case CardEffectType.AddTime:

                timer.AddTime(value);
               

                break;

            case CardEffectType.RemoveTime:

                timer.RemoveTime(value);
               

                break;

            case CardEffectType.MultiplyTime:

                timer.MultiplyTime(value);
               

                break;

            case CardEffectType.DivideTime:

                timer.DivideTime(value);
               

                break;

            case CardEffectType.IncreaseDrainRate:

                timer.IncreaseDrain(value);
                

                break;

            case CardEffectType.DecreaseDrainRate:

                timer.DecreaseDrain(value);
              

                break;

            case CardEffectType.FreezeTime:

                timer.Freeze(value);
               

                break;

            case CardEffectType.DeleteNextPlayedCards:

                deck.QueueDeleteCards((int)value);
                

                break;
        }
    }
}