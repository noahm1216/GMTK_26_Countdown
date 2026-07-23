using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TimerManager timer;
    [SerializeField] private TMP_Text timerText;

    private void Update()
    {
        timerText.text = timer.CurrentTime.ToString("0.0");
    }
}