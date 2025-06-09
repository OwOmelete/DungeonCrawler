using System;
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

    private void Update()
    {
        float joysticX = Input.GetAxis("Horizontal");
        float joysticY = Input.GetAxis("Vertical");
        if ((joysticY >= 0.01f || joysticY <= -0.01f  || joysticX >= 0.01f || joysticX <= -0.01f) && !EventSystem.current.currentSelectedGameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
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
