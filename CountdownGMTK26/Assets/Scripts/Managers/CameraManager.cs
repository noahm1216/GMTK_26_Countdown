using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera gameplayCamera;
    [SerializeField] private TimerManager timer;


    [Header("FOV Settings")]
    [SerializeField] private float safeFOV = 60f;
    [SerializeField] private float dangerFOV = 75f;

    [SerializeField] private float cameraSpeed = 2f;


    [Header("Panic Thresholds")]
    [SerializeField] private float dangerThreshold = 0.25f;
    [SerializeField] private float criticalThreshold = 0.10f;


    [Header("Noise + Amp")]
    [SerializeField] private CinemachineBasicMultiChannelPerlin noise;

    [SerializeField] private float dangerNoise = 0.3f;
    [SerializeField] private float dangerAmp = 1f;
    [SerializeField] private float criticalNoise = 0.7f;
    [SerializeField] private float criticalAmp = 2f;

    [SerializeField] private float noiseSpeed = 2f;


    public bool IsDanger { get; private set; }
    public bool IsCritical { get; private set; }


    private void Start()
    {
        gameplayCamera.Lens.FieldOfView = safeFOV;

        if (noise)
        {
            noise.AmplitudeGain = 0;
            noise.FrequencyGain = 0;
        }
    }


    private void Update()
    {
        if (timer.PeakTime <= 0)
            return;


        float timePercent =
            timer.CurrentTime / timer.PeakTime;


        timePercent = Mathf.Clamp01(timePercent);


        UpdateFOV(timePercent);

        UpdatePanicState(timePercent);

        UpdateNoise();
    }


    private void UpdateFOV(float timePercent)
    {
        float targetFOV =
            Mathf.Lerp(
                dangerFOV,
                safeFOV,
                timePercent);


        gameplayCamera.Lens.FieldOfView =
            Mathf.Lerp(
                gameplayCamera.Lens.FieldOfView,
                targetFOV,
                Time.deltaTime * cameraSpeed);
    }


    private void UpdatePanicState(float timePercent)
    {
        IsDanger =
            timePercent <= dangerThreshold;

        IsCritical =
            timePercent <= criticalThreshold;
    }


    private void UpdateNoise()
    {
        if (noise == null)
            return;


        float targetAmplitude = 0;
        float targetFrequency = 0;


        if (IsCritical)
        {
            targetAmplitude = criticalNoise;
            targetFrequency = criticalAmp;
        }
        else if (IsDanger)
        {
            targetAmplitude = dangerNoise;
            targetFrequency = dangerAmp;
        }


        noise.AmplitudeGain =
            Mathf.Lerp(
                noise.AmplitudeGain,
                targetAmplitude,
                Time.deltaTime * noiseSpeed);


        noise.FrequencyGain =
            Mathf.Lerp(
                noise.FrequencyGain,
                targetFrequency,
                Time.deltaTime * noiseSpeed);
    }
}