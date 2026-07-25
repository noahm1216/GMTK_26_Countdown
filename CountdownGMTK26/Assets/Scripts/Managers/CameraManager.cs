using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera gameplayCamera;
    [SerializeField] private TimerManager timer;


    [Header("FOV")]
    [SerializeField] private float safeFOV = 60f;
    [SerializeField] private float dangerFOV = 75f;


    [Header("Smoothing")]
    [SerializeField] private float cameraSpeed = 2f;


    private void Start()
    {
        gameplayCamera.Lens.FieldOfView = safeFOV;
    }


    private void Update()
    {
        if (timer.PeakTime <= 0)
            return;


        float timePercent =
            timer.CurrentTime / timer.PeakTime;


        timePercent = Mathf.Clamp01(timePercent);


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
}