using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayLoop : MonoBehaviour
{
    //public Camera primaryCamera;

    public Image DieImage;
    public Image WinImage;

    //public Text winText;
    public SeasonClockController seasonClockController;

    public ProgressController sunProgressController;
    public ProgressController waterProgressController;

    public Button restartButton;

    private float seasonProgress = 0.0f;
    public int currentSeasonId;               // 0 spring, 1 summer, 2 autumn, 3 winter
    public bool betweenSeasons;

    public SpriteRenderer arrowUp;
    public SpriteRenderer arrowDown;

    public float[] aboveGroundPercentageIncrease;
    public float[] underGroundPercentageIncrease;
        
    public int numberOfAliveThreatsAboveGround;
    public int numberOfAliveThreatsUnderGround;

    private float leafHealth;
    private float rootHealth;
    public bool playerDied;

    public AudioSource winSound;
    public AudioSource dieSound;

    // difficulty controls
    public float[] seasonSpeeds;
    public float[] sunThreatSpeeds;
    public float[] waterThreatSpeeds;

    // season related controls
    public int healthIncreaseAtSeasonSwitch;

    public GameObject groundMesh;
    public GameObject[] backgroundPlanes;

    public Material[] springMaterials;
    public Material[] summerMaterials;
    public Material[] autumnMaterials;
    public Material[] winterMaterials;

    public AudioSource[] backgroundMusicArray;

    // Intro mode
    public bool cameraIntroDone;


    // Start is called before the first frame update
    void Start()
    {
        StartNewGame();
        arrowUp.enabled = false;
        arrowDown.enabled = false;
    }

    // Fixed update is at fixed times
    void FixedUpdate()
    {
        if (GameRunning())
        {
            // Season progression
            if (seasonProgress < 100.0f)
            {
                seasonProgress += seasonSpeeds[currentSeasonId];
            }
            else
            {
                if (!betweenSeasons)
                {
                    StopCurrentSeason();
                }
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

            sunProgressController.progress = (int)leafHealth;
            waterProgressController.progress = (int)rootHealth;

            if (leafHealth < 30)
            {
                Debug.Log("low sun");
                arrowUp.enabled = true;
            }
            else
            {
                Debug.Log("enough sun");
                arrowUp.enabled = false;
            }
            if (rootHealth < 30)
            {
                Debug.Log("low water");
                arrowDown.enabled = true;
            }
            else
            {
                Debug.Log("enough water");
                arrowDown.enabled = false;
            }
        }
    }


    // process UNDERGROUND enemy clicks
    public void ProcessUndergroundEnemyClick()
    {
        if (GameRunning())
        {
            if (rootHealth <= 100.0f)
            {
                rootHealth += underGroundPercentageIncrease[currentSeasonId];
            }
            Mathf.Clamp(rootHealth, rootHealth, 100.0f);
        }
    }

    // process ABOVEGROUND enemy clicks
    public void ProcessAbovegroundEnemyClick()
    {
        if (GameRunning())
        {
            if (leafHealth <= 100.0f)
            {
                leafHealth += aboveGroundPercentageIncrease[currentSeasonId];
            }
            Mathf.Clamp(leafHealth, leafHealth, 100.0f);
        }
    }

    // run this when the player dies
    void KillPlayer()
    {
        playerDied = true;
        dieSound.Play();

        DieImage.enabled = true;

        //winText.text = "You didn't manage to save to three, shame on you.";
        restartButton.gameObject.SetActive(true);

        backgroundMusicArray[currentSeasonId].Stop();
        currentSeasonId = -1;
    }

    void StartNewGame()
    {
        restartButton.gameObject.SetActive(false);

        DieImage.enabled = false;
        WinImage.enabled = false;

        //winText.text = "";
        currentSeasonId = 0;
        ChangeSeasonBackgrounds();

        seasonProgress = 0.0f;
        leafHealth = 100.0f;
        rootHealth = 100.0f;
        playerDied = false;
        betweenSeasons = false;

        sunProgressController.progress = (int)leafHealth;
        waterProgressController.progress = (int)rootHealth;

        backgroundMusicArray[0].Play();
    }

    public bool GameRunning()
    {
        return cameraIntroDone && !betweenSeasons && !playerDied && currentSeasonId != -1;
    }

    void StopCurrentSeason()
    {
        winSound.Play();

        betweenSeasons = true;

        if (currentSeasonId == 3)
        {
            WinGame();
        }
        else
        {
            StartNextSeason();
        }
    }

    void StartNextSeason()
    {
        currentSeasonId += 1;
        seasonProgress = 0;

        ChangeSeasonMusic();
        ChangeSeasonBackgrounds();

        groundMesh.GetComponent<GroundController>().ChangeMaterial(currentSeasonId);

        leafHealth += healthIncreaseAtSeasonSwitch;
        rootHealth += healthIncreaseAtSeasonSwitch;

        seasonClockController.seasonId += 1; // currentSeasonId;

        betweenSeasons = false;
    }

    void ProcessThreats()
    {
        // Sun and water health decreasing over time (unless player takes action)
        if (leafHealth > 0.0f)
        {
            leafHealth -= sunThreatSpeeds[currentSeasonId] * numberOfAliveThreatsAboveGround;
        }
        else
        {
            KillPlayer();
        }

        if (rootHealth > 0.0f)
        {
            rootHealth -= waterThreatSpeeds[currentSeasonId] * numberOfAliveThreatsUnderGround;
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

    void ChangeSeasonMusic()
    {
        backgroundMusicArray[currentSeasonId - 1].Stop();
        backgroundMusicArray[currentSeasonId].Play();
    }

    void ChangeSeasonBackgrounds()
    {
        if (currentSeasonId == 0)
        {
            for (int i = 0; i < 7; ++i)
            {
                backgroundPlanes[i].GetComponent<MeshRenderer>().material = springMaterials[i];
            }
        }
        if (currentSeasonId == 1)
        {
            for (int i = 0; i < 7; ++i)
            {
                backgroundPlanes[i].GetComponent<MeshRenderer>().material = summerMaterials[i];
            }
        }
        if (currentSeasonId == 2)
        {
            for (int i = 0; i < 7; ++i)
            {
                backgroundPlanes[i].GetComponent<MeshRenderer>().material = autumnMaterials[i];
            }
        }
        if (currentSeasonId == 3)
        {
            for (int i = 0; i < 7; ++i)
            {
                backgroundPlanes[i].GetComponent<MeshRenderer>().material = winterMaterials[i];
            }
        }
    }

    void WinGame()
    {
        //winText.text = "YOU ARE VICTORIOUS";
        WinImage.enabled = true;
        restartButton.gameObject.SetActive(true);
    }
}