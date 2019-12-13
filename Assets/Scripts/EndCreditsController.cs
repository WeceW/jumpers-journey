using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndCreditsController : MonoBehaviour
{
    public float creditsDurationInSeconds;
    float time;

    void Start()
    {
        Cursor.visible = false;

        PlayerPrefs.SetInt("AllLevelsPassed", 1);
        time = 0f;

        SetPossibleTotalHighscores();

        string creds = "This game was created as a part of the course \nFundamentals of Game Development \nin LUT University\n(Fall 2019)";
        creds += "\n\n";
        creds += "\n\n";
        creds += "\n\n";
        creds += "Creator\nToni Weckroth";
        creds += "\n\n";
        creds += "Graphic Design\nToni Weckroth";
        creds += "\n\n";
        creds += "Level Design\nToni Weckroth";
        creds += "\n\n";
        creds += "Programming\nToni Weckroth";
        creds += "\n\n";
        creds += "\n\n";
        creds += "\n\n";
        creds += "MUSIC AND SOUND EFFECTS";
        creds += "\n\n";
        creds += "Main Menu:\n'The Night Sky'\nby Olan\n(Unity Asset Store)";
        creds += "\n\n";
        creds += "Level 1, Bootcamp Champ:\n'Settling Up'\nfrom Sci-Fi Music Loops Pack \nby LightBuffer Studio\n(Unity Asset Store)";
        creds += "\n\n";
        creds += "Level 2, Ice Ice Maybe:\n'Frozen Field'\nfrom Sci-Fi Music Loops Pack \nby LightBuffer Studio\n(Unity Asset Store)";
        creds += "\n\n";
        creds += "Level 3, Long Night:\n'Light Latt'\nfrom Sci-Fi Music Loops Pack \nby LightBuffer Studio\n(Unity Asset Store)";
        creds += "\n\n";
        creds += "Level 4, Fancy Fiesta:\n'Jolly Bot'\nfrom Sci-Fi Music Loops Pack \nby LightBuffer Studio\n(Unity Asset Store)";
        creds += "\n\n";
        creds += "Sound Effects:\n'FREE Casual Game SFX Pack'\nby Dustyroom\n(Unity Asset Store)";
        creds += "\n\n";
        creds += "\n\n";
        creds += "\n\n";
        creds += "Thanks for the inspiration, information and instructions:";
        creds += "\n\n";
        creds += "UNITY COMMUNITY";
        creds += "\n\n";
        creds += "STACK OVERFLOW\n(Especially stackoverflow.com/questions/42207122/create-a-basic-2d-platformer-game-in-unity)";
        creds += "\n\n";
        creds += "UNITY BLOG:\n2D Tilemap Asset Workflow: From Image to Level\n(blogs.unity3d.com/2018/01/25/2d-tilemap-asset-workflow-from-image-to-level/)";
        creds += "\n\n";
        creds += "INSTRUCTABLES:\nMake a 2D Video Game With Unity\n(www.instructables.com/id/Make-A-2D-Infinite-Runner-with-Unity/)";
        creds += "\n\n";
        creds += "PIXELNEST:\nCreating a 2D game with Unity\n(pixelnest.io/tutorials/2d-game-unity/)";
        creds += "\n\n";
        creds += "SITEPOINT:\nAdding Pause, Main Menu and Game over Screens in Unity\n(www.sitepoint.com/adding-pause-main-menu-and-game-over-screens-in-unity/)";
        creds += "\n\n";
        creds += "STUDYTONIGHT:\nUnity 3D: Adding Sound Effects to Game\n(www.studytonight.com/game-development-in-2D/audio-in-unity)";
        creds += "\n\n";
        creds += "Font Awesome (fontawesome.com)";
        creds += "\n\n";

        Text textComponent = GetComponentInChildren<Text>();
        textComponent.text = creds;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadMainMenu();

        time += Time.deltaTime;
        if (time >= creditsDurationInSeconds) 
            LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void SetPossibleTotalHighscores()
    {
        Transform parent = transform.parent;
        Text newHighscoreComponent = parent.Find("NewHighscore").GetComponent<Text>();
        Text newRecordComponent = parent.Find("NewRecord").GetComponent<Text>();
        newHighscoreComponent.text = "";
        newRecordComponent.text = "";

        if (PlayerPrefs.GetInt("IsOneRunFromBeginToEnd", 0) == 1) {
            int playerTotalScore = PlayerPrefs.GetInt("CurrentTotalScore", 0);
            float playersTotalRunTime = PlayerPrefs.GetFloat("CurrentRunTime", 0);

            if (playerTotalScore > PlayerPrefs.GetInt("TotalHighscore", 0)) {
                PlayerPrefs.SetInt("TotalHighscore", playerTotalScore);
                newHighscoreComponent.text = "You made a new awesome highscore! Your total score was " + playerTotalScore + ".";
            }

            if (playersTotalRunTime < PlayerPrefs.GetFloat("TotalTimeRecord", 0) || PlayerPrefs.GetFloat("TotalTimeRecord", 0) == 0) {
                PlayerPrefs.SetFloat("TotalTimeRecord", playersTotalRunTime);
                newRecordComponent.text = 
                    "You made a new time record! Total time was " + 
                    TimeSpan.FromSeconds(playersTotalRunTime).ToString("mm':'ss'.'fff") + 
                    ".";
            }

        }
    }
}
