using System;
using UnityEngine;

public class Heal : MonoBehaviour
{
   private TestPlayer player;
   private bool isInArea = false;
   
   public int healAmount;
   
   private void Update()
   {
      if (isInArea)
      {
         HealPlayer();
      }
   }
   
   private void OnTriggerStay2D(Collider2D other)
      {
         player = other.gameObject.GetComponent<TestPlayer>();
         isInArea = true;
      }

   public void HealPlayer()
   {
      if (Input.GetKeyDown(KeyCode.E))
      {
         //Debug.Log(isInArea);
         player.health += healAmount;
         //Destroy(gameObject);
      }
      
   }
}
