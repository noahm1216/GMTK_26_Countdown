using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModUI : MonoBehaviour
{
    public CardModifierBase mod { get; private set; }

   
    public Image image;
    public Image descBackground;
    public TextMeshProUGUI description;


    public void UpdateModInfo(CardModifierBase _mod)
    {
        mod = _mod;
        image.sprite = mod.artwork;
        description.text = mod.description;
        descBackground.gameObject.SetActive(false);
    }

    public void DescriptionToggle()
    {
        descBackground.gameObject.SetActive(!descBackground.gameObject.activeSelf);
    }

}
