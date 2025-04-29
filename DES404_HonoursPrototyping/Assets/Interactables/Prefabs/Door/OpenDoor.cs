using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OpenDoor : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator door1Animator;
    [SerializeField] protected Animator door2Animator;
    [SerializeField] private float doorDelay;

    public IEnumerator PlayAnimationAndWait(string doorAnimationName1, string doorAnimationName2)
    {
        if (door1Animator != null)
            door1Animator.Play(doorAnimationName1);

        if (door2Animator != null)
            door2Animator.Play(doorAnimationName2);

        DoorInteractable doorInteractable = GetComponent<DoorInteractable>();
        if (doorInteractable != null)
        {
            doorInteractable.isOpen = true;
        }

        yield return new WaitForSeconds(doorDelay);
    }

}
