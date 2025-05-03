using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : Interactables
{
    [SerializeField] private GameObject targetDoor;

    public override IEnumerator Interact()
    {
        FindTargetDoor();

        AudioManager.instance.PlaySFXByName("Audio/SFX/Key Pickup");
        HideInteractable();

        yield return new WaitForSeconds(0.5f);

        yield return OpenDoor(targetDoor);

        Destroy(gameObject);
    }

    private void FindTargetDoor()
    {
        targetDoor = GameObject.FindGameObjectWithTag("DoorInteractable");

        if (targetDoor == null)
        {
            Debug.Log("Could not find targetDoor");
        }
    }
}
