using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BalloonControll : MonoBehaviour
{

    public static BalloonControll instance;                         // Allow other classes to send signals to this class
    public GameObject PausePanel;                                   // Define the pause panel
    public GameObject GameOverText;                                 // Define the game over text
    public GameObject GameEndText;                                  // Define the game end text
    public GameObject ResumeButton;                                 // Define the resume button
    public Text scoreText;                                          // Define the score text
    public Text highScoreText;                                      // Define the high score text
    public bool GameOver = false;                                   // State that the game is not over at default
    public bool GameEnd = false;                                    // State that the game has not ended at default
    public bool GamePaused = false;                                 // State that the game is not paused at default
    public bool FreeMode = false;                                   // State the default mode (story- or freemode)

    int score = 0;                                                  // Define the starting score
    int popHighScore = 0;                                           // Define the initial high score

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Set time to move forward at the beginning
        Time.timeScale = 1;
    }

    // Load high score at start
    void Start()
    {
        popHighScore = PlayerPrefs.GetInt("popHighScore");

        highScoreText.text = "High score " + popHighScore.ToString();
    }

    // When called, resume the game time
    public void Resume()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {

        // If game has ended or is over, freeze the game
        if (GameEnd == true || GameOver == true)
        {
            Time.timeScale = 0;
        }

        // What to do when the player hits escape on pc or the back button on their phone
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePaused = true;
            Time.timeScale = 0;

            PausePanel.SetActive(true);
            ResumeButton.SetActive(true);
        }

        // Update the highscore if the current score is higher than the previous highscore
        if (score > popHighScore)
        {
            popHighScore = score;
            PlayerPrefs.SetInt("popHighScore", popHighScore);
            PlayerPrefs.Save();


            highScoreText.text = "High score " + popHighScore.ToString();
        }

        // If score is below -5, put game into game over
        if (score < -5)
        {
            PlayerDied();
        }

        // If game is not in freemode, and the score reaches above 25, end game (storymode)
        if (FreeMode == false)
        {
            if (score > 25)
            {
                PlayerEnd();
            }
        }
    }

    // Add one point to the score when balloon successfully, if game is over return to 0
    public void PlayerScored()
    {
        if (GameOver)
        {
            return;
        }
        score++;
        scoreText.text = "Score " + score.ToString();
    }

    // What to do if a balloon gets too big and pops
    public void BalloonPoppedLate()
    {
        if (GameOver)
        {
            return;
        }
        score--;
        scoreText.text = "Score " + score.ToString();
    }

    // Set game to be over if player dies
    public void PlayerDied()
    {
        PausePanel.SetActive(true);
        GameOverText.SetActive(true);
        GameOver = true;
    }

    // End game when player reaches the end of the storymode
    public void PlayerEnd()
    {
        PausePanel.SetActive(true);
        GameEndText.SetActive(true);
        GameEnd = true;
    }
}
