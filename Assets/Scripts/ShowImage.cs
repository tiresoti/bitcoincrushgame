// Displays Image component of Game Object this script is attached to
// with 1.5 seconds delay
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowImage : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DisplayImageWithDelay());
    }
    IEnumerator DisplayImageWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Image>().enabled = true;
    }
}
