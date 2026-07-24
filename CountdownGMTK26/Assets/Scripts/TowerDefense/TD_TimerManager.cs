using UnityEngine;
using TMPro;
using System;

public class TD_TimerManager : MonoBehaviour
{

    public bool isTimerActive;
    public float currentTime;
    public TextMeshProUGUI timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = 25f;
        isTimerActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if timer reached 0, then mark as inactive

        if (isTimerActive)
        {
            currentTime -= Time.deltaTime;

            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            timerText.text = "Before Full Moon: " + time.Minutes.ToString() + ":" + time.Seconds.ToString("D2");
        }

        if (currentTime < 1)
        {
            isTimerActive = false;
        }
    }
}
