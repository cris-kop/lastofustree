using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayLoop : MonoBehaviour
{
    public Camera primaryCamera;
    
    public Text winText;
    public SeasonClockController seasonClockController;

    public ProgressController sunProgressController;
    public ProgressController waterProgressController;

    public Button restartButton;

    private float seasonProgress = 0.0f;
    public int seasonId = 0;
    public bool seasonPassed;

    public float aboveGroundPercentageIncrease;
    public float underGroundPercentageIncrease;
    public int numberOfAliveThreatsAboveGround;
    public int numberOfAliveThreatsUnderGround;

    private float sunHealth;
    private float waterHealth;
    public bool playerDied;

    public float seasonSpeed;
    public float sunTreatSpeed;
    public float waterTreatSpeed;

    public AudioSource springBackgroundMusic;
    public AudioSource winSound;
    public AudioSource dieSound;

    // Intro mode
    public bool cameraIntroDone;


    // Start is called before the first frame update
    void Start()
    {
        restartButton.gameObject.SetActive(false);

        winText.text = "";
        seasonPassed = false;

        seasonProgress = 0.0f;
        sunHealth = 100.0f;
        waterHealth = 100.0f;
        playerDied = false;

        sunProgressController.progress = (int)sunHealth;
        waterProgressController.progress = (int)waterHealth;

        // start background music for Season 1: spring
        springBackgroundMusic.Play();
    }

    // Fixed update is at fixed times
    void FixedUpdate()
    {
        if (cameraIntroDone)
        {
            if (!playerDied && !seasonPassed)
            {
                // Season progression
                if (seasonProgress < 100.0f)
                {
                    seasonProgress += seasonSpeed;
                }
                else
                {
                    seasonPassed = true;
                    springBackgroundMusic.Stop();
                    winSound.Play();
                }

                // Sun and water health decreasing over time (unless player takes action)
                if (sunHealth > 0.0f)
                {
                    sunHealth -= sunTreatSpeed * numberOfAliveThreatsAboveGround;
                }
                else
                {
                    PlayerDied();
                }

                if (waterHealth > 0.0f)
                {
                    waterHealth -= waterTreatSpeed * numberOfAliveThreatsUnderGround;
                }
                else
                {
                    PlayerDied();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraIntroDone)
        {
            // servived the season?
            if (!seasonPassed)
            {
                seasonClockController.seasonProgress = seasonProgress;
            }

            if (seasonPassed)
            {
                winText.text = "Season survived!\nWater left: " + (int)waterHealth + "\nSun left: " + (int)sunHealth;
                restartButton.gameObject.SetActive(true);
            }

            // player died?
            if (!playerDied)
            {
                sunProgressController.progress = (int)sunHealth;
                waterProgressController.progress = (int)waterHealth;
            }

            if (playerDied)
            {
                winText.text = "You didn't manage to save to three, shame on you.";
                restartButton.gameObject.SetActive(true);
            }
        }
    }

    // process UNDERGROUND enemy clicks
    public void ProcessUndergroundEnemyClick()
    {
        if (!seasonPassed)
        {
            if (waterHealth <= 100.0f)
            {
                waterHealth += underGroundPercentageIncrease;    // TODO: make variable
            }
            Mathf.Clamp(waterHealth, waterHealth, 100.0f);
        }
    }

    // process ABOVEGROUND enemy clicks
    public void ProcessAbovegroundEnemyClick()
    {
        if (!seasonPassed)
        {
            if (sunHealth <= 100.0f)
            {
                sunHealth += aboveGroundPercentageIncrease;    // TODO: make variable
            }
            Mathf.Clamp(sunHealth, sunHealth, 100.0f);
        }
    }

    // run this when the player dies
    void PlayerDied()
    {
        playerDied = true;
        springBackgroundMusic.Stop();
        dieSound.Play();
    }
}
