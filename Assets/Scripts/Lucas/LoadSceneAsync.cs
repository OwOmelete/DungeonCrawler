using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadSceneAsync : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private int index;
    AsyncOperation asyncLoad;

    void Start()
    {
        asyncLoad = SceneManager.LoadSceneAsync(index);
        asyncLoad.allowSceneActivation = false;
        videoPlayer.loopPointReached += EndVideo;
    }

    void EndVideo(VideoPlayer vp)
    {
        asyncLoad.allowSceneActivation = true;
    }
}
