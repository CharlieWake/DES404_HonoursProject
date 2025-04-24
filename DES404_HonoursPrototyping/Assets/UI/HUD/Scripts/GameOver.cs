using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private TMP_Text gameOverText;
    private Image fadeImage;

    private float fadeDuration = 2f;
    private float textDelay = 0.2f;
    private float returnToMenuDelay = 2.5f;
    float targetAlpha = 250f / 255f;

    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image>();
        gameOverText = GetComponentInChildren<TMP_Text>();
    }

    public void CallGameOverScreen()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        Color color = fadeImage.color;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, targetAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        string message = "GAME\nOVER";
        gameOverText.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            gameOverText.text += message[i];
            yield return new WaitForSeconds(textDelay);
        }

        yield return new WaitForSeconds(returnToMenuDelay);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
