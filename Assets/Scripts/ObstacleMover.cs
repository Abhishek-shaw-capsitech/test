// using UnityEngine;

// public class ObstacleMover : MonoBehaviour
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


// --- ObstacleMover.cs ---
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // Move towards the player (who is at z=0)
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Destroy when it passes the player
        if (transform.position.z < -15f)
        {
            Destroy(gameObject);
        }
    }
}