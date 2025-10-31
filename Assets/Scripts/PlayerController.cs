// // using UnityEngine;

// // public class PlayerController : MonoBehaviour
// // {
// //     // Start is called once before the first execution of Update after the MonoBehaviour is created
// //     void Start()
// //     {

// //     }

// //     // Update is called once per frame
// //     void Update()
// //     {

// //     }
// // }


// // --- PlayerController.cs ---
// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Transform playerModel; // Assign the PlayerShape here

//     // [SerializeField] private AudioSource sfxSource;

//     [Header("Movement")]
//     [SerializeField] private float rotationSpeed = 10f;

//     [Header("Audio")]
//     [SerializeField] private AudioClip moveSfx;

//     // 0=Bottom, 1=Right, 2=Top, 3=Left
//     private int currentLane = 0;
//     private Quaternion targetRotation;

//     // Target rotations for each lane
//     private readonly Quaternion[] laneRotations = {
//         Quaternion.Euler(0, 0, 0),    // Bottom
//         Quaternion.Euler(0, 0, -90),  // Right
//         Quaternion.Euler(0, 0, 180),  // Top
//         Quaternion.Euler(0, 0, 90)    // Left
//     };

//     void Update()
//     {
//         // Smoothly rotate the entire rig to the target lane
//         transform.rotation = Quaternion.Lerp(
//             transform.rotation,
//             targetRotation,
//             Time.deltaTime * rotationSpeed
//         );
//     }

//     // Called by InputManager
//     public void SetLane(int laneIndex)
//     {
//         if (currentLane == laneIndex) return; // No change

//         currentLane = laneIndex;
//         targetRotation = laneRotations[currentLane];

//         // sfxSource.PlayOneShot(moveSfx);

//     }

//     // --- Collision Detection ---
//     // This is on the PlayerShape child object, so we add a separate script there.
//     // We will create PlayerCollision.cs next.

//     public void ResetPosition()
//     {
//         // Reset rotation for game restart
//         currentLane = 0;
//         targetRotation = laneRotations[0];
//         transform.rotation = targetRotation;
//     }
// }








// // --- PlayerController.cs ---
// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Transform playerModel; // Assign PlayerVisuals here
//     [SerializeField] private AudioSource sfxSource;
//     [SerializeField] private AudioClip moveSfx;
//     [SerializeField] private AudioClip changeShapeSfx; // Add this

//     [Header("Movement")]
//     [SerializeField] private float rotationSpeed = 10f;

//     [Header("Shape Visuals")]
//     [SerializeField] private GameObject cubeModel;
//     [SerializeField] private GameObject sphereModel;
//     [SerializeField] private GameObject pyramidModel;

//     // This is the player's current shape
//     public ShapeType currentShape { get; private set; }

//     private int currentLane = 0;
//     private Quaternion targetRotation;
//     private readonly Quaternion[] laneRotations = {
//         Quaternion.Euler(0, 0, 0),    // Bottom
//         Quaternion.Euler(0, 0, -90),  // Right
//         Quaternion.Euler(0, 0, 180),  // Top
//         Quaternion.Euler(0, 0, 90)    // Left
//     };

//     void Start()
//     {
//         // Start the game as a Cube
//         ChangeShape(ShapeType.Cube);
//     }

//     void Update()
//     {
//         // --- DEBUG: PC Arrow Key Controls ---
//         if (Input.GetKeyDown(KeyCode.DownArrow)) SetLane(0);
//         else if (Input.GetKeyDown(KeyCode.RightArrow)) SetLane(1);
//         else if (Input.GetKeyDown(KeyCode.UpArrow)) SetLane(2);
//         else if (Input.GetKeyDown(KeyCode.LeftArrow)) SetLane(3);

//         // Smoothly rotate
//         transform.rotation = Quaternion.Lerp(
//             transform.rotation,
//             targetRotation,
//             Time.deltaTime * rotationSpeed
//         );
//     }

//     public void SetLane(int laneIndex)
//     {
//         if (currentLane == laneIndex) return;
//         currentLane = laneIndex;
//         targetRotation = laneRotations[currentLane];
//         // sfxSource.PlayOneShot(moveSfx);
//     }

//     // This is called after a successful match!
//     public void ChangeToRandomShape()
//     {
//         // Pick a new shape that ISN'T the current one
//         ShapeType newShape = currentShape;
//         while (newShape == currentShape)
//         {
//             int randomIndex = Random.Range(0, 3); // 0=Cube, 1=Sphere, 2=Pyramid
//             newShape = (ShapeType)randomIndex;
//         }

//         ChangeShape(newShape);
//     }

//     // This function updates the state and the visuals
//     private void ChangeShape(ShapeType newShape)
//     {
//         currentShape = newShape;

//         // Update visuals
//         cubeModel.SetActive(newShape == ShapeType.Cube);
//         sphereModel.SetActive(newShape == ShapeType.Sphere);
//         pyramidModel.SetActive(newShape == ShapeType.Pyramid);

//         // sfxSource.PlayOneShot(changeShapeSfx);
//     }

//     public void ResetPosition()
//     {
//         currentLane = 0;
//         targetRotation = laneRotations[0];
//         transform.rotation = targetRotation;
//         ChangeShape(ShapeType.Cube); // Reset to cube
//     }
// }






// --- PlayerController.cs (Simplified) ---
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip moveSfx;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 10f;
    
    // This is the player's shape for the whole game.
    // Set this in the Inspector!
    [Header("Shape")]
    public ShapeType currentShape = ShapeType.Cube; 

    // We only need the visual for our one shape.
    [SerializeField] private GameObject playerVisualModel; 

    private int currentLane = 0;
    private Quaternion targetRotation;
    private readonly Quaternion[] laneRotations = {
        Quaternion.Euler(0, 0, 0),    // Bottom
        Quaternion.Euler(0, 0, 90),  // Right
        Quaternion.Euler(0, 0, 180),  // Top
        Quaternion.Euler(0, 0, -90)    // Left
    };

    void Start()
    {
        // Make sure only the correct visual is active.
        playerVisualModel.SetActive(true); 
    }

    void Update()
    {
        // --- DEBUG: PC Arrow Key Controls ---
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetLane(0);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) SetLane(1);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) SetLane(2);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) SetLane(3);
        
        // Smoothly rotate
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }
    
    public void SetLane(int laneIndex)
    {
        if (currentLane == laneIndex) return;
        currentLane = laneIndex;
        targetRotation = laneRotations[currentLane];
        sfxSource.PlayOneShot(moveSfx);
    }
    
    public void ResetPosition()
    {
        currentLane = 0;
        targetRotation = laneRotations[0];
        transform.rotation = targetRotation;
    }
}