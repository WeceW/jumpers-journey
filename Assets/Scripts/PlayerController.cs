using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Jump
{
    public float force;
    public int limit;
    public int jumpCount; // TODO: Make this private (but accessible from PlayerController)
}

[System.Serializable]
public class PlayerColors
{
    public Color normalColor;
    public Color powerColor;
    public Color doubleJumpColor;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Jump jump;
    public PlayerColors colors;

    int lives;
    int score;
    List<string> collectedItems;

    public bool onAir;
    public bool idle;

    private Vector3 startingPosition;
    private float movement;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private StatsBar statsBar;
    private LevelOverController levelOverMenu;
    private Transform sfx;
    private int currentLevel;

    void Start()
    {
        Cursor.visible = false;

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        statsBar = GameObject.FindWithTag("StatsBar").GetComponent<StatsBar>();
        levelOverMenu = GameObject.FindWithTag("LevelOverMenu").GetComponent<LevelOverController>();
        sfx = this.transform.Find("SFX");
        startingPosition = transform.position;
        onAir = true;
        idle = false;
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        lives = PlayerPrefs.GetInt("Lives", 0);
        score = 0;

        collectedItems = new List<string>();
    }

    void Update()
    {
        AudioSource moveSfx = sfx.transform.Find("Move").GetComponent<AudioSource>();

        if (Time.timeScale != 0) {

            if (moveSfx != null) {
                if (!idle && !onAir) {
                    moveSfx.mute = false;
                } else {
                    moveSfx.mute = true;
                }
            }

            var input = Input.GetAxis("Horizontal");
            movement = input * speed;

            anim.SetBool("isMoving", input < 0.1 && input > -0.1 ? false : true);
            spriteRenderer.flipX = SetFlipX(input, spriteRenderer.flipX);

            CheckIfInIdleMode();
        
            if (Input.GetKeyDown(KeyCode.Space)) {
                TryToJump();
            }
        }
        else {
            if (moveSfx != null) moveSfx.mute = true;
        }
    }

    void FixedUpdate()
    {
        rigidBody.velocity = new Vector3(movement, rigidBody.velocity.y, 0);
        if (rigidBody.velocity.y < -0.8) {
            onAir = true;
            if (jump.jumpCount == 0)
                jump.jumpCount = 1; 
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    { 
        if (col.tag == "JumpPower") {
            jump.force = jump.force + 0.5f;
            spriteRenderer.color = colors.powerColor;
            HandleCollection(col);
        }
        else if (col.tag == "2xJump") {
            jump.limit = 2;
            spriteRenderer.color = colors.doubleJumpColor;
            HandleCollection(col);
        }
        else if (col.tag == "Valuable") {
            HandleCollection(col);
        }
        else if (col.tag == "Life") {
            lives++;
            PlayerPrefs.SetInt("Lives", lives);
            statsBar.UpdateLives(lives);
            HandleCollection(col);
        }
        else if (col.tag == "Liquid") {
            Die();
        }
        else if (col.tag == "Finish") {
            HandleLevelPassed();
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Vector3 contactPoint = coll.contacts[0].point;
        Vector3 center = GetComponent<CircleCollider2D>().bounds.center;
        bool bottomContact = (contactPoint.y) < center.y-0.40;

        if (bottomContact) {
            if (onAir)
                PlaySoundEffect("Land");

            onAir = false;
            jump.jumpCount = 0;
            MuteSoundEffect("Jump");
        }

        if (coll.gameObject.tag == "Enemy") {
            if (bottomContact && coll.gameObject.name == "EnemyTank") {
                PlaySoundEffect("DestroyEnemy");
                AddScore(coll.gameObject.GetComponent<ItemProps>().points);
                Destroy(coll.gameObject);
            } 
            else {
                Die();
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        //onAir = true;
    }

    private void TryToJump()
    {
        if (!onAir)
            Jump(jump.force);
        else if (jump.jumpCount > 0 && jump.jumpCount <= jump.limit)
            Jump(jump.force-1); 
        else if (rigidBody.velocity.y == 0) 
            Jump(jump.force);

        jump.jumpCount = (jump.jumpCount == jump.limit) ? -1 : jump.jumpCount;
    }

    private void Jump(float force)
    {
        PlaySoundEffect("Jump");
        rigidBody.AddForce(new Vector3(0, 100*force, 0), ForceMode2D.Force);
        onAir = true;
        idle = false;
        jump.jumpCount++;
    }


    private bool SetFlipX(float input, bool curPos)
    {
        if (input < 0) return true;
        if (input > 0) return false;
        return curPos;
    }

    private void CheckIfInIdleMode()
    {
        if (rigidBody.velocity.x == 0 && rigidBody.velocity.y == 0) {
            // If the player's already idling before coming here, 
            // it's clear that player can't be on the air, right?
            onAir = idle ? false : onAir;
            jump.jumpCount = 0;
            idle = true;
        }
        else {
            idle = false;
        }
    }
    
    private void AddScore(int points)
    {
        score += points;
        statsBar.UpdateScore(score);
    }

    private void AddItem(string tag)
    {
        collectedItems.Add(tag);
        statsBar.UpdateCollectedItems(collectedItems);
    }

    private void HandleCollection(Collider2D item)
    {
        PlaySoundEffect("Collect"+item.tag);
        AddScore( item.GetComponent<ItemProps>().points );
        AddItem( item.tag );
        Destroy(item.gameObject);
    }

    private AudioSource PlaySoundEffect(string effect)
    {
        try
        {
            AudioSource audioSource = sfx.transform.Find(effect).GetComponent<AudioSource>();
            if (audioSource != null) {
                audioSource.Play();
                return audioSource;
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log("SFX for '" + effect + "' not found: " + e);
        }
        return null;
    }

    private void MuteSoundEffect(string effect)
    {
        try
        {
            AudioSource audioSource = sfx.transform.Find(effect).GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.Stop();
        }
        catch (NullReferenceException e)
        {
            Debug.Log("SFX for '" + effect + "' not found: " + e);
        }
    }

    private void Die()
    {
        PlaySoundEffect("Die");
        lives--;
        if (lives >= 0) {
            PlayerPrefs.SetInt("Lives", lives);
            statsBar.UpdateLives(lives);
            score = 0;
            AddScore(0);
            transform.position = startingPosition;
        } 
        else {
            levelOverMenu.ShowMenu(false, 0, 0, 0);
        }
    }

    private void HandleLevelPassed()
    {
        float passingTime = statsBar.GetTime();
        int timeBonus = statsBar.GetTimeBonus();
        score += timeBonus;
        PlaySoundEffect("Finish");
        PlayerPrefs.SetInt("Lives", lives);
        statsBar.Hide();
        levelOverMenu.ShowMenu(true, score, timeBonus, passingTime);
    }

}
