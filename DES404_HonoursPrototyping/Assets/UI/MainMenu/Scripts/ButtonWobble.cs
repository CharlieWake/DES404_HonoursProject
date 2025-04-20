using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWobble : MonoBehaviour
{
    private float swayAmount = 1.5f;
    private float swaySpeed = 1.25f;

    private float startRotationZ;

    // Start is called before the first frame update
    void Start()
    {
        startRotationZ = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        float sway = Mathf.Sin(Time.unscaledTime * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0f, 0f, startRotationZ + sway);
    }
}
