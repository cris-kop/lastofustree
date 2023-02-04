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
    public AudioSource[] chewySounds;

    private int playingChewySound;

    // User clicked on the enemy
    void OnMouseDown()
    {
        if (alive && !player.seasonPassed && !player.playerDied)
        {
            player.ProcessAbovegroundEnemyClick();
            timeDied = Time.time;
            alive = false;
            player.numberOfAliveThreatsAboveGround--;
            GetComponent<MeshRenderer>().enabled = false;
            killSounds[GetRandomNumberKillSound()].Play();
            chewySounds[playingChewySound].Stop();
        }
    }
        
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<GameplayLoop>();
    }

    // update at fixed timestep
    void FixedUpdate()
    {
        if (!alive && !player.seasonPassed && !player.playerDied)
        {
            if (Time.time > timeDied + respawnTimeSec)
            {
                alive = true;
                player.numberOfAliveThreatsAboveGround++;
                GetComponent<MeshRenderer>().enabled = true;
                playingChewySound = GetRandomNumberChewySound();
                chewySounds[playingChewySound].Play();
            }
        }
        if (player.seasonPassed)
        {
            gameObject.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {

    }
}

