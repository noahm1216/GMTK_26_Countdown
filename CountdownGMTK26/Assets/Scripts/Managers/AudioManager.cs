using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource aSource;
    public Slider volumeSlider;

    public void SetVolume()
    {
        aSource.volume = volumeSlider.value;
    }
}
