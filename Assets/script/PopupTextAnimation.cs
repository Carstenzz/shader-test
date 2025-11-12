using System.Collections;
using UnityEngine;

public class PopupTextAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float delayBetweenLetters = 0.05f;
    public float popUpDuration = 0.1f;
    public float settleDuration = 1.5f;
    public float popUpScale = 1.1f;
    public float popUpHeight = 0.1f;

    private Transform[] letters;
    private Vector3[] originalPositions;
    private Vector3[] originalScales;

    void Start()
    {
        // Get all child transforms (letters)
        int count = transform.childCount;
        letters = new Transform[count];
        originalPositions = new Vector3[count];
        originalScales = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            letters[i] = transform.GetChild(i);
            originalPositions[i] = letters[i].localPosition;
            originalScales[i] = letters[i].localScale;
            letters[i].localScale = Vector3.zero;
        }

        StartCoroutine(AnimateLetters());
    }

    IEnumerator AnimateLetters()
    {
        for (int i = 0; i < letters.Length; i++)
        {
            StartCoroutine(AnimateLetter(letters[i], originalPositions[i], originalScales[i]));
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }

    IEnumerator AnimateLetter(Transform letter, Vector3 originalPos, Vector3 originalScale)
    {
        float timer = 0f;

        // Phase 1: Pop up (scale from 0 to popUpScale, move up)
        while (timer < popUpDuration)
        {
            float t = timer / popUpDuration;
            letter.localScale = Vector3.Lerp(Vector3.zero, originalScale * popUpScale, t);
            letter.localPosition = Vector3.Lerp(originalPos, originalPos + Vector3.up * popUpHeight, t);
            timer += Time.deltaTime;
            yield return null;
        }

        // Fix overshoot position
        letter.localScale = originalScale * popUpScale;
        letter.localPosition = originalPos + Vector3.up * popUpHeight;

        timer = 0f;

        // Phase 2: Settle back (scale down to normal, move back to original position)
        while (timer < settleDuration)
        {
            float t = timer / settleDuration;
            letter.localScale = Vector3.Lerp(originalScale * popUpScale, originalScale, t);
            letter.localPosition = Vector3.Lerp(originalPos + Vector3.up * popUpHeight, originalPos, t);
            timer += Time.deltaTime;
            yield return null;
        }

        letter.localScale = originalScale;
        letter.localPosition = originalPos;
    }
}
