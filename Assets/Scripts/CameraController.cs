using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeedKeyboard;
    public float moveSpeedMouseScroll;
    public float rotationAngle;

    private float camMaxY = 16.0f;
    private float camMinY = -6.5f;
    private float posYatZeroAngle;

    // Start is called before the first frame update
    void Start()
    {
        posYatZeroAngle = camMaxY - ((camMaxY - camMinY) * 0.5f);
    }

    // Update is called once per frame
    void Update()
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
            Debug.Log("Cam rot X: " + camRot.x);
        }
    }
}
