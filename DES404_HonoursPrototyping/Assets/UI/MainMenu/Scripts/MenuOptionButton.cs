using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuOptionButton : MonoBehaviour
{
    public string optionName;

    public void Press()
    {
        Debug.Log("Press on Menu Option Button Triggered");
        
        switch (optionName)
        {
            case "Start":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon_1");
                break;
            case "Quit":
                Application.Quit();
                break;
            case "SpinnerSpeed":
                Debug.Log("Setting Spinner Speed");
                break;
            default:
                Debug.LogWarning("Unknown menu option");
                break;
        }
    }
}
