using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 10f; 
    private float dampingSpeed = 2f;

    private float shakeFrequency = 20f;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            float xOffset = Mathf.Sin(Time.time * shakeFrequency) * shakeMagnitude;
            transform.localPosition = originalPosition + new Vector3(xOffset, 0f, 0f);

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPosition;
        }
    }

    public void TriggerShake(float duration = 0.2f)
    {
        shakeDuration = duration;
    }
}
