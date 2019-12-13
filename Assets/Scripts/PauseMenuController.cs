using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    EventSystem eventSystem;
    GameObject[] pauseObjects;
    LevelOverController levelOverController;

    void Start()
    {
        eventSystem = EventSystem.current;
        levelOverController = GameObject.FindWithTag("LevelOverMenu").GetComponent<LevelOverController>();
        Time.timeScale = 1;
		pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
		HidePaused();
    }

    void Update()
    {
        if (levelOverController.isLevelOver)
            return;

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
            PauseControl();
        }
    }

    public void PauseControl()
    {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
            ShowPaused();
        } 
        else if (Time.timeScale == 0) {
            Time.timeScale = 1;
            HidePaused();
        }
    }

    public void Restart(){
        int levelInd = SceneManager.GetActiveScene().buildIndex;

        if (levelInd == 1) 
            PlayerPrefs.SetInt("IsOneRunFromBeginToEnd", 1);
        else 
            PlayerPrefs.SetInt("IsOneRunFromBeginToEnd", 0);

        PlayerPrefs.SetInt("CurrentTotalScore", 0);
        PlayerPrefs.SetInt("Lives", 0);

        SceneManager.LoadScene(levelInd);
	}

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

	public void HidePaused(){
        Cursor.visible = false;
		foreach(GameObject g in pauseObjects){
			g.SetActive(false);
		}
	}

	public void ShowPaused(){
        Cursor.visible = true;
		foreach(GameObject g in pauseObjects){
            if (g.name == "PlayButton") {
                eventSystem.SetSelectedGameObject(g, new BaseEventData(eventSystem));
            }
			g.SetActive(true);
		}
	}

}
