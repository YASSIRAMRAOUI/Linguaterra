using UnityEngine;
using System.Collections.Generic;

public class PlayerAnimalCollision : MonoBehaviour
{
    public AnimalPanelManager manager;
    private HashSet<GameObject> collidedAnimals = new HashSet<GameObject>();

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Animal") && !collidedAnimals.Contains(collision.gameObject))
        {
            collidedAnimals.Add(collision.gameObject);

            AnimalData data = collision.gameObject.GetComponent<AnimalData>();
            if (data != null && manager != null)
            {
                int index = System.Array.IndexOf(manager.animals, data);
                if (index >= 0)
                {
                    manager.StartAnimalChallenge(index);
                }
                else
                {
                    Debug.LogError("Animal non trouvé dans le tableau.");
                }
            }
        }
    }
}
