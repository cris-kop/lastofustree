using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenScript : MonoBehaviour
{
    public AudioSource buttonClickSound;
    
    public void StartMainScene()
    {
        buttonClickSound.Play();
        SceneManager.LoadScene("Mainscene");
    }

    public void QuitGame()
    {
        buttonClickSound.Play();
        Application.Quit();
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
