using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject leftPlatforms;   // References to platforms that should appear/disappear
    [SerializeField] GameObject rightPlatforms; // or slide in/out when the game starts/ends
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
    public float spawnRate = 2.5f;
    public float commission;
	
    private float score;
    private int maxLives = 3;
    private float[] xPosition = { -13, 13 };
    private float[] yPosition = { 4.3f, 8.6f };


    public void StartGame()
	{
        isGameActive = true;
        livesLeft = maxLives;
		score = 0;
        commission = 0.01f;
        UpdateScore(0);
        UpdateLife(0);
		
        titleScreen.gameObject.SetActive(false);

        ShowGameElements(true);
        StartCoroutine(SpawnObjects());
	}
	
    public void ShowGameElements(bool isVisible)
    {
        // Deactivating platforms should be probably replaced with moving them to visible part - future feature
        leftPlatforms.gameObject.SetActive(isVisible);
		rightPlatforms.gameObject.SetActive(isVisible);
		player.gameObject.SetActive(isVisible);
        scoreText.gameObject.SetActive(isVisible);
        lifeText.gameObject.SetActive(isVisible);
        commissionText.gameObject.SetActive(isVisible);
        musicToggle.gameObject.SetActive(!isVisible);
    }

    public void GameOver()
	{
        playerAudio.PlayOneShot(soundFXs[3], 0.7f);
		isGameActive = false;
		ShowGameElements(false);
		
		gameOverText.text = "Game Over! \n You've scored " + score + " BTC. Great job!";
		gameOverText.gameObject.SetActive(true);
		restartButton.gameObject.SetActive(true);
		
	}

    public void UpdateScore(float scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score + " BTC";
        commissionText.text = "Commission: " + commission + " BTC";
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

    public void RestartGame()
    {
        PlayButtonSound(); // I had to put it here because restart button reloads the scene too quickly
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        return new Vector3(xPosition[Random.Range(0, xPosition.Length)], yPosition[Random.Range(0, yPosition.Length)]);
    }

}
