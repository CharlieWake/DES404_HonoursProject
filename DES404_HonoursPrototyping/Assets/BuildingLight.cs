using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BuildingLight : MonoBehaviour
{

    private Light2D Light2D;

    // Start is called before the first frame update
    void Start()
    {
        Light2D = GetComponent<Light2D>();
        Light2D.intensity = 0.01f;
    }
}
