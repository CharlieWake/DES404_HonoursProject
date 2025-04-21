using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionDots : MonoBehaviour
{
    [Header("References")]
    private GameObject actionDotPrefab;
    private Transform dotContainer;

    private List<Image> actionDots = new List<Image>();
    
    
    void Awake()
    {
        actionDotPrefab = Resources.Load<GameObject>("ActionDot/ActionDot");
        dotContainer = gameObject.GetComponent<Transform>();
    }

    public void SetActionDotCount(int totalActions)
    {
        foreach (Transform child in dotContainer)
        {
            Destroy(child.gameObject);
        }

        actionDots.Clear();

        for (int i = 0; i < totalActions; i++)
        {
            GameObject newDot = Instantiate(actionDotPrefab, dotContainer);
            Image dotImage = newDot.GetComponent<Image>();
            actionDots.Add(dotImage);
        }
    }

    public void ResetActionDots()
    {
        foreach (Image dot in actionDots)
        {
            dot.color = Color.white;
        }
    }

    public void UseAction()
    {
        for (int i = 0; i < actionDots.Count; i++)
        {
            if (actionDots[i].color != Color.gray)
            {
                actionDots[i].color = Color.gray;
                break;
            }
        }
    }
}
