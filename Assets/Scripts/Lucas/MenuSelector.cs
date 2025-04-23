using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private GameObject firstButton;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null); // reset
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}