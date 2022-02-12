using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<ParticleSystem> explosionParticle;
    public int collisionCounter;

    // Variables for player's position (4 combinations total) and rotation (2 ways)
    public float xPos = 2.8f;
    public float yPos = 0.96f;
    private GameManager gameManager;
    private Quaternion leftRotation = new Quaternion(-0.2f, -1.0f, 0.0f, 0.0f);
    private Quaternion rightRotation = new Quaternion(0.0f, 0.0f, 0.2f, 1.0f);
    // Interval between game difficulty increasings
    private int interval = 5;
    
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Left upper position
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = new Vector3(-xPos, yPos);
            transform.rotation = leftRotation;
        }
        // Right upper position
        else if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = new Vector3(xPos, yPos);
            transform.rotation = rightRotation;
        }
        // Left lower position
        else if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position = new Vector3(-xPos, -yPos - 2.5f);
            transform.rotation = leftRotation;
        }
        // Right lower position
        else if (Input.GetKeyDown(KeyCode.L))
        {
            transform.position = new Vector3(xPos, -yPos - 2.5f);
            transform.rotation = rightRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            gameManager.playerAudio.PlayOneShot(gameManager.soundFXs[0], 0.7f);
            Destroy(collision.gameObject);
            Instantiate(explosionParticle[0], collision.transform.position, explosionParticle[0].transform.rotation);
            gameManager.UpdateScore(gameManager.commission / 100);
 
        }
        else if (collision.gameObject.CompareTag("Powerup"))
        {
            gameManager.playerAudio.PlayOneShot(gameManager.soundFXs[1], 1f);
            Destroy(collision.gameObject);
            Instantiate(explosionParticle[1], collision.transform.position, explosionParticle[0].transform.rotation);
            gameManager.UpdateLife(1);  
        }

        // Game becomes harder as it progresses
        collisionCounter++;

        // Interval between spawns decreases being multiplied by 0.8 every double <interval> collisions
        if (collisionCounter % (interval * 2) == 0 && gameManager.spawnRate > 0.5f)
        {
            gameManager.spawnRate *= 0.8f;
        }
        // Commission also grows every <interval> collisions
        if (collisionCounter % interval == 0)
        {
            gameManager.commission += 1;
        }
    }
}
