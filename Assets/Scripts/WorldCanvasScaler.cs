using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class WorldCanvasAutoScaler : MonoBehaviour
{
    public Vector2 referenceResolution = new Vector2(1920, 1080);
    private Vector3 baseScale = new Vector3(0.001f, 0.001f, 0.001f); // Scale in world units

    void Update()
    {
        float scaleX = Screen.width / referenceResolution.x;
        float scaleY = Screen.height / referenceResolution.y;
        float scale = Mathf.Min(scaleX, scaleY); // Uniform scale

        transform.localScale = baseScale * scale;
    }
}
