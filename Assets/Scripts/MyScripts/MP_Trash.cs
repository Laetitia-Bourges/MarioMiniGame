using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Trash : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        Destroy(_other.gameObject);
    }
}
