using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEaterScript : MonoBehaviour
{
    GameplayLoop player;

    private bool alive = true;
    private float timeDied;

    public float respawnTimeSec;

    // User clicked on the enemy
    void OnMouseDown()
    {
        if (alive && !player.seasonPassed && !player.playerDied)
        {
            player.ProcessUndergroundEnemyClick();
            timeDied = Time.time;
            alive = false;
            player.numberOfAliveThreatsUnderGround--;
            GetComponent<MeshRenderer>().enabled = false;
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
                player.numberOfAliveThreatsUnderGround++;
                GetComponent<MeshRenderer>().enabled = true;
            }
        }
        if(player.seasonPassed)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
