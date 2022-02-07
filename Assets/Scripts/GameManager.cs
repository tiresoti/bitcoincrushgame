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

	// This method can be called by Start button, starts the game
    public void StartGame()
	{
		isGameActive = true;
        livesLeft = maxLives;
		score = 0;
        commission = 0.01f;
        UpdateScore(0);
        UpdateLife(0);
		
        // Hide the title screen
        titleScreen.gameObject.SetActive(false);
        
        // Now we should display game elements and start spawning objects
        ShowGameElements(true);
        StartCoroutine(SpawnObjects());
	}
	
    // Displays or hides game elements (player, platforms, score & life counters)
    public void ShowGameElements(bool isVisible)
    {
        // Deactivating platforms should be probably replaced with moving them to visible part - future feature
        leftPlatforms.gameObject.SetActive(isVisible);
		rightPlatforms.gameObject.SetActive(isVisible);
		player.gameObject.SetActive(isVisible);
        scoreText.gameObject.SetActive(isVisible);
        lifeText.gameObject.SetActive(isVisible);
        commissionText.gameObject.SetActive(isVisible);
        // It also hides music toggle
        musicToggle.gameObject.SetActive(!isVisible);
    }

	// When game is over, game elements get hidden, total score and restart button are shown
    public void GameOver()
	{
        playerAudio.PlayOneShot(soundFXs[3], 0.7f);
		isGameActive = false;
		ShowGameElements(false);
		
		gameOverText.text = "Game Over! \n You've scored " + score + " BTC. Great job!";
		gameOverText.gameObject.SetActive(true);
		restartButton.gameObject.SetActive(true);
		
	}

    // This method updates score count and displays it on the gamescreen
    public void UpdateScore(float scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score + " BTC";
        commissionText.text = "Commission: " + commission + " BTC";
    }

    // This method updates life count and displays it on the gamescreen. End the game if no lives left
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

    // This method restarts the game by reloading the scene
    public void RestartGame()
    {
        PlayButtonSound(); // I had to put it here because restart button reloads the scene too quickly
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // This ienumerator spawns new objects from a list of bitcoins and cookies in a random of 4 possible locations
    IEnumerator SpawnObjects()
    {
        while(isGameActive)
        {
            float randomX;
            float randomY;
            Vector3 randomPosition;
            
            GetRandomPosition(out randomX, out randomY);
            randomPosition = new Vector3(randomX, randomY);

            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index], randomPosition, targets[index].gameObject.transform.rotation);
            yield return new WaitForSeconds(spawnRate);
        }
    }
    
    // This method is called by pressing clicking button, it plays sound of clicking a button
    public void PlayButtonSound()
    {
        playerAudio.PlayOneShot(soundFXs[2], 0.5f);
    }


    // It's harder to make a random range between 2 fixed values than in a decimal range so we need combinations
    // Let Random method decide whether it will be positive or negative
    void GetRandomPosition(out float x, out float y)
    {
        int sign = Random.Range(0, 2);
        if (sign == 0) x = 13f;
        else x = -13f;

        sign = Random.Range(0, 2);
        if (sign == 0)
            y = 4.3f;
        else y = 8.6f;
    }
}
