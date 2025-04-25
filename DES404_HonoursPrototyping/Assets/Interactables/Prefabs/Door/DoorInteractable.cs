using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactables
{
    [Header("FloatingLockedText")]
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform doorCanvas;
    [SerializeField] GameWin gameWin;

    public bool isOpen = false;

    public override IEnumerator Interact()
    {
        if (isOpen == false)
        {
            FloatingDamageText("Locked!", transform.position);
            yield return null;
        }
        else
        {
            GameManager.instance.PlayerStats.isDead = true;
            gameWin.CallGameWinScreen();
            Debug.Log("You beat the level!");
        }
    }

    void FloatingDamageText(string lockedText, Vector3 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition + Vector3.up * 0.5f);
        GameObject textObject = Instantiate(floatingTextPrefab, worldPosition, Quaternion.identity, doorCanvas);
        textObject.GetComponent<FloatingDamageText>().setText(lockedText);
    }
}
