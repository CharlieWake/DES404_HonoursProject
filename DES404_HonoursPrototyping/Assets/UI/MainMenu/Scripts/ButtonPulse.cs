using UnityEngine;
using UnityEngine.UI;

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

    private float swayAmount = 1.5f;
    private float swaySpeed = 1.25f;

    private float startRotationZ;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        startRotationZ = transform.eulerAngles.z;

        // Ensure glowImage is assigned and initialize with no alpha (invisible)
        if (glowImage != null)
        {
            originalColour = glowImage.color;
            originalColour.a = 0f;  // Set initial alpha to 0 (invisible)
            glowImage.color = originalColour;
        }
    }

    void Update()
    {
        // Add wobble effect to the button
        float sway = Mathf.Sin(Time.unscaledTime * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0f, 0f, startRotationZ + sway);

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
    }
}
