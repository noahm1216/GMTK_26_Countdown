using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    [Header("Card Info")]
    public string cardName;

    [TextArea]
    public string description;

    public Sprite artwork;

    [Header("Gameplay")]
    public CardEffectType effect;

    public EffectAlignment alignment;

    public float value;

    public Color textColor = Color.white;

    [Header("Special Rules")]
    public bool removeAfterPlay;

#if UNITY_EDITOR
    private void OnValidate()
    {
        description = GenerateDescription();
    }
#endif

    private string GenerateDescription()
    {
        switch (effect)
        {
            case CardEffectType.AddTime:
                return $"+{value:0} Seconds";

            case CardEffectType.RemoveTime:
                return $"-{value:0} Seconds";

            case CardEffectType.MultiplyTime:
                return $"Multiply Time x{value:0.##}";

            case CardEffectType.DivideTime:
                return $"Divide Time by {value:0.##}";

            case CardEffectType.IncreaseDrainRate:
                return $"Increase Drain +{value:0.##}";

            case CardEffectType.DecreaseDrainRate:
                return $"Decrease Drain -{value:0.##}";

            case CardEffectType.FreezeTime:
                return $"Freeze Timer for {value:0.#} Seconds";

            case CardEffectType.DeleteNextPlayedCards:
                return $"Delete Next {value:0} Played Cards";

            case CardEffectType.IgnoreNextCard:
                return $"Ignore next {value:0} card effect";

            default:
                return "";
        }
    }
}