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

        CardModifierBase cardmodToReturn = null;
        if (_id > -1 && _id <= allModifiers.Count) // grab a specific card
        {
            if (!ownedModifiers.Contains(allModifiers[_id]) || ownedModifiers.Contains(allModifiers[_id]) && _repeatOkay)
                cardmodToReturn = allModifiers[_id];
        }

        if (cardmodToReturn == null)
        {
            List<CardModifierBase> possibleCardmods = new List<CardModifierBase>();
            for (int i = 0; i < allModifiers.Count; i++)
            {
                if (allModifiers[i].alignment == _alignment)
                {
                    if (!ownedModifiers.Contains(allModifiers[i]) || ownedModifiers.Contains(allModifiers[i]) && _repeatOkay)
                        possibleCardmods.Add(allModifiers[i]);
                }
            }
            cardmodToReturn = possibleCardmods[UnityEngine.Random.Range(0, possibleCardmods.Count + 1)];
        }

        return null;
    }
}
