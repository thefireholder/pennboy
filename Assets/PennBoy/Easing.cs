using UnityEngine;

public static class Easing
{
    public static float EaseOutExpo(float t) {
        return Mathf.Approximately(t, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * t);
    }
}