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


    public void AnimateEnter()
    {
        StartCoroutine(EnterAnimation());
    }


    public void AnimateExit(Vector3 targetPosition)
    {
        StartCoroutine(ExitAnimation(targetPosition));
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


    private IEnumerator ExitAnimation(Vector3 target)
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


        Destroy(gameObject);
    }
}