using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float gravityModifier = 1f;

    public List<ParticleSystem> explosionParticle;
    private Rigidbody playerRb;
    private GameManager gameManager;

    // Variables for player's position (4 combinations total) and rotation (2 ways)
    float xPos = 2.8f;
    float yPos = 0.96f;
    Quaternion leftRotation = new Quaternion(-0.2f, -1.0f, 0.0f, 0.0f);
    Quaternion rightRotation = new Quaternion(0.0f, 0.0f, 0.2f, 1.0f);

    int collisionCounter = 0; // This variable counts how many collisions happened
    [SerializeField] int interval = 5; // Interval between game difficulty increasings
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        Physics.gravity *= gravityModifier;
    }

    void Update()
    {
        MovePlayer();
    }

    // This method changes player's position and rotation based on keyboard input
    void MovePlayer()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Left upper position
        {
            transform.position = new Vector3(-xPos, yPos);
            transform.rotation = leftRotation;
        }
        else if (Input.GetKeyDown(KeyCode.P))// Right upper position
        {
            transform.position = new Vector3(xPos, yPos);
            transform.rotation = rightRotation;
        }
        else if (Input.GetKeyDown(KeyCode.A)) // Left lower position
        {
            transform.position = new Vector3(-xPos, -yPos - 2.5f);
            transform.rotation = leftRotation;
        }
        else if (Input.GetKeyDown(KeyCode.L)) // Right lower position
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
 
        } else if (collision.gameObject.CompareTag("Powerup"))
        {
            gameManager.playerAudio.PlayOneShot(gameManager.soundFXs[1], 1f);
            Destroy(collision.gameObject);
            Instantiate(explosionParticle[1], collision.transform.position, explosionParticle[0].transform.rotation);
            gameManager.UpdateLife(1);  
        }

        // Game becomes harder as it progresses
        // Each 50 collisions gravity intensifies and bitcoins/cookies roll faster
        collisionCounter++;
        if (collisionCounter == 50)
        {
            collisionCounter = 0;
            gravityModifier += 0.07f;
            Physics.gravity *= gravityModifier;
        }
        
        // Interval between spawns decreases being multiplied by 0.8 every <interval> collisions
        if (collisionCounter % (interval * 2) == 0 && gameManager.spawnRate > 0.5f)
        {
            gameManager.spawnRate *= 0.8f;
        }

        // Commission also grows but it's not restricted
        if (collisionCounter % interval == 0)
        {
            gameManager.commission += 1;
        }
        
    }
}
