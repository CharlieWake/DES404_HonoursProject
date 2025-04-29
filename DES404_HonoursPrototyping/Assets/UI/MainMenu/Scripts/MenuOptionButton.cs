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
        // AudioManager.instance.PlaySFXByName("Audio/SFX/")
        
        switch (optionName)
        {
            case "Start":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon_1");
                AudioManager.instance.PlayMusicByName("Audio/Music/Exploration");
                AudioManager.instance.PlayAmbienceByName("Audio/SFX/Dungeon Ambience Loop");
                break;
            case "Quit":
                Application.Quit();
                break;
            case "SpinnerSpeed":
                // Debug.Log("Setting Spinner Speed");
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    mainMenuCircle.gameObject.SetActive(false);
                    spinnerSpeedPanel.gameObject.SetActive(true);
                    break;
                }
                else
                {
                    pauseMenuPanel.gameObject.SetActive(false);
                    spinnerSpeedPanel.gameObject.SetActive(true);
                    break;
                }                    
            case "SlowSpeed":
                SettingsManager.instance.movementSpinnerSpeed = -30f;
                break;
            case "NormalSpeed":
                SettingsManager.instance.movementSpinnerSpeed = -50f;
                break;
            case "FastSpeed":
                SettingsManager.instance.movementSpinnerSpeed = -70f;
                break;
            case "ReturnMainMenu":
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    spinnerSpeedPanel.gameObject.SetActive(false);
                    mainMenuCircle.gameObject.SetActive(true);                    
                    break;
                }
                else
                {
                    spinnerSpeedPanel.gameObject.SetActive(false);
                    pauseMenuPanel.gameObject.SetActive(true);
                    break;
                }
            case "QuitLevel":
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                AudioManager.instance.PlayMusicByName("Audio/Music/MainMenu");
                break;
            case "ResumeGame":
                GameManager.instance.UnpauseGame();
                break;
            case "MuteUnmute":
                AudioManager.instance.MuteAudio();
                break;
            case "ConfirmChoice":
                if (confirmChoicePanel != null)
                {
                    confirmChoicePanel.SetActive(true);
                    spinnerSpeedPanel.gameObject.SetActive(false);
                }
                break;
            case "ReturnSplashScreen":
                if (spinnerSpeedPanel != null)
                {
                    spinnerSpeedPanel.gameObject.SetActive(true);
                    confirmChoicePanel.SetActive(false);
                }
                break;
            default:
                Debug.LogWarning("Unknown menu option");
                break;
        }
    }
}
