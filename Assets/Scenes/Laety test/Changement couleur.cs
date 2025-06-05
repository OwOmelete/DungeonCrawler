using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteCouleur : MonoBehaviour
{
    public Color couleur = Color.white;

    void OnValidate()
    {
        // Change la couleur dans l'inspecteur en temps r�el
        GetComponent<SpriteRenderer>().color = couleur;
    }

    void Start()
    {
        // S�curit� au cas o� OnValidate ne se d�clenche pas
        GetComponent<SpriteRenderer>().color = couleur;
    }
}
