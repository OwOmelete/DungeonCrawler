using UnityEngine;
using UnityEngine.SceneManagement; // Permet de gerer le changement de scenes

public class MockUp_Scene_Changer : MonoBehaviour
{
   //Fonction qui est appelé quand le bouton est cliqué
   public void ChangementScene(int index)
   {
      SceneManager.LoadScene(index);
   }
   
   /*public void VersScene2b()
   {
      SceneManager.LoadScene(6);
   }*/

   
}
