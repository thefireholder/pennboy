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
            var rectTrans = obj.GetComponent<RectTransform>();
            var canvasGroup = obj.GetComponent<CanvasGroup>();

            var finalPos = rectTrans.anchoredPosition;
            var initYPos = finalPos.y - 100f;

            StartCoroutine(Anim.Animate(animTime, t => {
                canvasGroup.alpha = t;
                rectTrans.anchoredPosition =
                    new Vector2(finalPos.x, Mathf.Lerp(initYPos, finalPos.y, Easing.EaseOutExpo(t)));
            }));

            yield return new WaitForSeconds(animDelay);
        }

        pressAnyBehavior.SetActive(true);
    }
}