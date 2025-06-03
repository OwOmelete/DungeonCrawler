using Unity.VisualScripting;
using UnityEngine;

public class ActivateHeat : MonoBehaviour
{

    public Material heat;
    public float active;

    private void Start()
    {
        active = heat.GetFloat("_Active"); 
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            active += 1;
            heat.GetFloat("_Active") += 1; 
        } 

    }  
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            active -= 1; 
        } 

    }
}
