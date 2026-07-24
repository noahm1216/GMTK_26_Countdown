using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerReducer : MonoBehaviour
{
    public Button reducerTimerBtn;
    private TD_TimerManager timer;
    private TD_ScoreManager scoreManager;
    
    void Start()
    {
        timer = GameObject.FindObjectsByType<TD_TimerManager>()[0];
        scoreManager = GameObject.FindObjectsByType<TD_ScoreManager>()[0];
        Button btn = reducerTimerBtn.GetComponent<Button>();

        btn.onClick.AddListener(ReduceTimer);
    }
    
    void ReduceTimer()
    {
        if (scoreManager.currency >= 50)
        {    
            Debug.Log("Timer clicked.");
            scoreManager.ReduceCurrency(50);
            timer.currentTime -= 10f;
        }
    }

}
