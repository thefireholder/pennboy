using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyBehavior : MonoBehaviour
{
    public CanvasGroup pressAny;
    public Image keyBorder;
    public TMP_Text text;

    private float elapsed;

    // Update is called once per frame
    private void Update() {
        pressAny.alpha = 0.4f * Mathf.Cos(elapsed * 5f) + 0.6f;
        elapsed += Time.deltaTime;

        if (Input.anyKey) {
            keyBorder.color = new Color(255f, 222f, 0f);
            text.color = new Color(255f, 222f, 0f);
        }
    }
}