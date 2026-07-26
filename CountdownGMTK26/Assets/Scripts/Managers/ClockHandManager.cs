using UnityEngine;

public class ClockHandManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimerManager timer;

    [Header("Clock Hands")]
    [SerializeField] private Transform mainHand;
    [SerializeField] private Transform smallHand;

    [Header("Rotation")]
    [SerializeField] private bool rotateClockwise = true;
    [SerializeField] private float mainHandSecondsPerRotation = 60f;
    [SerializeField] private float smallHandSecondsPerRotation = 30f;

    [Header("Jump Detection")]
    [SerializeField] private float suddenChangeThreshold = 0.15f;

    [Header("Smoothing")]
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private float rotationSmoothSpeed = 12f;

    private float visualClockTime;
    private float previousTimerTime;
    private float currentMainAngle;
    private float currentSmallAngle;
    private bool hasPreviousTimerTime;

    private void Update()
    {
        if (timer == null)
            return;

        if (!hasPreviousTimerTime)
        {
            previousTimerTime = timer.CurrentTime;
            hasPreviousTimerTime = true;
            return;
        }

        float timerDelta = timer.CurrentTime - previousTimerTime;

        bool timerChangedThisFrame = !Mathf.Approximately(timerDelta, 0f);

        if (timerChangedThisFrame)
        {
            visualClockTime += Time.deltaTime;

            if (Mathf.Abs(timerDelta) > suddenChangeThreshold)
            {
                if (timerDelta > 0f)
                {
                    visualClockTime += timerDelta;
                }
                else
                {
                    visualClockTime += timerDelta;

                    if (visualClockTime < 0f)
                        visualClockTime = 0f;
                }
            }
        }

        previousTimerTime = timer.CurrentTime;

        UpdateHands();
    }

    private void UpdateHands()
    {
        UpdateMainHand();
        UpdateSmallHand();
    }

    private void UpdateMainHand()
    {
        if (mainHand == null)
            return;

        float rotations = visualClockTime / mainHandSecondsPerRotation;
        float targetAngle = rotations * 360f;

        if (rotateClockwise)
            targetAngle *= -1f;

        if (smoothRotation)
        {
            currentMainAngle = Mathf.LerpAngle(
                currentMainAngle,
                targetAngle,
                rotationSmoothSpeed * Time.deltaTime
            );
        }
        else
        {
            currentMainAngle = targetAngle;
        }

        mainHand.localRotation = Quaternion.Euler(0f, 0f, currentMainAngle);
    }

    private void UpdateSmallHand()
    {
        if (smallHand == null)
            return;

        float rotations = visualClockTime / smallHandSecondsPerRotation;
        float targetAngle = rotations * 360f;

        if (rotateClockwise)
            targetAngle *= -1f;

        if (smoothRotation)
        {
            currentSmallAngle = Mathf.LerpAngle(
                currentSmallAngle,
                targetAngle,
                rotationSmoothSpeed * Time.deltaTime
            );
        }
        else
        {
            currentSmallAngle = targetAngle;
        }

        smallHand.localRotation = Quaternion.Euler(0f, 0f, currentSmallAngle);
    }
}