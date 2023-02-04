using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveEaterScript : MonoBehaviour
{
    GameplayLoop player;

    private bool alive = true;
    private float timeDied;

    public float respawnTimeSec;

    // User clicked on the enemy
    void OnMouseDown()
    {
        if (alive && !player.seasonPassed)
        {
            player.ProcessAbovegroundEnemyClick();
            timeDied = Time.time;
            alive = false;
            player.numberOfAliveThreatsAboveGround--;
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
        if (!alive)
        {
            if (Time.time > timeDied + respawnTimeSec)
            {
                alive = true;
                player.numberOfAliveThreatsAboveGround++;
                GetComponent<MeshRenderer>().enabled = true;
            }
        }
        if (player.seasonPassed)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

