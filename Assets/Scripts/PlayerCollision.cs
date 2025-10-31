// // using UnityEngine;

// // public class PlayerCollision : MonoBehaviour
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


// // --- PlayerCollision.cs ---
// using UnityEngine;

// public class PlayerCollision : MonoBehaviour
// {
//     // This script sits on the child object with the collider
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("SafeGate"))
//         {
//             // Passed through the safe gate
//             GameManager.Instance.IncreaseScore();
//             // Destroy the parent obstacle wall
//             Destroy(other.transform.parent.gameObject);
//         }
//         else if (other.CompareTag("Obstacle"))
//         {
//             // Hit a solid wall
//             GameManager.Instance.EndGame();
//             // Destroy the parent obstacle wall
//             Destroy(other.transform.parent.gameObject);
//         }
//     }
// }





// // --- PlayerCollision.cs ---
// using UnityEngine;

// public class PlayerCollision : MonoBehaviour
// {
//     // We need to tell the PlayerController to change shapes
//     [SerializeField] private PlayerController playerController;

//     // This script sits on PlayerVisuals (the child object)
//     private void OnTriggerEnter(Collider other)
//     {
//         // Check if we hit an obstacle
//         Obstacle obstacle = other.GetComponent<Obstacle>();
//         if (obstacle == null) return;

//         // Check for a shape match
//         if (obstacle.shapeType == playerController.currentShape)
//         {
//             // --- MATCH! ---
//             GameManager.Instance.IncreaseScore();
//             playerController.ChangeToRandomShape(); // Shift to a new shape!
//         }
//         else
//         {
//             // --- WRONG SHAPE! ---
//             GameManager.Instance.EndGame();
//         }

//         // Destroy the obstacle we hit
//         Destroy(other.gameObject);
//     }
// }


// --- PlayerCollision.cs (Simplified) ---
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // We only need the Player's SHAPE, not the whole controller
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle == null) return;
        
        // Check for a shape match
        if (obstacle.shapeType == playerController.currentShape)
        {
            // --- MATCH! ---
            GameManager.Instance.IncreaseScore();
            // We NO LONGER change shapes here.
        }
        else
        {
            // --- WRONG SHAPE! ---
            GameManager.Instance.EndGame();
        }

        // Destroy the obstacle we hit
        Destroy(other.gameObject);
    }
}