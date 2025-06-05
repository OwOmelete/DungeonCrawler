using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject menuImage;
    public Button firstSelectedButton;

    public Input input; 

    private void Start()
    {
        //input.Enable(); 

        Time.timeScale = 0;
        menuCanvas.SetActive(true);
        menuImage.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
    }

    private void Update()
    {
        if (input.Equals(enabled)/*UI.GoBack.triggered*/) 
        {
            GoToMainMenu();
        }
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

    public void GoToMainMenu()
    {
        menuCanvas.SetActive(true);
        menuImage.SetActive(true);
        Time.timeScale = 0;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
    }
}