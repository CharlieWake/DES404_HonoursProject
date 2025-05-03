using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Contracts;

public class PauseCharging : MonoBehaviour
{
    public Image radialFillImage;
    public TextMeshProUGUI pausingText;
    public float holdingTime;

    private float holdTimer = 0f;
    private bool isHolding = false;

    private CanvasGroup canvasGroup;

    [SerializeField] private PausingTutorial pauseTutorialPopup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding && !GameManager.instance.isGamePaused)
        {
            holdTimer += Time.unscaledDeltaTime;
            float fillAmount = Mathf.Clamp01(holdTimer / holdingTime);
            radialFillImage.fillAmount = fillAmount;

            if (fillAmount == 1f)
            {
                PauseGame();
                CancelHold();
                gameObject.SetActive(false);
            }
        }
        else if (!isHolding && radialFillImage.fillAmount > 0f)
        {
            radialFillImage.fillAmount -= Time.unscaledDeltaTime / holdingTime;

            if (radialFillImage.fillAmount <= 0f)
            {
                radialFillImage.fillAmount = 0f;
                StartCoroutine(FadeOut());
            }
        }      
    }

    public void StartHold()
    {
        if (GameManager.instance.isGamePaused) return;

        gameObject.SetActive(true);        
        isHolding = true;
        holdTimer = 0f;
        radialFillImage.fillAmount = 0f;   
        
        StartCoroutine(FadeIn());
    }

    public void CancelHold()
    {
        isHolding = false;
        holdTimer = 0f;
        // gameObject.SetActive(false);
    }

    private void PauseGame()
    {
        AudioManager.instance.PlaySFXByName("Audio/SFX/UI-SelectPause");
        GameManager.instance.PauseGame();
        if (pauseTutorialPopup != null)
        {
            pauseTutorialPopup.FadeOutPopup();
        }
    }

    IEnumerator FadeIn()
    {
        float fadeDuration = 0.2f;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float fadeDuration = 0.2f;
        float t = 0f;
        float startingAlpha = canvasGroup.alpha;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startingAlpha, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
