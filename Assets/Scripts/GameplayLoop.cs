using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayLoop : MonoBehaviour
{
    public Text seasonProgressText;
    public Text winText;
    public Text sunHealthText;
    public Text waterHealthText;

    private float seasonProgress = 0.0f;
    public bool seasonPassed;

    public float aboveGroundPercentageIncrease;
    public float underGroundPercentageIncrease;
    public int numberOfAliveThreatsAboveGround;
    public int numberOfAliveThreatsUnderGround;

    private float sunHealth;
    private float waterHealth;
    private bool playerDied;

    public float seasonSpeed;
    public float sunTreatSpeed;
    public float waterTreatSpeed;

    // Start is called before the first frame update
    void Start()
    {
        seasonProgressText.text = "Season progress: 0%";
        winText.text = "";
        seasonPassed = false;

        seasonProgress = 0.0f;
        sunHealth = 100.0f;
        waterHealth = 100.0f;
        playerDied = false;

        sunHealthText.text = "Sun : 100%";
        waterHealthText.text = "Water: 100%";
    }

    // Fixed update is at fixed times
    void FixedUpdate()
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
            }

            // Sun and water health decreasing over time (unless player takes action)
            if (sunHealth > 0.0f)
            {
                sunHealth -= sunTreatSpeed * numberOfAliveThreatsAboveGround;
            }
            else
            {
                playerDied = true;
            }

            if (waterHealth > 0.0f)
            {
                waterHealth -= waterTreatSpeed * numberOfAliveThreatsUnderGround;
            }
            else
            {
                playerDied = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // servived the season?
        if(!seasonPassed)
        { 
            seasonProgressText.text = "Season progress: " + ((int)seasonProgress) + "%";
        }
        
        if(seasonPassed) 
        {
            winText.text = "Season survived!\nWater left: " + (int)waterHealth + "\nSun left: " + (int)sunHealth;
        }

        // player died?
        if(!playerDied)
        {
            sunHealthText.text = "Sun: " + ((int)sunHealth) + "%";
            waterHealthText.text = "Water: " + ((int)waterHealth) + "%";

            
        }

        if(playerDied)
        {
            winText.text = "You killed the tree, shame on you.";
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
}
