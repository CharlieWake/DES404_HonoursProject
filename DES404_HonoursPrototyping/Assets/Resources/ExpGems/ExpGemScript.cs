using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpGemScript : MonoBehaviour
{
    private RectTransform targetUI;

    private Vector3 startPoint;
    private Vector3 scatterTarget;
    private Vector3 endPoint;
    private Vector3 controlPoint;

    private float scatterTime = 0.2f;     // Time to scatter outward
    private float zoomTime = 0.4f;        // Time to zoom toward the XP bar
    private float scatterElapsed = 0f;
    private float zoomElapsed = 0f;

    private bool isScattering = true;
    private bool isZooming = false;

    public void Initialize(Vector3 spawnPosition)
    {
        startPoint = spawnPosition;

        // Phase 1: Calculate scatter offset direction
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float scatterDistance = Random.Range(50f, 150f); // Adjust as needed
        scatterTarget = startPoint + (Vector3)(randomDir * scatterDistance);

        // Phase 2: Get the XP bar destination
        GameObject targetObject = GameObject.Find("XPBar");
        if (targetObject != null)
        {
            targetUI = targetObject.GetComponent<RectTransform>();
            endPoint = targetUI.position;
        }
    }

    void Update()
    {
        if (isScattering)
        {
            scatterElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(scatterElapsed / scatterTime);
            transform.position = Vector3.Lerp(startPoint, scatterTarget, t);

            if (t >= 1f)
            {
                isScattering = false;
                isZooming = true;
                zoomElapsed = 0f;

                // Set up the curved path now that the gem has scattered
                Vector3 offset = new Vector3(Random.Range(-100f, 100f), Random.Range(50f, 150f), 0f);
                controlPoint = transform.position + offset;
                startPoint = transform.position; // New curve start point
            }

            return;
        }

        if (isZooming)
        {
            zoomElapsed += Time.deltaTime;
            float linearT = Mathf.Clamp01(zoomElapsed / zoomTime);
            float t = Mathf.Pow(linearT, 3f); // Ease-in for speed-up

            Vector3 position = Mathf.Pow(1 - t, 2) * startPoint
                             + 2 * (1 - t) * t * controlPoint
                             + Mathf.Pow(t, 2) * endPoint;

            transform.position = position;

            if (linearT >= 1f)
            {
                GameManager.instance.playerStats.AddExperience(1);

                if (targetUI != null)
                {
                    UIShake shaker = targetUI.GetComponent<UIShake>();
                    if (shaker != null)
                    {
                        shaker.TriggerShake(0.2f);
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}


