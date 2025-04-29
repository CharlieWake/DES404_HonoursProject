using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cinemachine;

public abstract class Interactables : MonoBehaviour
{
    private Vector3Int interactablesPosition;
    private GameObject interactableSprite;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBrain brain;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            interactablesPosition = GameManager.instance.FloorTilemap.WorldToCell(transform.position);
            GameManager.instance.GridManager.SetTileAsOccupied(interactablesPosition, true);
        }

        if (TurnManager.instance != null)
        {
            virtualCamera = TurnManager.instance.VirtualCamera;
        }

        brain = Camera.main.GetComponent<CinemachineBrain>();

        interactableSprite = transform.GetChild(0).gameObject;
    }

    public virtual IEnumerator Interact()
    {
        Debug.Log("Interactable does the thing");
        yield break;
    }

    protected IEnumerator OpenDoor(GameObject targetDoor)
    {               
        if (virtualCamera != null)
        {
            virtualCamera.Follow = targetDoor.transform;
            yield return WaitForCameraBlend();
        }

        OpenDoor doorScript = targetDoor.GetComponent<OpenDoor>();
        if (doorScript != null)
        {
            yield return doorScript.PlayAnimationAndWait("OpenDoor1", "OpenDoor2");
        }

        if (virtualCamera != null)
        {
            virtualCamera.Follow = GameManager.instance.PlayerController.transform;
            yield return WaitForCameraBlend();
        }
    }

    private IEnumerator WaitForCameraBlend(float bufferTime = 1.25f)
    {
        if (brain == null) yield break;

        while (brain.IsBlending)
        {
            yield return null;
        }

        yield return new WaitForSeconds(bufferTime);
    }

    protected void HideInteractable()
    {
        if (interactableSprite != null)
        {
            interactableSprite.SetActive(false);
        }
    }

}
