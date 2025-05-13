using UnityEngine;

public class Heal : MonoBehaviour
{
   private TestPlayer player;
   private bool isInArea = false;
   
   public KeyCode healKey = KeyCode.E;
   public int healAmount;
   
   private void Update()
   {
      if (isInArea)
      {
         if (Input.GetKeyDown(healKey))
         {
            HealPlayer();
         }
      }
   }
   
   private void OnTriggerStay2D(Collider2D other)
      {
         player = other.gameObject.GetComponent<TestPlayer>();
         isInArea = true;
      }

   public void HealPlayer()
   {
      //player.health += healAmount;
      
   }
}
