using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowImage : MonoBehaviour
{
    // Displays Image component of Game Object this script is attached to after 4 seconds
    void Start()
    {
        StartCoroutine(DisplayImageWithDelay());
    }
    IEnumerator DisplayImageWithDelay()
    {
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<Image>().enabled = true;
    }
}
