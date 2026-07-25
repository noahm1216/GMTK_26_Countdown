using System.Collections.Generic;
using UnityEngine;


public class ModifierManager : MonoBehaviour
{
    public List<CardModifierBase> allModifiers = new List<CardModifierBase>();
    public List<CardModifierBase> ownedModifiers = new List<CardModifierBase>();


    public float CheckModsCardPlayed(CardData _cardData, float _timeCur =-1)
    {
        //print($"Checking Card Mods - Card Played : val ={_cardData.value}");
        float newValue = 0;

        for(int i = 0; i < ownedModifiers.Count; i++)
        {
            newValue = ownedModifiers[i].OnCardSelected(_cardData, _timeCur);
            //print("checked");
        }

       //print($"Cards Checked: New Value = {newValue}");
        return newValue;
    }


    public CardModifierBase ReturnModiferFromAllModifiers(EffectAlignment _alignment, int _id = -1, bool _repeatOkay = false)
    {
        print("Return Modifier From All Modifier List");
        return null;
    }
}
