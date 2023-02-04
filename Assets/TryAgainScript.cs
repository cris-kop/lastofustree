using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class TryAgainScript : MonoBehaviour
{
    public void RetryGame()
    {
        SceneManager.LoadScene("Splashscreen");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
