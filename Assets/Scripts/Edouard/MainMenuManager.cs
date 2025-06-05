using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject menuImage;
    public Button firstSelectedButton;

    private void Start()
    {
        Time.timeScale = 0;
        menuCanvas.SetActive(true);
        menuImage.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown("joystick button 7"))
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