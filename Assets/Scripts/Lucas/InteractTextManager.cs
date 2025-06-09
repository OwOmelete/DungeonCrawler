using System;
using UnityEngine;

public class InteractTextManager : MonoBehaviour
{
    public static InteractTextManager INSTANCE;
    
    public bool firstPlant = true;
    public bool firstRadar = true;
    public bool firstSpikeDamage = true;

    private void Start()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
