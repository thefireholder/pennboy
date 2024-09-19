using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Anim;

public class PressAnyBehavior : MonoBehaviour
{
    public CanvasGroup pressAny;
    public Image keyBorder;
    public TMP_Text text;
    public List<GameObject> texts;
    public GameObject repeatingBg;

    private float elapsed;
    private bool transitioned;

    // Update is called once per frame
    private void Update() {
        if (transitioned) return;

        pressAny.alpha = 0.4f * Mathf.Sin(elapsed * 5f) + 0.6f;
        elapsed += Time.deltaTime;

        if (Input.anyKey) {
            transitioned = true;
            StartCoroutine(AnimateLogo());
        }
    }

    private IEnumerator AnimateLogo() {
        // Flash yellow
        var flashColor = new Color(255f, 222f, 0f);
        keyBorder.color = flashColor;
        text.color = flashColor;

        // Fade "Press any key..." text back to white after flashing to yellow
        StartCoroutine(Animate(1f, t => {
            var currCol = Color.Lerp(flashColor, Color.white, t);
            keyBorder.color = currCol;
            text.color = currCol;
        }));

        // Fade out all the text
        var canvasGroups = texts.Select(obj => obj.GetComponent<CanvasGroup>());
        StartCoroutine(Animate(0.3f, t => {
            foreach (var canvasGroup in canvasGroups) canvasGroup.alpha = 1f - t;
        }));

        // Zoom out background
        var rectTransform = repeatingBg.GetComponent<RectTransform>();
        var initScale = rectTransform.localScale;
        var finalScale = new Vector3(1.5f, 1.5f, 1.5f);
        StartCoroutine(Animate(0.7f, t => {
            t = Easing.EaseInOutQuint(t);
            rectTransform.localScale = Vector3.Lerp(initScale, finalScale, t);
        }));

        yield return new WaitForSeconds(0.35f);

        // Fade background to white
        var bgImage = repeatingBg.GetComponent<Image>();
        var initBgCol = bgImage.color;
        StartCoroutine(Animate(0.3f, t => { bgImage.color = Color.Lerp(initBgCol, Color.white, t); }));
    }
}