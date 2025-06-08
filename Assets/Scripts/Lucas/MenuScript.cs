using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private FadeChangeScene fadeChangeSceneRef;
    [SerializeField] private int sceneStartIndex;
    [SerializeField] private int sceneCreditIndex;
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
