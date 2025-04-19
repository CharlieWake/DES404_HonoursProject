using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpinnerController : MonoBehaviour
{
    public float orbitRadius = 140f;
    public float spinSpeed = -60f;

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
        rectTransform = GetComponent<RectTransform>();
        centerRect = transform.parent.GetComponent<RectTransform>();
    }

    void Update()
    {
        currentAngle += spinSpeed * Time.unscaledDeltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;

        // Orbit position (around center)
        float x = Mathf.Cos(radians) * orbitRadius;
        float y = Mathf.Sin(radians) * orbitRadius;

        rectTransform.anchoredPosition = new Vector2(x, y);

        // Point outward from center
        Vector2 directionFromCenter = rectTransform.anchoredPosition.normalized;
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
}