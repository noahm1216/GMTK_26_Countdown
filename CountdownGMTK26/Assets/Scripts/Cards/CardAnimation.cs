using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Animation")]
    [SerializeField] private float enterSpeed = 8f;
    [SerializeField] private float exitSpeed = 8f;

    [SerializeField] private float popScale = 1.15f;

    private Vector3 originalPosition;
    private Vector3 baseScale;
    private Vector3 restingScale = Vector3.one;
    private bool isHovered;
    private bool suppressHoverUntilExit;

    private CanvasGroup canvasGroup;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        baseScale = transform.localScale;
        restingScale = baseScale;
    }


    public void AnimateEnter(Vector3 startPosition)
    {
        transform.position = startPosition;
        StartCoroutine(EnterAnimation());
    }


    public void AnimateExit(Vector3 targetPosition, System.Action onFinished)
    {
        StartCoroutine(ExitAnimation(targetPosition, onFinished));
    }

    public void AnimateDestroy(System.Action finished)
    {
        StartCoroutine(DestroyRoutine(finished));
    }

    private IEnumerator DestroyRoutine(System.Action finished)
    {
        float t = 0;

        Vector3 startScale = transform.localScale;

        while (t < 1)
        {
            t += Time.deltaTime * exitSpeed;

            transform.localScale =
                Vector3.Lerp(
                    startScale,
                    Vector3.zero,
                    t);

            canvasGroup.alpha = 1 - t;

            yield return null;
        }

        finished?.Invoke();

        Destroy(gameObject);
    }

    private IEnumerator EnterAnimation()
    {
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;


        float progress = 0;


        while (progress < 1)
        {
            progress += Time.deltaTime * enterSpeed;


            transform.localScale =
                Vector3.Lerp(
                    Vector3.zero,
                    restingScale,
                    progress);


            canvasGroup.alpha = progress;


            yield return null;
        }


        transform.localScale = restingScale;
    }


    private IEnumerator ExitAnimation(Vector3 target, System.Action onFinished)
    {
        Vector3 start = transform.position;

        float progress = 0;


        while (progress < 1)
        {
            progress += Time.deltaTime * exitSpeed;


            transform.position =
                Vector3.Lerp(
                    start,
                    target,
                    progress);


            transform.localScale =
                Vector3.Lerp(
                    Vector3.one,
                    Vector3.zero,
                    progress);


            canvasGroup.alpha =
                Mathf.Lerp(
                    1,
                    0,
                    progress);


            yield return null;
        }

        onFinished?.Invoke();
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isHovered || suppressHoverUntilExit)
            return;

        isHovered = true;

        originalPosition = transform.position;
        transform.position += Vector3.up * 20f;
        transform.localScale = baseScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        suppressHoverUntilExit = false;
        ResetHoverState();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        suppressHoverUntilExit = true;
        ResetHoverState();
    }

    public void ResetHoverState()
    {
        if (isHovered)
        {
            transform.position = originalPosition;
            isHovered = false;
        }

        transform.localScale = baseScale;
    }

    private void OnDisable()
    {
        suppressHoverUntilExit = false;
        ResetHoverState();
    }

}