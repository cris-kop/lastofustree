using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveEaterScript : MonoBehaviour
{
    GameplayLoop player;

    private bool alive = true;
    private float timeDied;

    public float respawnTimeSec;

    public AudioSource[] killSounds;
    private AudioSource randomKillSound;

    public AudioSource[] chewySounds;
    private AudioSource randomChewySound;

    // User clicked on the enemy
    void OnMouseDown()
    {
        Debug.Log("OnMouseDown for leave-eater!");
        if (alive && !player.seasonPassed && !player.playerDied)
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
        if (!alive && !player.seasonPassed && !player.playerDied)
        {
            if (Time.time > timeDied + respawnTimeSec)
            {
                Respawn();
            }
        }
        
        if (player.seasonPassed && alive)
        {
            SetDeadState();
        }
        if(player.playerDied && alive)
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

        Debug.Log("Setdeadstate called on leaf eater!");
        player.ProcessAbovegroundEnemyClick();
        timeDied = Time.time;
        alive = false;
        player.numberOfAliveThreatsAboveGround--;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Sets all properties when a leave eater respawns
    void Respawn()
    {
        alive = true;
        player.numberOfAliveThreatsAboveGround++;
        GetComponent<SpriteRenderer>().enabled = true;
        randomChewySound = chewySounds[GetRandomNumberChewySound()];
        randomChewySound.Play();

    }
}

