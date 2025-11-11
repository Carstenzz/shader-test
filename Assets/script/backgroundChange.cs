using System;
using UnityEngine;

public class BackgroundChange : MonoBehaviour
{
    [SerializeField] private string white = "#CFAE73";
    [SerializeField] private string black = "#4e2c15";
    [SerializeField] private float transitionDuration = 1.5f;

    private Material runtimeMaterial;  // private instance
    private Color colorWhite;
    private Color colorBlack;
    private Color startColor;
    private Color targetColor;
    private float transitionTimer = 0f;
    private bool isTransitioning = false;
    private bool isWhiteTurn = true;

    void Start()
    {
        // Create a runtime instance of the material so it doesn't affect others
        var renderer = GetComponent<Renderer>();
        runtimeMaterial = renderer.material;

        ColorUtility.TryParseHtmlString(white, out colorWhite);
        ColorUtility.TryParseHtmlString(black, out colorBlack);

        startColor = colorWhite;
        targetColor = colorWhite;

        runtimeMaterial.SetColor("_MainColor", targetColor);
        runtimeMaterial.SetColor("_AccentColor", targetColor);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isWhiteTurn = !isWhiteTurn;
            startColor = runtimeMaterial.GetColor("_MainColor");
            targetColor = isWhiteTurn ? colorWhite : colorBlack;
            transitionTimer = 0f;
            isTransitioning = true;
        }

        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            Color newColor = Color.Lerp(startColor, targetColor, t);

            runtimeMaterial.SetColor("_MainColor", newColor);
            runtimeMaterial.SetColor("_AccentColor", newColor);

            if (t >= 1f)
                isTransitioning = false;
        }
    }
}
