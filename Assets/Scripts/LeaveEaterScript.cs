using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveEaterScript : MonoBehaviour
{
    private GameplayLoop player;

    private bool enemyAlive = true;
    private float timeDied;

    public float[] respawnTimeSec;

    public AudioSource[] killSounds;
    private AudioSource randomKillSound;

    public AudioSource[] chewySounds;
    private AudioSource randomChewySound;

    // User clicked on the enemy
    void OnMouseDown()
    {
        if(player.GameRunning() && enemyAlive)
        { 
            SetDeadState();
        }
    }
        
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<GameplayLoop>();

        randomChewySound = chewySounds[GetRandomNumberChewySound()];
        randomChewySound.Play();

        randomKillSound = killSounds[GetRandomNumberKillSound()];
    }

    // update at fixed timestep
    void FixedUpdate()
    {
        if(player.GameRunning() && !enemyAlive)
        { 
            if (Time.time > timeDied + respawnTimeSec[player.currentSeasonId])
            {
                Respawn();
            }
        }

        if (player.betweenSeasons && enemyAlive)
        {
            SetDeadState();
        }
        if (player.playerDied && enemyAlive)
        {
            SetDeadState();
        }
    }


    // get random number
    int GetRandomNumberKillSound()
    {
        return Random.Range(0, killSounds.Length);
    }

    // get random number
    int GetRandomNumberChewySound()
    {
        return Random.Range(0, chewySounds.Length);
    }

    // Sets all properties when a leave eater dies
    void SetDeadState()
    {
        randomChewySound.Stop();
        randomKillSound.Play();
        randomKillSound = killSounds[GetRandomNumberKillSound()];

        player.ProcessAbovegroundEnemyClick();
        timeDied = Time.time;
        enemyAlive = false;
        player.numberOfAliveThreatsAboveGround--;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Sets all properties when a leave eater respawns
    void Respawn()
    {
        enemyAlive = true;
        player.numberOfAliveThreatsAboveGround++;
        GetComponent<SpriteRenderer>().enabled = true;
        randomChewySound = chewySounds[GetRandomNumberChewySound()];
        randomChewySound.Play();

    }
}

