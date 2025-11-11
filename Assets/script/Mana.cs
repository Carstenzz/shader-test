using System.Collections;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 0.5f;
    private Material runtimeMaterial;
    private bool isTransitioning = false;
    private bool manaOn = false;

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        runtimeMaterial = renderer.material;

        // start fully transparent
        runtimeMaterial.SetFloat("_Opacity", 0f);
    }

    // This will be called from TileAnim
    public void PlayMana()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionMana());
        }
    }

    private IEnumerator TransitionMana()
    {
        isTransitioning = true;

        float startValue = manaOn ? 1f : 0f;
        float endValue = manaOn ? 0f : 1f;
        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = timer / transitionDuration;
            float opacity = Mathf.Lerp(startValue, endValue, t);

            runtimeMaterial.SetFloat("_Opacity", opacity);
            yield return null;
        }

        runtimeMaterial.SetFloat("_Opacity", endValue);

        manaOn = !manaOn;
        isTransitioning = false;
    }
}
