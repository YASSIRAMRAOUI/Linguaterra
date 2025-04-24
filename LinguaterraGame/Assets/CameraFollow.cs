using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float xOffset = 0f; // Offset on the X-axis
    public float yOffset = 0f; // Offset on the Y-axis
    public float xSmoothSpeed = 5f; // Smoothing for horizontal movement
    public float ySmoothSpeed = 2f; // Smoothing for vertical movement (if you want some)
    public float yClampMin = float.NegativeInfinity; // Minimum Y position of the camera
    public float yClampMax = float.PositiveInfinity; // Maximum Y position of the camera

    private float targetX;
    private float targetY;
    private float currentXVelocity;
    private float currentYVelocity;

    void LateUpdate() // Use LateUpdate to follow after player movement
    {
        if (player == null)
        {
            Debug.LogError("ERROR: Player reference not set in CameraFollow script!");
            return; // Exit if no player is assigned
        }

        // Calculate the target X position with offset and smoothing
        targetX = Mathf.SmoothDamp(transform.position.x, player.position.x + xOffset, ref currentXVelocity, xSmoothSpeed * Time.deltaTime);

        // Calculate the target Y position with offset and smoothing (optional)
        targetY = Mathf.SmoothDamp(transform.position.y, player.position.y + yOffset, ref currentYVelocity, ySmoothSpeed * Time.deltaTime);

        // Clamp the Y position if you want to restrict vertical movement
        targetY = Mathf.Clamp(targetY, yClampMin, yClampMax);

        // Update the camera's position
        transform.position = new Vector3(targetX, targetY, transform.position.z); // Keep the camera's Z position
    }

    // Optional: Method to set the player reference dynamically
    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}