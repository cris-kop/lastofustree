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
    private bool seasonPassed;
    
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
        if (!playerDied)
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
                sunHealth -= sunTreatSpeed;
            }
            else
            {
                playerDied = true;
            }

            if (waterHealth > 0.0f)
            {
                waterHealth -= waterTreatSpeed;
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
            seasonProgressText.text = "Season progress: " + ((int)seasonProgress).ToString() + "%";
        }
        
        if(seasonPassed) 
        {
            winText.text = "Season survived!";
        }

        // player died?
        if(!playerDied)
        {
            sunHealthText.text = "Sun: " + ((int)sunHealth) + "%";
            waterHealthText.text = "Water: " + ((int)waterHealth) + "%";
        }

        if(playerDied)
        {
            winText.text = "You killed the tree, shame on you!";
        }
    }
}
