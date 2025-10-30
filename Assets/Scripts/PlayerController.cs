// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }


// --- PlayerController.cs ---
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerModel; // Assign the PlayerShape here

    // [SerializeField] private AudioSource sfxSource;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Audio")]
    [SerializeField] private AudioClip moveSfx;

    // 0=Bottom, 1=Right, 2=Top, 3=Left
    private int currentLane = 0;
    private Quaternion targetRotation;

    // Target rotations for each lane
    private readonly Quaternion[] laneRotations = {
        Quaternion.Euler(0, 0, 0),    // Bottom
        Quaternion.Euler(0, 0, -90),  // Right
        Quaternion.Euler(0, 0, 180),  // Top
        Quaternion.Euler(0, 0, 90)    // Left
    };

    void Update()
    {
        // Smoothly rotate the entire rig to the target lane
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    // Called by InputManager
    public void SetLane(int laneIndex)
    {
        if (currentLane == laneIndex) return; // No change

        currentLane = laneIndex;
        targetRotation = laneRotations[currentLane];
        
        // sfxSource.PlayOneShot(moveSfx);

    }

    // --- Collision Detection ---
    // This is on the PlayerShape child object, so we add a separate script there.
    // We will create PlayerCollision.cs next.

    public void ResetPosition()
    {
        // Reset rotation for game restart
        currentLane = 0;
        targetRotation = laneRotations[0];
        transform.rotation = targetRotation;
    }
}