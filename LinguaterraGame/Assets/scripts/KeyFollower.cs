using UnityEngine;

public class KeyFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 3f;
    private bool canFollow = false;

    void Update()
    {
        if (canFollow)
        {
            // On ne garde que X et Y, et fixe Z pour éviter qu'elle "disparaisse"
            Vector2 currentPos = transform.position;
            Vector2 targetPos = player.position;

            Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, followSpeed * Time.deltaTime);

            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            Debug.Log("Position de la clé: " + transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("La clé entre en collision avec le joueur");

            if (FindObjectOfType<WordManager>().AllLettersFound())
            {
                canFollow = true;
                Debug.Log("La clé commence à suivre le joueur !");
            }
            else
            {
                Debug.Log("La clé ne suit pas car toutes les lettres ne sont pas trouvées.");
            }
        }
    }
}
