using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject platforms;
    [SerializeField] GameObject player;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI lifeText;
    [SerializeField] TextMeshProUGUI commissionText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] GameObject titleScreen;
    [SerializeField] Button startButton;
    [SerializeField] Button restartButton;
    [SerializeField] Toggle musicToggle;
    [SerializeField] int livesLeft;
    
    public List<GameObject> targets;
    public List<AudioClip> soundFXs;
    public AudioSource playerAudio;

    public bool isGameActive;
    public float spawnRate;
    public float commission;
	
    private PlayerController playerController;
    private float score;
    private int maxLives = 3;
    private int tutorialStep = 0;
    private float[] xCoinPosition = { -13, 13 };
    private float[] yCoinPosition = { 4.3f, 8.6f };


    public void StartGame(bool firstLaunch)
    {
        
        isGameActive = true;
        livesLeft = maxLives;
        score = 0;
        commission = 1;
        spawnRate = 3f;

        UpdateScore(0);
        UpdateLife(0);

        ShowGameElements(true);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.collisionCounter = 0;
        titleScreen.gameObject.SetActive(false);
        ShowRestartScreenElements(false);
        if (firstLaunch)
        {
            ShowTutorial(0);
        }
        else StartCoroutine(SpawnObjects());
    }
	
    public void ShowGameElements(bool isVisible)
    {
        platforms.gameObject.SetActive(isVisible);
		player.gameObject.SetActive(isVisible);
        scoreText.gameObject.SetActive(isVisible);
        lifeText.gameObject.SetActive(isVisible);
        commissionText.gameObject.SetActive(isVisible);
        musicToggle.gameObject.SetActive(!isVisible);
    }

    public void ShowRestartScreenElements(bool isVisible)
    {
        gameOverText.text = "Game Over! \n You've scored " + score + " BTC. Great job!";
        gameOverText.gameObject.SetActive(isVisible);
        restartButton.gameObject.SetActive(isVisible);
    }

    public void GameOver()
    {
        playerAudio.PlayOneShot(soundFXs[3], 0.7f);
        isGameActive = false;
        ShowGameElements(false);
        ShowRestartScreenElements(true);
    }

    public void UpdateScore(float scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score + " BTC";
        commissionText.text = "Commission: " + (commission / 100) + " BTC";
    }

    public void UpdateLife(int livesToAdd)
    {
        if (livesLeft == maxLives && livesToAdd > 0)
            lifeText.text = "Lives: " + maxLives;
        else
        {
            livesLeft = livesLeft + livesToAdd;
            lifeText.text = "Lives: " + livesLeft;
        }

        if (livesLeft == 0) GameOver();
    }

    void ShowTutorial(int step)
    {
        switch (step)
        {
            case 0:
                Instantiate(targets[1], new Vector3(xCoinPosition[0], yCoinPosition[1]), targets[1].gameObject.transform.rotation);

                StartCoroutine(DestroyObstacleIfPlayerReachesPosition(new Vector3(-playerController.xPos, playerController.yPos),
                                                                        GameObject.Find("Tutorial/Left Upper Q Obstacle"),
                                                                        GameObject.Find("Tutorial/Canvas/Pointer Q")));
                break;
            case 1:
                Instantiate(targets[1], new Vector3(xCoinPosition[0], yCoinPosition[0]), targets[1].gameObject.transform.rotation);
                StartCoroutine(DestroyObstacleIfPlayerReachesPosition(new Vector3(-playerController.xPos, -playerController.yPos - 2.5f),
                                                                        GameObject.Find("Left Lower A Obstacle"),
                                                                        GameObject.Find("Tutorial/Canvas/Pointer A")));
                break;
            case 2:
                Instantiate(targets[1], new Vector3(xCoinPosition[1], yCoinPosition[1]), targets[1].gameObject.transform.rotation);
                StartCoroutine(DestroyObstacleIfPlayerReachesPosition(new Vector3(playerController.xPos, playerController.yPos),
                                                                        GameObject.Find("Right Upper P Obstacle"),
                                                                        GameObject.Find("Tutorial/Canvas/Pointer P")));
            break;
            case 3:
                Instantiate(targets[1], new Vector3(xCoinPosition[1], yCoinPosition[0]), targets[1].gameObject.transform.rotation);
                StartCoroutine(DestroyObstacleIfPlayerReachesPosition(new Vector3(playerController.xPos, -playerController.yPos - 2.5f),
                                                                        GameObject.Find("Right Lower L Obstacle"),
                                                                        GameObject.Find("Tutorial/Canvas/Pointer L")));
            break;
            default:
                StartCoroutine(SpawnObjects());
            break;
        }
    }

    IEnumerator DestroyObstacleIfPlayerReachesPosition(Vector3 requiredPosition, GameObject currentObstacle, GameObject currentPointer)
    {
        currentObstacle.gameObject.SetActive(true);
        currentPointer.gameObject.SetActive(true);
        while(GameObject.Find("Player").transform.position != requiredPosition)
        {
            yield return new WaitForSeconds(0.1f); 
        }
        Destroy(currentObstacle);
        currentPointer.gameObject.SetActive(false);
        
        ShowTutorial(++tutorialStep);
        yield break;
    }
    IEnumerator SpawnObjects()
    {
        while(isGameActive)
        {
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index], GetRandomPosition(), targets[index].gameObject.transform.rotation);
            yield return new WaitForSeconds(spawnRate);
        }
    }
    

    public void PlayButtonSound()
    {
        playerAudio.PlayOneShot(soundFXs[2], 0.5f);
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(xCoinPosition[Random.Range(0, xCoinPosition.Length)], yCoinPosition[Random.Range(0, yCoinPosition.Length)]);
    }

}
