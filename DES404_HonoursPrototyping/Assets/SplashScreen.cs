using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private Image fullscreenImage;
    [SerializeField] private float fadeDuration;
    [SerializeField] private GameObject spinnerPanel;
    [SerializeField] private GameObject spinnerObject;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private TextMeshProUGUI startingText;
    [SerializeField] private OpenDoor openDoor;

    private bool tutorialStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && tutorialStarted == false)
        {
            tutorialStarted = true;
            AudioManager.instance.PlaySFXByName("Audio/SFX/UI-SelectPause");
            AudioManager.instance.PlayMusicByName("Audio/SFX/Dungeon Ambience Loop");
            startingText.enabled = false;
            StartCoroutine(FadeImage());
        }
        else if (Input.anyKeyDown && tutorialStarted == true)
        {
            Debug.Log("Tutorial already started");
        }
    }

    private IEnumerator FadeImage()
    {
        yield return new WaitForSeconds(1f);
        
        yield return openDoor.PlayAnimationAndWait("OpenDoor1", "OpenDoor2");

        yield return new WaitForSeconds(2f);
        
        Color colour = fullscreenImage.color;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / fadeDuration;
            colour.a = Mathf.Lerp(0, 1, t);
            fullscreenImage.color = colour;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        if (spinnerPanel != null)
        {
            spinnerPanel.SetActive(true);
            spinnerObject.SetActive(true);
        }

        yield return new WaitForSeconds(6f);

        // if (startGameButton != null)
        {
            // startGameButton.SetActive(true);
        }
    }
}
