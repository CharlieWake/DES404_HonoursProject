using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    public CanvasGroup uiGroup;
    public float fadeDuration = 1f;
    public Vector3 targetPlayerPosition = new Vector3(-2.5f, -0.5f, 0f);

    private bool playerAtPosition = false;


    // Start is called before the first frame update
    void Start()
    {
        FadeInPopup();
    }

    public void FadeInPopup()
    {
        StartCoroutine(FadeInCanvasGroup());
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



    // Update is called once per frame
    void Update()
    {
        if (playerAtPosition == false && GameManager.instance.PlayerController.transform.position == targetPlayerPosition)
        {
            Debug.Log("Player in position, fading out tutorial");
            playerAtPosition = true;
            StartCoroutine(FadeOutCanvasGroup());
        }
    }
}
