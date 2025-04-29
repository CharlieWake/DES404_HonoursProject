using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonPulse : MonoBehaviour
{
    public bool isSelected = false;
    public float selectedScale = 1.1f;
    public float scaleSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    public Image glowImage;  // The glow effect
    public float glowFadeSpeed = 10f;
    public float maxAlpha = 0.6f;  // Maximum alpha value for glow

    private Color originalColour;

    public Image buttonImage;
    private float unselectedAlpha = 0.25f;
    private Color originalButtonColour;
    public TextMeshProUGUI buttonText;  // The TMP text on the button
    private float textUnselectedAlpha = 0.5f;
    private Color originalTextColour;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        // Ensure glowImage is assigned and initialize with no alpha (invisible)
        if (glowImage != null)
        {
            originalColour = glowImage.color;
            originalColour.a = 0f;  // Set initial alpha to 0 (invisible)
            glowImage.color = originalColour;
        }

        if (buttonImage != null)
        {
            originalButtonColour = buttonImage.color;
        }

        if (buttonText != null)
        {
            originalTextColour = buttonText.color;
        }
    }

    void Update()
    {
        // Scale the button
        targetScale = isSelected ? originalScale * selectedScale : originalScale;
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);

        // Update the glow effect if glowImage is assigned
        if (glowImage != null)
        {
            // Set glow alpha based on selection state
            float targetAlpha = isSelected ? maxAlpha : 0f;
            Color newColor = glowImage.color;
            newColor.a = Mathf.Lerp(glowImage.color.a, targetAlpha, Time.unscaledDeltaTime * glowFadeSpeed);
            glowImage.color = newColor;  // Update the glow image color with the new alpha
        }

        if (buttonImage != null)
        {
            float targetAlpha = isSelected ? 1f : unselectedAlpha;
            Color newColour = originalButtonColour;
            newColour.a = Mathf.Lerp(buttonImage.color.a, targetAlpha, Time.unscaledDeltaTime * glowFadeSpeed);
            buttonImage.color = newColour;
        }

        if (buttonText != null)
        {
            float targetAlpha = isSelected ? 1f : textUnselectedAlpha;
            Color newColor = originalTextColour;
            newColor.a = Mathf.Lerp(buttonText.color.a, targetAlpha, Time.unscaledDeltaTime * glowFadeSpeed);
            buttonText.color = newColor;
        }
    }

    // Reset pulse effect (called when button is no longer selected)
    public void ResetPulse()
    {
        // Reset scaling
        targetScale = originalScale;
        transform.localScale = originalScale;

        // Reset the glow effect (make it invisible)
        if (glowImage != null)
        {
            var resetColor = glowImage.color;
            resetColor.a = 0f;
            glowImage.color = resetColor;  // Set glow alpha back to 0
        }

        if (buttonImage != null)
        {
            var resetColour = buttonImage.color;
            resetColour.a = unselectedAlpha;
            buttonImage.color = resetColour;
        }

        if (buttonText != null)
        {
            var resetColor = buttonText.color;
            resetColor.a = textUnselectedAlpha;
            buttonText.color = resetColor;
        }
    }
}
