using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI statsText;

    // Start is called before the first frame update
    void Start()
    {
        statsText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowLevelUpText(int oldLevel, int newLevel, float oldMaxHealth, float newMaxHealth, int oldActions, int newActions)
    {
        Time.timeScale = 0f;
        GameManager.instance.isGamePaused = true;
        
        gameObject.SetActive(true);

        statsText.text = $"Level: {oldLevel} > {newLevel}\n" +
                         $"HP: {oldMaxHealth} > {newMaxHealth}\n" +
                         $"Actions: {oldActions} > {newActions}";

        StartCoroutine(HidePanelDelay(3f));
    }

    IEnumerator HidePanelDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        GameManager.instance.isGamePaused = false;
        gameObject.SetActive(false);
    }
}
