using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausingTutorial : MonoBehaviour
{
    public CanvasGroup uiGroup;
    public float fadeDuration = 1f;
    public Vector3 targetPlayerPosition = new Vector3(0.5f, -0.5f, 0f);

    private bool playerAtPosition = false;

    private bool isFadingOut = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAtPosition == false && GameManager.instance.PlayerController.transform.position == targetPlayerPosition && SettingsManager.instance.pauseTutorialSeen == false)
        {
            Debug.Log("Player in position, fading in tutorial");
            playerAtPosition = true;
            StartCoroutine(FadeInCanvasGroup());
        }
    }

    public void FadeInPopup()
    {
        StartCoroutine(FadeInCanvasGroup());
    }

    public void FadeOutPopup()
    {
        if (isFadingOut == false)
        {
            StartCoroutine(FadeOutCanvasGroup());
            isFadingOut = true;
        }

    }

    private IEnumerator FadeInCanvasGroup()
    {
        yield return new WaitForSeconds(3f);

        float time = 0f;

        while (time < fadeDuration)
        {
            uiGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        uiGroup.alpha = 1f;
        SettingsManager.instance.pauseTutorialSeen = true;

        yield return new WaitForSeconds(3f);
        FadeOutPopup();
    }

    private IEnumerator FadeOutCanvasGroup()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            uiGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        uiGroup.alpha = 0f;
    }
}
