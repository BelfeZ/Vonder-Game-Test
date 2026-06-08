using TMPro;
using UnityEngine;
using System.Collections;

public class BannerManager : MonoBehaviour
{
    public static BannerManager Instance;

    public CanvasGroup canvasGroup;
    public TextMeshProUGUI bannerText;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float showDuration = 5f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        Instance = this;

        canvasGroup.alpha = 0;
    }

    public void ShowBanner(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(BannerRoutine(message));
    }

    private IEnumerator BannerRoutine(string message)
    {
        canvasGroup.alpha = 1;
        bannerText.text = message;

        yield return new WaitForSeconds(showDuration);

        yield return Fade(1f, 0f);

        canvasGroup.alpha = 0;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(from, to, elapsed / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = to;
    }
}