using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script destructs anything that could have fallen beyond the ground
public class DestructionDebris : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
       Destroy(collision.gameObject);
    }
}
