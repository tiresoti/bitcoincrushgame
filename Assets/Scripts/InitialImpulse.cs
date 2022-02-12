// This script adds some initial force to bitcoins and cookies
// so they would fall on platforms faster
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialImpulse : MonoBehaviour
{
    private Rigidbody playerRb;
    private float forceMagnitude = 300f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.AddForce(Vector3.down * forceMagnitude, ForceMode.Impulse);
    }
}
