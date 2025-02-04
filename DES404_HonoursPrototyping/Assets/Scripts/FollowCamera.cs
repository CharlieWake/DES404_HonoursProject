using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] GameObject playerCharacter;

    void LateUpdate()
    {
        transform.position = playerCharacter.transform.position + new Vector3(0, 0, -10);
    }
}
