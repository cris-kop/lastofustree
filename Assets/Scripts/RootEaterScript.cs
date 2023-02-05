using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEaterScript : MonoBehaviour
{
    private GameplayLoop player;

    private bool enemyAlive = true;
    private float timeDied;

    public float[] respawnTimeSec;

    public AudioSource[] killSounds;

    // User clicked on the enemy
    void OnMouseDown()
    {
        if (player.GameRunning() && enemyAlive)
        {
            player.ProcessUndergroundEnemyClick();
            timeDied = Time.time;
            enemyAlive = false;
            player.numberOfAliveThreatsUnderGround--;
            GetComponent<SpriteRenderer>().enabled = false;
            killSounds[GetRandomNumberKillSound()].Play();
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
        if(player.GameRunning() && !enemyAlive)
        {
            if (Time.time > timeDied + respawnTimeSec[player.currentSeasonId])
            {
                enemyAlive = true;
                player.numberOfAliveThreatsUnderGround++;
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        if (player.betweenSeasons)
        {
            gameObject.SetActive(false);
        }
    }

    // get random number
    int GetRandomNumberKillSound()
    {
        return Random.Range(0, killSounds.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
