using UnityEngine;
using System;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    [SerializeField]
    private float startingTime = 5f;

    [SerializeField]
    private float drainRate = 1f;

    public float CurrentTime { get; private set; }

    public float PeakTime { get; private set; }

    public bool IsRunning { get; private set; }

    public float timeBetweenUpgrades = 15;
    private float lastTimeUpgrade;
    public bool selectingUpgrade;

    public event Action OnGameOver;

    private float freezeTimer;

    [Header("Difficulty Scaling")]
    [SerializeField] private bool increaseDrainOverTime = true;

    [SerializeField] private float drainIncreaseInterval = 30f;

    [SerializeField] private float drainIncreaseAmount = .25f;

    private float elapsedTime;

    private float nextDrainIncrease;

    public float CurrentDrainRate => drainRate;

    public void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        CurrentTime = startingTime;
        PeakTime = startingTime;

        IsRunning = false;
        elapsedTime = 0;
        nextDrainIncrease = drainIncreaseInterval;
    }

    public void StartTimer()
    {
        IsRunning = true;
        lastTimeUpgrade = Time.time;
        selectingUpgrade = false;
    }

    public void FreezeTimer()
    {
        IsRunning = false;
    }

    public bool TimeForUpgrade()
    {
        return Time.time > lastTimeUpgrade + timeBetweenUpgrades;
    }

    void Update()
    {
        if (!IsRunning)
            return;

        if (TimeForUpgrade()) { GameManager.Instance.GrabUpgradeOptions(); FreezeTimer(); selectingUpgrade = true; }

        if (freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            return;
        }        

        elapsedTime += Time.deltaTime;


        if (increaseDrainOverTime &&
            elapsedTime >= nextDrainIncrease)
        {
            IncreaseDrain(drainIncreaseAmount);

            nextDrainIncrease += drainIncreaseInterval;

            Debug.Log($"Drain increased! Current drain: {drainRate}");
        }


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

        if (CurrentTime > PeakTime)
        {
            PeakTime = CurrentTime;
        }
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

    public void Freeze(float seconds)
    {
        freezeTimer = Mathf.Max(freezeTimer, seconds);

        Debug.Log($"Timer Frozen for {seconds} seconds.");
    }
}