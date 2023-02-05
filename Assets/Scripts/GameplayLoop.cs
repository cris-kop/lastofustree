using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayLoop : MonoBehaviour
{
    //public Camera primaryCamera;
    
    public Text winText;
    public SeasonClockController seasonClockController;

    public ProgressController sunProgressController;
    public ProgressController waterProgressController;

    public Button restartButton;

    private float seasonProgress = 0.0f;
    public int currentSeasonId;               // 0 spring, 1 summer, 2 autumn, 3 winter
    public bool betweenSeasons;

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

    // season related controls
    public int healthIncreaseAtSeasonSwitch;

    // Intro mode
    public bool cameraIntroDone;


    // Start is called before the first frame update
    void Start()
    {
        StartNewGame();
    }

    // Fixed update is at fixed times
    void FixedUpdate()
    {
        if(GameRunning())
        {
            // Season progression
            if (seasonProgress < 100.0f)
            {
                seasonProgress += seasonSpeed;
            }
            else
            {
                StopCurrentSeason();
            }
            if (!betweenSeasons)
            {
                ProcessThreats();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameRunning())
        {
            UpdateSeasonClock();

            sunProgressController.progress = (int)sunHealth;
            waterProgressController.progress = (int)waterHealth;
        }
    }

    // process UNDERGROUND enemy clicks
    public void ProcessUndergroundEnemyClick()
    {
        if(GameRunning())
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
        if (GameRunning())
        {
            if (sunHealth <= 100.0f)
            {
                sunHealth += aboveGroundPercentageIncrease;    // TODO: make variable
            }
            Mathf.Clamp(sunHealth, sunHealth, 100.0f);
        }
    }

    // run this when the player dies
    void KillPlayer()
    {
        playerDied = true;
        springBackgroundMusic.Stop();
        dieSound.Play();
        currentSeasonId = -1;

        winText.text = "You didn't manage to save to three, shame on you.";
        restartButton.gameObject.SetActive(true);
    }

    void StartNewGame()
    {
        restartButton.gameObject.SetActive(false);

        winText.text = "";
        currentSeasonId = 0;

        seasonProgress = 0.0f;
        sunHealth = 100.0f;
        waterHealth = 100.0f;
        playerDied = false;
        betweenSeasons = false;

        sunProgressController.progress = (int)sunHealth;
        waterProgressController.progress = (int)waterHealth;

        // start background music for Season 1: spring
        springBackgroundMusic.Play();
    }

    public bool GameRunning()
    {
        return cameraIntroDone && !betweenSeasons && !playerDied && currentSeasonId != -1;
    }

    void StopCurrentSeason()
    {
        winSound.Play();

        winText.text = "Season survived!\nWater left: " + (int)waterHealth + "\nSun left: " + (int)sunHealth;
        restartButton.gameObject.SetActive(true);

        betweenSeasons = true;

        StartNextSeason();
    }

    void StartNextSeason()
    {
        currentSeasonId += 1;
        springBackgroundMusic.Stop();
        betweenSeasons = false;
    }

    void ProcessThreats()
    {
        // Sun and water health decreasing over time (unless player takes action)
        if (sunHealth > 0.0f)
        {
            sunHealth -= sunTreatSpeed * numberOfAliveThreatsAboveGround;
        }
        else
        {
            KillPlayer();
        }

        if (waterHealth > 0.0f)
        {
            waterHealth -= waterTreatSpeed * numberOfAliveThreatsUnderGround;
        }
        else
        {
            KillPlayer();
        }
    }

    void UpdateSeasonClock()
    {
        seasonClockController.seasonProgress = seasonProgress;
    }
}
