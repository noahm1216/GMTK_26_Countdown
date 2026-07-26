using System.Collections.Generic;
using UnityEngine;

public class ModifierUIManager : MonoBehaviour
{
    public static ModifierUIManager Instance { get; private set; }


    public ModUI modIconTemplate;
    private Transform modIconHolder;

    // public void Awake()
    // {
    //     if (Instance != null && Instance != this) Destroy(this);
    //     else Instance = this;
    // }

    public void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        
        if (modIconTemplate) modIconHolder = modIconTemplate.transform.parent;
    }

    public void UpdateModIconsList(List<CardModifierBase> _modsOwned)
    {
        DeleteModsInList();

        if (_modsOwned == null || _modsOwned.Count == 0) return;

        for (int i = 0; i < _modsOwned.Count; i++)
        {
            ModUI modUiClone = Instantiate(modIconTemplate, modIconHolder);
            modUiClone.UpdateModInfo(_modsOwned[i]);
            modUiClone.gameObject.SetActive(true);
        }

    }

    private void DeleteModsInList()
    {
        if (!modIconHolder) return;

        for (int i = 0; i < modIconHolder.childCount; i++)
            if (modIconHolder.GetChild(i) != modIconTemplate.transform) Destroy(modIconHolder.GetChild(i).gameObject);
    }
}
