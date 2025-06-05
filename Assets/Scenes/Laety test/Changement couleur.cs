using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteCouleur : MonoBehaviour
{
    public Color couleur = Color.white;

    void OnValidate()
    {
        // Change la couleur dans l'inspecteur en temps réel
        GetComponent<SpriteRenderer>().color = couleur;
    }

    void Start()
    {
        // Sécurité au cas où OnValidate ne se déclenche pas
        GetComponent<SpriteRenderer>().color = couleur;
    }
}
