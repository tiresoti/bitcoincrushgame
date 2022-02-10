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
    private float[] xCoinPosition = { -13, 13 };
    private float[] yCoinPosition = { 4.3f, 8.6f };


    public void StartGame(bool firstLaunch)
    {
        if (!firstLaunch)
        {
            Debug.Log("Game is restarted");
        };
        isGameActive = true;
        livesLeft = maxLives;
        score = 0;
        commission = 1;
        spawnRate = 2.5f;

        UpdateScore(0);
        UpdateLife(0);

        ShowGameElements(true);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.collisionCounter = 0;
        titleScreen.gameObject.SetActive(false);
        ShowRestartScreenElements(false);
        StartCoroutine(SpawnObjects());
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
