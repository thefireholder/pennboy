using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyBehavior : MonoBehaviour
{
    public CanvasGroup pressAny;
    public Image keyBorder;
    public TMP_Text text;
    public List<GameObject> texts;
    public RectTransform repeatingBg;

    private float elapsed;
    private bool transitioned;

    // Update is called once per frame
    private void Update() {
        if (transitioned) return;

        pressAny.alpha = 0.4f * Mathf.Cos(elapsed * 5f) + 0.6f;
        elapsed += Time.deltaTime;

        if (Input.anyKey) {
            transitioned = true;
            StartCoroutine(AnimateLogo());
        }
    }

    private IEnumerator ZoomOutBackground() {
        var elapsedTime = 0f;
        const float ANIM_TIME = 0.7f;

        var initScale = repeatingBg.localScale;
        var finalScale = new Vector3(1.5f, 1.5f, 1.5f);

        while (elapsedTime <= ANIM_TIME) {
            var t = elapsedTime / ANIM_TIME;
            t = Easing.EaseInOutQuint(t);
            repeatingBg.localScale = Vector3.Lerp(initScale, finalScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        repeatingBg.localScale = finalScale;
    }

    private IEnumerator AnimateLogo() {
        keyBorder.color = new Color(255f, 222f, 0f);
        text.color = new Color(255f, 222f, 0f);

        StartCoroutine(Fade());

        var elapsedTime = 0f;
        const float ANIM_TIME = 0.3f;

        while (elapsedTime <= ANIM_TIME) {
            var t = elapsedTime / ANIM_TIME;

            foreach (var obj in texts) obj.GetComponent<CanvasGroup>().alpha = 1f - t;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (var obj in texts) obj.GetComponent<CanvasGroup>().alpha = 0f;

        StartCoroutine(ZoomOutBackground());

        yield return new WaitForSeconds(0.35f);
    }

    private IEnumerator Fade() {
        var elapsedTime = 0f;
        const float ANIM_TIME = 1f;

        var initCol = new Color(255f, 222f, 0f);

        while (elapsedTime <= ANIM_TIME) {
            var t = elapsedTime / ANIM_TIME;
            var currCol = Color.Lerp(initCol, Color.white, t);

            keyBorder.color = currCol;
            text.color = currCol;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        keyBorder.color = Color.white;
        text.color = Color.white;
    }
}