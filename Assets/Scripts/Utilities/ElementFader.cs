using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(CanvasGroup))]
public class ElementFader : MonoBehaviour
{
    public delegate void CallbackMethod();
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        elementToMove = GetComponent<RectTransform>();
    }

    public void FadeIn(float duration = 1, CallbackMethod callbackMethod = null, float callbackDelay = 0)
    {
        canvasGroup.blocksRaycasts= true;
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, callbackMethod, callbackDelay, duration));
    }

    public void FadeOut(float duration = 1, CallbackMethod callbackMethod = null, float callbackDelay = 0)
    {
        canvasGroup.blocksRaycasts = false;
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, callbackMethod, callbackDelay, duration));
    }

    public void SetAlpha(float i)
    {
        canvasGroup.alpha = i;
    }

    public float GetAlpha()
    {
        return canvasGroup.alpha;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, CallbackMethod callbackMethod = null, float callbackDelay = 0, float duration = 1f)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float timeRatio = (Time.time - startTime) / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeRatio);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        yield return new WaitForSeconds(callbackDelay);
        callbackMethod?.Invoke();
    }

    public RectTransform elementToMove;

    private Coroutine movementCoroutine;

    private Vector3 initialPosition;

    public void MoveElement(float direction, float moveSpeed = 100f, float moveDuration = 1f)
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        movementCoroutine = StartCoroutine(MoveElementCoroutine(direction, moveSpeed, moveDuration));
    }

    private IEnumerator MoveElementCoroutine(float direction, float moveSpeed, float moveDuration)
    {
        float timer = 0f;
        initialPosition = elementToMove.anchoredPosition;
        Vector2 targetPosition = new Vector2(initialPosition.x + (direction * moveSpeed * moveDuration), initialPosition.y);

        while (timer < moveDuration)
        {
            float deltaTime = Time.deltaTime;
            timer += deltaTime;

            float t = timer / moveDuration;
            Vector2 newPosition = Vector2.Lerp(initialPosition, targetPosition, t);
            elementToMove.anchoredPosition = newPosition;

            yield return null;
        }

        elementToMove.anchoredPosition = targetPosition;
    }

    public void ResetElement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        elementToMove.anchoredPosition = initialPosition;
    }

}
