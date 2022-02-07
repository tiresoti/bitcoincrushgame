using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    private GameManager gameManager;

    // Getting a reference to GameManager script to call methods from it
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // This script decreases lives count if a bitcoin falls on the ground and destroys the object that has fallen
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            if(gameManager.isGameActive) gameManager.playerAudio.PlayOneShot(gameManager.soundFXs[4], 0.4f);
            Destroy(collision.gameObject);
            gameManager.UpdateLife(-1);
        }
        
        if (collision.gameObject.CompareTag("Powerup"))
        {
            Destroy(collision.gameObject);
        }
    }
}
