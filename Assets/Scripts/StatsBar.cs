using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    private Text score;
    private Text timer;
    private float currentTime;
    public float timeBonusSeconds;

    private GameObject jumpPowerIcon;
    private Text jumpPowerText;
    private GameObject doubleJumpIcon;
    private int currentTotalScore;
    private Text livesText;
    
    void Start()
    {
        currentTotalScore = PlayerPrefs.GetInt("CurrentTotalScore", 0);
        score = this.transform.Find("Score").GetComponent<Text>();
        UpdateScore(0);

        timer = this.transform.Find("Timer").GetComponent<Text>();
        currentTime = 0;

        jumpPowerText = this.transform.Find("MiddlePanel/Left/JumpPowerCount").GetComponent<Text>();
        jumpPowerIcon = GameObject.Find("JumpPowerIcon");
        doubleJumpIcon = GameObject.Find("2xJumpIcon");
        livesText = this.transform.Find("MiddlePanel/Right/LivesIcon/Lives").GetComponent<Text>();

        jumpPowerText.text = "";
        jumpPowerIcon.SetActive(false);
        doubleJumpIcon.SetActive(false);
        livesText.text = PlayerPrefs.GetInt("Lives", 0).ToString();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (timeBonusSeconds > 0)
            timeBonusSeconds -= Time.deltaTime;

        timer.text 
            = TimeSpan.FromSeconds(currentTime).ToString("mm':'ss'.'fff") 
            + "\nTime Bonus " + GetTimeBonus().ToString("0");
    }

    public float GetTime()
    {
        return currentTime;
    }

    public int GetTimeBonus()
    {
        return (int)(timeBonusSeconds*10);
    }

    public void UpdateScore(int points)
    {
        score.text = "SCORE " + points.ToString() + "\nTotal Score " + (currentTotalScore+points);
    }

    public void UpdateLives(int livesLeft)
    {
        livesText.text = livesLeft.ToString();
    }

    public void UpdateCollectedItems(List<string> items)
    {
        if (items.Count > 0) {
            int jumpPowerCount = 0;
            foreach (string item in items) {
                if (item == "JumpPower") {
                    UpdateJumpPowerInfo(++jumpPowerCount);
                }
                if (item == "2xJump") {
                    UpdateDoubleJumpInfo();
                }
            }
        }
    }

    void UpdateJumpPowerInfo(int count)
    {
        if (count > 0) 
            jumpPowerIcon.SetActive(true);
        if (count > 1) 
            jumpPowerText.text = "x " + count;
    }

    void UpdateDoubleJumpInfo()
    {
        doubleJumpIcon.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

}
