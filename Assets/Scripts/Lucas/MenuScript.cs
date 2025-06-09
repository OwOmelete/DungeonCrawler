using UnityEngine;
using UnityEngine.EventSystems;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private FadeChangeScene fadeChangeSceneRef;
    [SerializeField] private int sceneStartIndex;
    [SerializeField] private int sceneCreditIndex;
    [SerializeField] private GameObject firstButton;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    public void StartGame()
    {
        fadeChangeSceneRef.ChangeScene(sceneStartIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void StartCredits()
    {
        fadeChangeSceneRef.ChangeScene(sceneCreditIndex);
    }
}
