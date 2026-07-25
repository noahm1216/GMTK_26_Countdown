using System.Collections.Generic;
using UnityEngine;


public class ModifierManager : MonoBehaviour
{
    public List<CardModifierBase> allModifiers = new List<CardModifierBase>();
    public List<CardModifierBase> ownedModifiers = new List<CardModifierBase>();


    public float CheckModsCardPlayed(CardData _cardData, float _timeCur = -1)
    {
        //print($"Checking Card Mods - Card Played : val ={_cardData.value}");
        float newValue = 0;

        for (int i = 0; i < ownedModifiers.Count; i++)
        {
            newValue = ownedModifiers[i].OnCardSelected(_cardData, _timeCur);
            //print("checked");
        }

        //print($"Cards Checked: New Value = {newValue}");
        return newValue;
    }

    public bool CardModPlayable(CardModifierBase _mod)
    {
        return (_mod.timesUsableInGame == -1 || _mod.timesUsableInGame > _mod.timesApplied);
    }


    public CardModifierBase ReturnModiferFromAllModifiers(EffectAlignment _alignment, int _id = -1, bool _repeatOkay = true) // set to false if we dont want repeat mod/runes
    {
        print("Return Modifier From All Modifier List");

        CardModifierBase cardmodToReturn = null;
        if (_id > -1 && _id <= allModifiers.Count) // grab a specific card
        {
            print($"Grabbing Mod By ID {_id}");
            if (!ownedModifiers.Contains(allModifiers[_id]) || ownedModifiers.Contains(allModifiers[_id]) && CardModPlayable(allModifiers[_id]) && _repeatOkay)
                cardmodToReturn = allModifiers[_id];
        }

        if (cardmodToReturn == null)
        {
            print($"NO Mod grabbed By ID {_id}");

            List<CardModifierBase> possibleCardmods = new List<CardModifierBase>();
            for (int i = 0; i < allModifiers.Count; i++)
            {
                if (allModifiers[i].alignment == _alignment)
                {
                    print($"Found modifier of similar alignment {allModifiers[i].modName}");
                    if (!ownedModifiers.Contains(allModifiers[i]) || ownedModifiers.Contains(allModifiers[i]) && CardModPlayable(allModifiers[i]) && _repeatOkay)
                    { possibleCardmods.Add(allModifiers[i]); print($"Add possible mod: {allModifiers[i].modName}"); }
                }
            }
            if (possibleCardmods.Count > 0)
                cardmodToReturn = possibleCardmods[Random.Range(0, possibleCardmods.Count)];
            else Debug.LogError("CANT FIND ANY MOD UPGRADES");

            return cardmodToReturn;
        }
        return null;
    }
}
