using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeSpeed = 1f;
    public float popDuration = 0.75f;

    private TextMeshProUGUI textMesh;
    private Color startingColour;

    private Vector3 initialScale;
    private float timeAlive = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        startingColour = textMesh.color;

        initialScale = transform.localScale;
        transform.localScale = Vector3.zero;

        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;

        // Scale pop animation
        if (timeAlive < popDuration)
        {
            float t = timeAlive / popDuration;
            float scale = Mathf.Lerp(0f, 2f, t);
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else if (timeAlive < popDuration + 0.1f)
        {
            // Shrink slightly back to normal
            float t = (timeAlive - popDuration) / 0.1f;
            float scale = Mathf.Lerp(2f, 1f, t);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        Color color = textMesh.color;
        color.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = color;
    }

    public void setText(string text)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();

        textMesh.text = text;
    }
}
