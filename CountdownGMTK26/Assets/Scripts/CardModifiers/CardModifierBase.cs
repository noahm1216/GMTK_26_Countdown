using UnityEngine;

public abstract class CardModifierBase : MonoBehaviour
{
    public int timesApplied { get; protected set; } // how many times this bonus has been activated
    [Tooltip("How many times this rune can be applied. If -1 then unlimited")]
    public int timesUsableInGame = -1;
    public EffectAlignment alignment = EffectAlignment.Neutral;

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
