using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MNGR_LevelManager : MonoBehaviour
{
    //Handles Waypoints through a level, so a death doesn't mean restarting from the beginning of the level each time.
    [Header("Death/Respawn")]
    public CTRL_Waypoint[] spawnPoints = new CTRL_Waypoint[0];
    public int curSpawnPointIndex = 0;

    [Header("Level Name Strings")]
    public string thisLevelName;
    public string nextLevelName;
    public string previousLevelName;
    public string mainMenuName;

    [Header("Level Indeces")]
    public int thisLevelIndex;
    public int nextLevelIndex;
    public int previousLevelIndex;
    public int mainMenuIndex;

    [Header("UI Stuff")]
    public GameObject overlayUIPanel;
    public GameObject loseUIPanel;
    public GameObject winUIPanel;
    public GameObject pauseUIPanel;

    [Header("Boolean Logic")]
    public bool isPaused = false;

    [Header("Level Timer")]
    public float levelTimer;
    private float levelTimerReset;

    [Header("Text Objects")]
    public TextMeshProUGUI timerText;
    private string timerTextString;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuIndex = 0;
        thisLevelIndex = SceneManager.GetActiveScene().buildIndex;
        thisLevelName = SceneManager.GetActiveScene().name;
        unpauseTime();
        levelTimerReset = levelTimer;
        timerTextString = levelTimer.ToString("F2");
    }

    private void Update()
    {
        getInput();

        evaluateLevelTimer();
    }

    private void evaluateLevelTimer()
    {
        levelTimer -= Time.deltaTime;
        timerTextString = levelTimer.ToString("F2");
        timerText.text = timerTextString;
        if (levelTimer <= 0.0f)
        {
            levelTimer = 0.0f;
            loseLevel();
        }
    }

    private void getInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                unpauseTime();
                pauseUIPanel.SetActive(false);
                overlayUIPanel.SetActive(true);
            }
            else
            {

                overlayUIPanel.SetActive(false);
                pauseUIPanel.SetActive(true);
                pauseTime();
            }
        }
    }

    public void pauseTime()
    {
        Time.timeScale = 0.0f;
        Time.fixedDeltaTime = 0.0f;
        isPaused = true;
    }

    public void unpauseTime()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        isPaused = false;
    }

    public void loadNextLevel()
    {
        nextLevelIndex = thisLevelIndex + 1;
        unpauseTime();
        SceneManager.LoadScene(nextLevelIndex);
    }

    public void loadPreviousLevel()
    {
        nextLevelIndex = thisLevelIndex - 1;
        unpauseTime();
        SceneManager.LoadScene(previousLevelIndex);
    }

    public void reloadThisLevel()
    {
        unpauseTime();
        SceneManager.LoadScene(thisLevelIndex);
    }

    public void loadMainMenu()
    {
        unpauseTime();
        SceneManager.LoadScene(mainMenuIndex);
    }

    public void winLevel()
    {
        overlayUIPanel.SetActive(false);
        winUIPanel.SetActive(true);
        pauseTime();

    }

    public void loseLevel()
    {
        overlayUIPanel.SetActive(false);
        loseUIPanel.SetActive(true);
        pauseTime();

    }

}
