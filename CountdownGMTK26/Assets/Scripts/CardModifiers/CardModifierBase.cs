using UnityEngine;

public abstract class CardModifierBase : MonoBehaviour
{

    [Header("Card Info")]
    public string modName;

    [TextArea]
    public string description;

    public Sprite artwork;

    [Header("Gameplay")]
    //public CardEffectType effect;

    [Tooltip("How many times this rune can be applied. If -1 then unlimited")]
    public int timesUsableInGame = -1;
    [Tooltip("If this is considered a helpful, negative, or other card type. Effects which pool it will be placed in")]
    public EffectAlignment alignment;

    //public float value;

    public Color textColor = Color.white;

    [Header("Special Rules")]
    public bool removeAfterPlay;


    public int timesApplied { get; protected set; } // how many times this bonus has been activated
    

    /// <summary>
    /// When a card is selected an event occurs
    /// </summary>
    /// <param name="_timeCur"> the current timer in game</param>
    /// <param name="_cardData"> the data of the card being selected </param>
    /// <returns></returns>
    public virtual float OnCardSelected(CardData _cardData, float _timeCur = -1)
    {
        return -1;
    }

    /// <summary>
    ///  When the timer reaches a certain time, an event can occur 
    /// </summary>
    /// <param name="_timeCur"> the current timer in-game </param>
    /// <returns></returns>
    public virtual float OnTimeWindowReached(float _timeCur)
    {
        return -1;
    }
}
