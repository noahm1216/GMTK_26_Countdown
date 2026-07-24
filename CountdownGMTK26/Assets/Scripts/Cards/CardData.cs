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

    public float value;

    public Color textColor;
}