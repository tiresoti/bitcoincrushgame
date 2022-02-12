// Destroys particle in 2 seconds
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2);
    }
}
