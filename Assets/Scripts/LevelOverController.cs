using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelOverController : MonoBehaviour
{
    EventSystem eventSystem;
    public bool isLevelOver;
    private GameObject[] menuObjects;
    private Text greeting;
    private Text levelScore;
    private Text levelHighscore;
    private Text levelRunTime;
    private Text levelBestRunTime;

    void Start()
    {
        eventSystem = EventSystem.current;
		menuObjects = GameObject.FindGameObjectsWithTag("ShowOnLevelOver");

        greeting = GameObject.Find("Text").GetComponent<Text>();
        levelScore = GameObject.Find("LevelScore").GetComponent<Text>();
        levelHighscore = GameObject.Find("LevelHighscore").GetComponent<Text>();
        levelRunTime = GameObject.Find("LevelTime").GetComponent<Text>();
        levelBestRunTime = GameObject.Find("LevelBestTime").GetComponent<Text>();

		HideMenu();
        isLevelOver = false;
    }

    public void TryAgain()
    {
        int levelInd = SceneManager.GetActiveScene().buildIndex;

        if (levelInd == 1) 
            PlayerPrefs.SetInt("IsOneRunFromBeginToEnd", 1);
        else 
            PlayerPrefs.SetInt("IsOneRunFromBeginToEnd", 0);

        PlayerPrefs.SetInt("CurrentTotalScore", 0);
        PlayerPrefs.SetInt("Lives", 0);

        Time.timeScale = 1;
        SceneManager.LoadScene(levelInd);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void HideMenu(){
		foreach(GameObject g in menuObjects){
			g.SetActive(false);
		}
	}

	public void ShowMenu(bool passed, int score, int timeBonus, float passingTime) {
        Cursor.visible = true;

        Time.timeScale = 0;
        isLevelOver = true;
        greeting.text = passed 
            ? "Level passed!" 
            : "Sorry, you failed.";

        if (passed) {
            PlayerPrefs.SetInt("Level" + SceneManager.GetActiveScene().buildIndex + "Passed", 1);
            SetHighscoreStats(score, timeBonus, passingTime);
        }

		foreach(GameObject g in menuObjects){
            if (!passed && g.name == "ContinueButton") {
                DisableButton(g);
            } else if (passed && g.name == "ContinueButton") {
                eventSystem.SetSelectedGameObject(g, new BaseEventData(eventSystem));
            } else if (!passed && g.name == "TryAgainButton") {
                eventSystem.SetSelectedGameObject(g, new BaseEventData(eventSystem));
            }
			g.SetActive(true);
		}
	}

    public void DisableButton(GameObject g)
    {
        g.GetComponent<Button>().interactable = false;
        g.GetComponentInChildren<Text>().GetComponent<Text>().color = new Color(0.4f,0.4f,0.4f,1);
    }

    private void SetHighscoreStats(int score, int timeBonus, float passingTime)
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int highscore = PlayerPrefs.GetInt("Level"+currentLevel+"Highscore", 0);
        float bestTime = PlayerPrefs.GetFloat("Level"+currentLevel+"TimeRecord", 0);

        if (score > highscore) {
            PlayerPrefs.SetInt("Level"+currentLevel+"Highscore", score);
            levelScore.text = "NEW HIGHSCORE " + score.ToString() + "! (Time Bonus " + timeBonus + ")";
            if (highscore > 0)
                levelHighscore.text = "Old Highscore " + highscore.ToString();
        } else {
            levelScore.text = "SCORE " + score.ToString() + "! (Time Bonus " + timeBonus + ")";
            levelHighscore.text = "Highscore " + highscore.ToString();
        }

        if (passingTime < bestTime || bestTime == 0) {
            PlayerPrefs.SetFloat("Level"+currentLevel+"TimeRecord", passingTime);
            levelRunTime.text = "NEW RECORD " + TimeSpan.FromSeconds(passingTime).ToString("mm':'ss'.'fff") + "!";
            if (bestTime > 0) 
                levelBestRunTime.text = "Old Record " + TimeSpan.FromSeconds(bestTime).ToString("mm':'ss'.'fff");
        } else {
            levelRunTime.text = "TIME " + TimeSpan.FromSeconds(passingTime).ToString("mm':'ss'.'fff");
            levelBestRunTime.text = "Record " + TimeSpan.FromSeconds(bestTime).ToString("mm':'ss'.'fff");
        }

        PlayerPrefs.SetInt("CurrentTotalScore", 
            PlayerPrefs.GetInt("CurrentTotalScore", 0) + score
        );

        PlayerPrefs.SetFloat("CurrentRunTime", 
            PlayerPrefs.GetFloat("CurrentRunTime", 0) + passingTime
        );
    }
}
