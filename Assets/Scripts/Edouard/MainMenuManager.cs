using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManuManager : MonoBehaviour
{
    
    public GameObject menuCanvas;
    public GameObject menuImage;
    private void Start()
    {
        Time.timeScale = 0;
        menuCanvas.SetActive(true);
        menuImage.SetActive(true);
    }

    public void StartGame()
    {
        menuCanvas.SetActive(false);
        menuImage.SetActive(false);
       Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
