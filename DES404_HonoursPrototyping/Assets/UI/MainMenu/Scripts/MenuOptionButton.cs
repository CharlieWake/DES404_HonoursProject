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

    public Transform canvas;
    public Transform mainMenuCircle;
    public Transform spinnerSpeedCircle;
    public Transform pauseMenuCircle;

    private void Start()
    {
        

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            canvas = GameObject.Find("MenuCanvas").transform;
            mainMenuCircle = canvas.Find("MainMenuCircle");
            spinnerSpeedCircle = canvas.Find("SpinnerSpeedCircle");
        }
        else
        {
            canvas = GameObject.Find("HUD").transform;
            pauseMenuCircle = canvas.Find("PauseMenuPanel/PauseMenuCircle");
            spinnerSpeedCircle = canvas.Find("SpinnerSpeedCircle");
        }
    }

    public void Press()
    {              
        switch (optionName)
        {
            case "Start":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon_1");
                break;
            case "Quit":
                Application.Quit();
                break;
            case "SpinnerSpeed":
                // Debug.Log("Setting Spinner Speed");
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    mainMenuCircle.gameObject.SetActive(false);
                    spinnerSpeedCircle.gameObject.SetActive(true);
                    break;
                }
                else
                {
                    pauseMenuCircle.gameObject.SetActive(false);
                    spinnerSpeedCircle.gameObject.SetActive(true);
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
                    spinnerSpeedCircle.gameObject.SetActive(false);
                    mainMenuCircle.gameObject.SetActive(true);                    
                    break;
                }
                else
                {
                    spinnerSpeedCircle.gameObject.SetActive(false);
                    pauseMenuCircle.gameObject.SetActive(true);
                    break;
                }

            case "QuitLevel":
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                break;
            case "ResumeGame":
                GameManager.instance.UnpauseGame();
                break;
            default:
                Debug.LogWarning("Unknown menu option");
                break;
        }
    }
}
