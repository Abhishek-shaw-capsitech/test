// using UnityEngine;

// public class Obstacle : MonoBehaviour
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


// --- Obstacle.cs ---
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // This is set in the prefab inspector
    public ShapeType shapeType; 
    // public float speed = 10f;

    void Update()
    {
        // Move towards the player
        // transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.back * Spawner.currentSpeed * Time.deltaTime, Space.World);

        // Destroy when it passes the player
        if (transform.position.z < -15f)
        {
            Destroy(gameObject);
        }
    }
}