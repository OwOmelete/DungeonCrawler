using UnityEngine;

public class UiBar : MonoBehaviour
{
    [SerializeField] private GameObject[] arrayBar = new GameObject[5];
    private int actualSprite = 0;
    
    public void ChangeSprite(int value)
    {
        arrayBar[actualSprite].SetActive(false);
        arrayBar[value].SetActive(true);
        actualSprite = value;
    }
}
