using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuOptionButton : MonoBehaviour
{
    public string optionName;
    [SerializeField] private MenuSpinnerController MenuSpinnerController;

    [SerializeField] public Transform canvas;
    [SerializeField] public Transform mainMenuCircle;
    [SerializeField] public Transform spinnerSpeedPanel;
    [SerializeField] public Transform pauseMenuPanel;
    [SerializeField] private GameObject confirmChoicePanel;

    private void Start()
    {       

    }

    public void Press()
    {                         
        switch (optionName)
        {
            case "Start":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon_1");
                PlayUIForwardSFX();
                AudioManager.instance.PlayMusicByName("Audio/Music/Exploration");
                AudioManager.instance.PlayAmbienceByName("Audio/SFX/Dungeon Ambience Loop");
                break;
            case "Quit":
                PlayUIBackwardsSFX();
                Application.Quit();
                break;
            case "SpinnerSpeed":
                // Debug.Log("Setting Spinner Speed");
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    PlayUIForwardSFX();
                    mainMenuCircle.gameObject.SetActive(false);
                    spinnerSpeedPanel.gameObject.SetActive(true);
                    break;
                }
                else
                {
                    PlayUIForwardSFX();
                    pauseMenuPanel.gameObject.SetActive(false);
                    spinnerSpeedPanel.gameObject.SetActive(true);
                    break;
                }                    
            case "SlowSpeed":
                PlayUIForwardSFX();
                SettingsManager.instance.movementSpinnerSpeed = -30f;
                break;
            case "NormalSpeed":
                PlayUIForwardSFX();
                SettingsManager.instance.movementSpinnerSpeed = -70f;
                break;
            case "FastSpeed":
                PlayUIForwardSFX();
                SettingsManager.instance.movementSpinnerSpeed = -90f;
                break;
            case "ReturnMainMenu":
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    PlayUIBackwardsSFX();
                    spinnerSpeedPanel.gameObject.SetActive(false);
                    mainMenuCircle.gameObject.SetActive(true);                    
                    break;
                }
                else
                {
                    PlayUIBackwardsSFX();
                    spinnerSpeedPanel.gameObject.SetActive(false);
                    pauseMenuPanel.gameObject.SetActive(true);
                    break;
                }
            case "QuitLevel":
                PlayUIBackwardsSFX();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                AudioManager.instance.PlayMusicByName("Audio/Music/MainMenu");
                break;
            case "ResumeGame":
                PlayUIBackwardsSFX();
                GameManager.instance.UnpauseGame();
                break;
            case "MuteUnmute":
                PlayUIForwardSFX();
                AudioManager.instance.MuteAudio();
                break;
            case "ConfirmChoice":
                if (confirmChoicePanel != null)
                {
                    PlayUIForwardSFX();
                    confirmChoicePanel.SetActive(true);
                    spinnerSpeedPanel.gameObject.SetActive(false);
                }
                break;
            case "ReturnSplashScreen":
                if (spinnerSpeedPanel != null)
                {
                    PlayUIBackwardsSFX();
                    spinnerSpeedPanel.gameObject.SetActive(true);
                    confirmChoicePanel.SetActive(false);
                }
                break;
            default:
                Debug.LogWarning("Unknown menu option");
                break;
        }
    }

    private void PlayUIForwardSFX()
    {
        AudioManager.instance.PlaySFXByName("Audio/SFX/UI-SelectPause");
    }

    private void PlayUIBackwardsSFX()
    {
        AudioManager.instance.PlaySFXByName("Audio/SFX/UI-BackResume");
    }
}
