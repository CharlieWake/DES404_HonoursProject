using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchFlicker : MonoBehaviour
{
    public Light2D torchLight;
    public float intensityMin = 1.8f;
    public float intensityMax = 2.2f;
    public float flickerSpeed = 0.1f;

    private float timer;

    Vector3 originalPos;

    void Start()
    {
        if (torchLight == null)
            torchLight = GetComponent<Light2D>();

        originalPos = transform.localPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= flickerSpeed)
        {
            // Slight random intensity change
            torchLight.intensity = Random.Range(intensityMin, intensityMax);

            transform.localPosition = originalPos + new Vector3(
           Random.Range(-0.01f, 0.01f),
           Random.Range(-0.01f, 0.01f),
           0f
       );

            timer = 0f;
        }
    }
}

