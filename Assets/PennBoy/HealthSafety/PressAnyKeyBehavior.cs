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
    public GameObject repeatingBg;
    public Color flashColor;

    private float elapsed;
    private bool paused;

    // Update is called once per frame
    private void Update() {
        if (paused) return;

        pressAny.alpha = 0.4f * Mathf.Cos(elapsed * 5f) + 0.6f;
        elapsed += Time.deltaTime;
    }

    public void Flash() {
        keyBorder.color = flashColor;
        text.color = flashColor;
    }
}