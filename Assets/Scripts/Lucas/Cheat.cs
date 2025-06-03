using System;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private OxygenManager oxygenManagerRef;
    [SerializeField] private LightManager lightManagerRef;
    [SerializeField] private GameObject canvas;
    private bool isActive;
    private float x;
    private float y;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
            canvas.SetActive(isActive);
        }
    }

    #region Tp

    public void setX(float value)
    {
        x = value;
    }
    public void setY(float value)
    {
        y = value;
    }
    public void Tp()
    {
        playerTransform.position = new Vector3(x,y,0f);
    }

    #endregion
    
    public void FullRessources()
    {
           oxygenManagerRef.AddOxygen(oxygenManagerRef.maxOxygen);
           lightManagerRef.AddLight(lightManagerRef.maxLight);
           playerTransform.GetComponent<Player>().player.hp = 4;
    }
}
