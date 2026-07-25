using System.Collections;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float enterSpeed = 8f;
    [SerializeField] private float exitSpeed = 8f;

    [SerializeField] private float popScale = 1.15f;

    private CanvasGroup canvasGroup;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
                    Vector3.one * popScale,
                    progress);


            canvasGroup.alpha = progress;


            yield return null;
        }


        transform.localScale = Vector3.one;
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
}