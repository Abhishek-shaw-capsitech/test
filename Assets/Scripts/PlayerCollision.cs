// using UnityEngine;

// public class PlayerCollision : MonoBehaviour
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


// --- PlayerCollision.cs ---
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // This script sits on the child object with the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SafeGate"))
        {
            // Passed through the safe gate
            GameManager.Instance.IncreaseScore();
            // Destroy the parent obstacle wall
            Destroy(other.transform.parent.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            // Hit a solid wall
            GameManager.Instance.EndGame();
            // Destroy the parent obstacle wall
            Destroy(other.transform.parent.gameObject);
        }
    }
}