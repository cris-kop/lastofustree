using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeedKeyboard;
    public float moveSpeedMouseScroll;
    public float rotationAngle;

    public float camMaxY;
    public float camMinY;
    private float posYatZeroAngle;

    public float introMoveSpeed;
    // UGLY BUT WORKING
    private bool introPart1done = false;
    private bool introPart2done = false;
    private Vector3 camStartPos;

    GameplayLoop player;

    public GameObject countdown;

    // Start is called before the first frame update
    void Start()
    {
        posYatZeroAngle = camMaxY - ((camMaxY - camMinY) * 0.5f);
        camStartPos = GetComponent<Transform>().transform.position;

        player = GameObject.Find("Player").GetComponent<GameplayLoop>();
    }

    // Update is called once per frame
    void Update()
    {       
        if (player.cameraIntroDone)
        {
            if (GetComponent<Transform>().transform.position.y <= camMaxY && GetComponent<Transform>().transform.position.y >= camMinY)
            {
                float moveVertical = Input.GetAxis("Vertical") * moveSpeedKeyboard;

                moveVertical += Input.mouseScrollDelta.y * moveSpeedMouseScroll;

                Vector3 movement = new Vector3(0.0f, moveVertical, 0.0f);
                movement = movement * Time.deltaTime;

                Vector3 newPos = GetComponent<Transform>().transform.position += movement;

                newPos.y = Mathf.Clamp(newPos.y, camMinY + 0.1f, camMaxY - 0.1f);

                GetComponent<Transform>().transform.position = newPos;


                Vector3 camRot = GetComponent<Transform>().transform.eulerAngles;
                camRot.x = ((newPos.y - posYatZeroAngle) / (camMaxY - camMinY)) * (rotationAngle * 2.0f);
                GetComponent<Transform>().transform.eulerAngles = camRot;
            }
        }
    }

    // fixed update frequency - abused for camera intro sequence
    void FixedUpdate()
    {
        if (!player.cameraIntroDone)
        {
            if (!introPart1done)
            {
                if (GetComponent<Transform>().transform.position.y < camMaxY - 1.0f)
                {
                    Vector3 movement = new Vector3(0.0f, introMoveSpeed, 0.0f);
                    Vector3 newPos = GetComponent<Transform>().transform.position += movement;
                    GetComponent<Transform>().transform.position = newPos;

                    // countdown
                    Vector3 countdownPos = countdown.GetComponent<Transform>().position;
                    countdownPos.y = newPos.y + 2.5f;
                    countdown.GetComponent<Transform>().position = countdownPos;
                }
                else
                {
                    introPart1done = true;
                }
            }
            if (introPart1done && !introPart2done)
            {
                if (GetComponent<Transform>().transform.position.y > camMinY + 1.0f)
                {
                    Vector3 movement = new Vector3(0.0f, -introMoveSpeed, 0.0f);
                    Vector3 newPos = GetComponent<Transform>().transform.position += movement;
                    GetComponent<Transform>().transform.position = newPos;

                    // countdown
                    Vector3 countdownPos = countdown.GetComponent<Transform>().position;
                    countdownPos.y = newPos.y + 2.5f;
                    countdown.GetComponent<Transform>().position = countdownPos;
                }
                else
                {
                    introPart2done = true;
                }
            }
            if( introPart2done)
            {
                if (GetComponent<Transform>().transform.position.y <= camStartPos.y)
                {
                    Vector3 movement = new Vector3(0.0f, introMoveSpeed, 0.0f);
                    Vector3 newPos = GetComponent<Transform>().transform.position += movement;

                    GetComponent<Transform>().transform.position = newPos;

                    // countdown
                    Vector3 countdownPos = countdown.GetComponent<Transform>().position;
                    countdownPos.y = newPos.y + 2.5f;
                    countdown.GetComponent<Transform>().position = countdownPos;
                }
                else
                {
                    player.cameraIntroDone = true;
                    //countdown.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }
}
