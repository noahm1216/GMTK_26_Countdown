using UnityEngine;
using System;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    private float startingTime = 5f;

    [SerializeField]
    private float drainRate = 1f;

    public float CurrentTime { get; private set; }

    public bool IsRunning { get; private set; }

    public event Action OnGameOver;

    void Start()
    {
        CurrentTime = startingTime;
        IsRunning = true;
    }

    void Update()
    {
        if (!IsRunning)
            return;

        CurrentTime -= drainRate * Time.deltaTime;

        if (CurrentTime <= 0)
        {
            CurrentTime = 0;
            IsRunning = false;

            Debug.Log("GAME OVER");

            OnGameOver?.Invoke();
        }
    }

    public void AddTime(float amount)
    {
        CurrentTime += amount;
    }

    public void RemoveTime(float amount)
    {
        CurrentTime = Mathf.Max(0, CurrentTime - amount);
    }

    public void MultiplyTime(float multiplier)
    {
        CurrentTime *= multiplier;
    }

    public void DivideTime(float divisor)
    {
        CurrentTime /= divisor;
    }

    public void IncreaseDrain(float amount)
    {
        drainRate += amount;
    }

    public void DecreaseDrain(float amount)
    {
        drainRate = Mathf.Max(.1f, drainRate - amount);
    }
}