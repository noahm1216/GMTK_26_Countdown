using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TimerManager timer;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text timerOutlineText;

    [SerializeField] public TMP_ColorGradient defaultGradient;
    [SerializeField] public TMP_ColorGradient subtractGradient;
    [SerializeField] public TMP_ColorGradient freezeGradient;
    [SerializeField] public TMP_ColorGradient addGradient;
    [SerializeField] public TMP_ColorGradient orangeGradient;
    [SerializeField] public TMP_ColorGradient purpleGradient;
    
    [SerializeField] public float colorGradientShiftTime;
    [SerializeField] public float flashGradientInTime;
    [SerializeField] public float flashGradientHoldTime;
    [SerializeField] public float flashGradientOutTime;

    private TMP_ColorGradient currentGradient;

    private Coroutine currentRoutine;

    private void Awake()
    {
        currentGradient = defaultGradient;
    }

    private void Update()
    {
        timerText.text = timer.CurrentTime.ToString("0.0");
        timerOutlineText.text = timerText.text;
        
        if (timer.freezeTimer > 0)
        {
            
        }      
    }

    public void FlashColorGradient(TMP_ColorGradient targetGradient, float flashHoldTime)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        
        currentRoutine = StartCoroutine(FlashColorGradientRoutine(targetGradient, flashGradientInTime, flashHoldTime, flashGradientOutTime));
    }

    public void ChangeColorGradient(TMP_ColorGradient targetGradient)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        
        currentRoutine = StartCoroutine(ChangeColorGradientRoutine(targetGradient, colorGradientShiftTime));
    }

    IEnumerator ChangeColorGradientRoutine(TMP_ColorGradient targetGradient, float lerpTime)
    {
        TMP_ColorGradient temp = new TMP_ColorGradient();
        float t = 0;
        while(t < lerpTime)
        {
            temp.topLeft = Color.Lerp(currentGradient.topLeft, targetGradient.topLeft, t / lerpTime);
            temp.bottomLeft = Color.Lerp(currentGradient.bottomLeft, targetGradient.bottomLeft, t / lerpTime);
            temp.topRight = Color.Lerp(currentGradient.topRight, targetGradient.topRight, t / lerpTime);
            temp.bottomRight = Color.Lerp(currentGradient.bottomRight, targetGradient.bottomRight, t / lerpTime);

            timerText.colorGradientPreset = temp;
            
            t += Time.deltaTime;
        }
        yield return null;

        // timerText.colorGradientPreset = targetGradient;
        // currentGradient = targetGradient;
    }

    IEnumerator FlashColorGradientRoutine(TMP_ColorGradient targetGradient, float inTime, float holdTime, float outTime)
    {
        Debug.Log("flash color gradient in");
        yield return ChangeColorGradientRoutine(targetGradient, inTime);

        if (holdTime > 0)
        {
            Debug.Log("Holding");
            yield return new WaitForSeconds(holdTime);
        }
        
        Debug.Log("change back to default");
        yield return ChangeColorGradientRoutine(defaultGradient, outTime);

        currentRoutine = null;
    }
}