using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSafetyStart : MonoBehaviour
{
    public List<GameObject> texts;
    public GameObject pressAnyBehavior;
    [Range(0f, 1f)] public float animTime;
    [Range(0f, 1f)] public float animDelay;

    private IEnumerator Start() {
        pressAnyBehavior.SetActive(false);

        foreach (var obj in texts) obj.GetComponent<CanvasGroup>().alpha = 0f;

        foreach (var obj in texts) {
            StartCoroutine(TranslateUpFadeIn(obj));
            yield return new WaitForSeconds(animDelay);
        }

        pressAnyBehavior.SetActive(true);
    }

    private IEnumerator TranslateUpFadeIn(GameObject obj) {
        var rectTransform = obj.GetComponent<RectTransform>();
        var canvasGroup = obj.GetComponent<CanvasGroup>();

        var finalYPos = rectTransform.anchoredPosition.y;
        var finalXPos = rectTransform.anchoredPosition.x;

        var elapsed = 0f;
        var initYPos = finalYPos - 100f;

        while (elapsed <= animTime) {
            var t = elapsed / animTime;
            t = Easing.EaseOutExpo(t);

            rectTransform.anchoredPosition = new Vector2(finalXPos, Mathf.Lerp(initYPos, finalYPos, t));
            canvasGroup.alpha = t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(finalXPos, finalYPos);
        canvasGroup.alpha = 1f;
    }
}