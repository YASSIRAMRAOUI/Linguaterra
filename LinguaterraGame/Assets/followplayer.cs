using UnityEngine;

public class HorizontalCameraFollow : MonoBehaviour
{
    public Transform player;                // Le joueur à suivre
    public float horizontalThreshold = 4f;  // Distance horizontale à partir de laquelle la caméra suit
    public float smoothing = 5f;            // Pour lisser le mouvement

    private float fixedY;                   // On garde Y fixe
    private float fixedZ;                   // Z aussi, car c’est une vue 2D

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    void Update()
    {
        Vector3 camPos = transform.position;
        float playerX = player.position.x;

        if (Mathf.Abs(playerX - camPos.x) > horizontalThreshold)
        {
            float targetX = playerX;
            float smoothedX = Mathf.Lerp(camPos.x, targetX, smoothing * Time.deltaTime);
            transform.position = new Vector3(smoothedX, fixedY, fixedZ);
        }
    }
}
