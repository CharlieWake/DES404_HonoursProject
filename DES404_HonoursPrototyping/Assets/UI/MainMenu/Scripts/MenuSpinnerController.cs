using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpinnerController : MonoBehaviour
{
    public float orbitRadius = 140f;
    public float spinSpeed;

    private float currentAngle = 0f;
    private RectTransform rectTransform;
    private RectTransform centerRect;

    private MenuOptionButton currentButton;
    private ButtonPulse buttonPulse;

    public float buttonPulseSpeed = 2f;
    public float minPulseScale = 0.95f;
    public float maxPulseScale = 1.05f;



    void Start()
    {
        if (SettingsManager.instance != null)
        {
            spinSpeed = SettingsManager.instance.movementSpinnerSpeed;
        }
        else
        {
            spinSpeed = -50f;
        }
      
        rectTransform = GetComponent<RectTransform>();
        centerRect = transform.parent.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (SettingsManager.instance != null)
            UpdateSpinnerSpeed();

        currentAngle += spinSpeed * Time.unscaledDeltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;

        // Orbit position (around shifted center)
        float x = Mathf.Cos(radians) * orbitRadius;
        float y = Mathf.Sin(radians) * orbitRadius;

        Vector2 offsetPosition = new Vector2(x, y - 110f);
        Vector2 centerOffset = new Vector2(0f, -110f);

        rectTransform.anchoredPosition = offsetPosition;

        // Corrected direction
        Vector2 directionFromCenter = offsetPosition - centerOffset;
        directionFromCenter.Normalize();

        float angle = Mathf.Atan2(directionFromCenter.y, directionFromCenter.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentButton = other.GetComponent<MenuOptionButton>();
        buttonPulse = other.GetComponent<ButtonPulse>();
        buttonPulse.isSelected = true;
        Debug.Log("Entered a button!");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentButton != null && other.gameObject == currentButton.gameObject)
        {
            currentButton = null;
            buttonPulse.isSelected = false;
        }
    }

    public void ButtonPress()
    {
        currentButton?.Press();
    }

    private void UpdateSpinnerSpeed()
    {
        spinSpeed = SettingsManager.instance.movementSpinnerSpeed;
    }
}