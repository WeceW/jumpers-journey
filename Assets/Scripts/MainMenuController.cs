using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    EventSystem eventSystem;
    Button creditsButton;

    void Start()
    {
        Cursor.visible = true;

        eventSystem = EventSystem.current;
        GameObject startButton = GameObject.FindWithTag("StartButton");
        eventSystem.SetSelectedGameObject(startButton, new BaseEventData(eventSystem));

        creditsButton = this.transform.Find("Highscores/Credits").GetComponent<Button>();
        if (PlayerPrefs.GetInt("AllLevelsPassed", 0) == 1)
            creditsButton.interactable = true;

        PlayerPrefs.SetInt("IsOneRunFromBeginToEnd", 0);
        PlayerPrefs.SetInt("CurrentTotalScore", 0);
        PlayerPrefs.SetFloat("CurrentRunTime", 0);
        PlayerPrefs.SetInt("Lives", 0);

        SetHighscores();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartTheGame()
    {
        LoadLevel("Level1");
    }

    public void LoadLevel(string level)
    {
        if (level == "Level1")
            PlayerPrefs.SetInt("IsOneRunFromBeginToEnd", 1);

        SceneManager.LoadScene(level);
    }

    void SetHighscores()
    {
        SetLevelHighscore("Level1", 1);
        SetLevelHighscore("Level2", 2);
        SetLevelHighscore("Level3", 3);
        SetLevelHighscore("Level4", 4);
        SetLevelHighscore("Total", 0);
    }

    void SetLevelHighscore(string level, int levelInd)
    {
        Transform hs = this.transform.Find("Highscores");
        Transform levelName = hs.transform.Find(level+"Name");
        Text levelScore = hs.transform.Find(level+"Score").GetComponent<Text>();
        Text levelTime = hs.transform.Find(level+"Time").GetComponent<Text>();
        float levelRecord = PlayerPrefs.GetFloat(level+"TimeRecord", 0);

        if (levelInd == 1 || (level != "Total" && PlayerPrefs.GetInt("Level"+(levelInd-1)+"Passed", 0) == 1)) {
            // Unlock the link to the levels that are previously reached!
            levelName.GetComponent<Button>().interactable = true;
        }

        levelScore.text = PlayerPrefs.GetInt(level+"Highscore", 0).ToString();

        levelTime.text 
            = levelRecord > 0
            ? TimeSpan.FromSeconds(levelRecord).ToString("mm':'ss'.'fff")
            : "-";
    }

    public void ResetHighscores()
    {
        PlayerPrefs.SetInt("Level1Highscore", 0);
        PlayerPrefs.SetInt("Level2Highscore", 0);
        PlayerPrefs.SetInt("Level3Highscore", 0);
        PlayerPrefs.SetInt("Level4Highscore", 0);
        PlayerPrefs.SetInt("TotalHighscore", 0);
        PlayerPrefs.SetFloat("Level1TimeRecord", 0);
        PlayerPrefs.SetFloat("Level2TimeRecord", 0);
        PlayerPrefs.SetFloat("Level3TimeRecord", 0);
        PlayerPrefs.SetFloat("Level4TimeRecord", 0);
        PlayerPrefs.SetFloat("TotalTimeRecord", 0);
        SetHighscores();
    }

    public void DeleteAllPlayerPrefs(bool deleteAll)
    {
        if (deleteAll)
            PlayerPrefs.DeleteAll();
    }

}
