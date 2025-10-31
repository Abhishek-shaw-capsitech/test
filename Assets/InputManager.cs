// using UnityEngine;

// public class InputManager : MonoBehaviour
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

// using UnityEngine;
// using UnityEngine.InputSystem;

// public class InputManager : MonoBehaviour
// {
//     [SerializeField] private PlayerController playerController;
//     [SerializeField] private float minSwipeDistance = 50f;

//     private PlayerControls playerControls;
//     private Vector2 swipeStartPosition;

//     private void Awake()
//     {
//         // playerController = FindObjectOfType<PlayerController>();
//         playerController = FindFirstObjectByType<PlayerController>();
//         playerControls = new PlayerControls();

//         playerControls.Game.PrimaryContact.started += OnTouchStart;
//         playerControls.Game.PrimaryContact.canceled += OnTouchEnd;
//     }

//     private void OnEnable() => playerControls.Enable();
//     private void OnDisable() => playerControls.Disable();

//     private void OnTouchStart(InputAction.CallbackContext context)
//     {
//         swipeStartPosition = playerControls.Game.PrimaryPosition.ReadValue<Vector2>();
//     }

//     private void OnTouchEnd(InputAction.CallbackContext context)
//     {
//         if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

//         Vector2 swipeEndPosition = playerControls.Game.PrimaryPosition.ReadValue<Vector2>();
//         Vector2 swipeDelta = swipeEndPosition - swipeStartPosition;

//         if (swipeDelta.magnitude < minSwipeDistance) return; // Not a swipe

//         if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
//         {
//             if (swipeDelta.x > 0) playerController.SetLane(1); // Right
//             else playerController.SetLane(3); // Left
//         }
//         else
//         {
//             if (swipeDelta.y > 0) playerController.SetLane(2); // Up
//             else playerController.SetLane(0); // Down
//         }
//     }
// }


// --- InputManager.cs (Robust Version) ---
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float minSwipeDistance = 50f;

    private PlayerControls playerControls;
    private Vector2 swipeStartPosition;

    // Awake is used to create objects and find references
    private void Awake()
    {
        // Find the player
        if (playerController == null)
        {
            // playerController = FindObjectOfType<PlayerController>();
            playerController = FindFirstObjectByType<PlayerController>();
        }
        
        // Create the controls object
        playerControls = new PlayerControls();
    }

    // OnEnable is used to subscribe to events and enable controls
    private void OnEnable()
    {
        // Subscribe to the touch events
        playerControls.Game.PrimaryContact.started += OnTouchStart;
        playerControls.Game.PrimaryContact.canceled += OnTouchEnd;

        // Enable the action map
        playerControls.Game.Enable();
        
        playerControls.Game.MoveUp.performed += ctx => playerController.SetLane(2);
        playerControls.Game.MoveDown.performed += ctx => playerController.SetLane(0);
        playerControls.Game.MoveLeft.performed += ctx => playerController.SetLane(3);
        playerControls.Game.MoveRight.performed += ctx => playerController.SetLane(1);
    }

    // OnDisable is used to unsubscribe from events and disable controls
    private void OnDisable()
    {
        // Unsubscribe to prevent errors
        playerControls.Game.PrimaryContact.started -= OnTouchStart;
        playerControls.Game.PrimaryContact.canceled -= OnTouchEnd;

        // Disable the action map
        playerControls.Game.Disable();
        
        playerControls.Game.MoveUp.performed -= ctx => playerController.SetLane(2);
        playerControls.Game.MoveDown.performed -= ctx => playerController.SetLane(0);
        playerControls.Game.MoveLeft.performed -= ctx => playerController.SetLane(3);
        playerControls.Game.MoveRight.performed -= ctx => playerController.SetLane(1);

        
    }

    private void OnTouchStart(InputAction.CallbackContext context)
    {
        swipeStartPosition = playerControls.Game.PrimaryPosition.ReadValue<Vector2>();
    }

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        Vector2 swipeEndPosition = playerControls.Game.PrimaryPosition.ReadValue<Vector2>();
        Vector2 swipeDelta = swipeEndPosition - swipeStartPosition;

        if (swipeDelta.magnitude < minSwipeDistance) return; // Not a swipe

        // Determine direction
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            // Horizontal
            if (swipeDelta.x > 0) playerController.SetLane(1); // Right
            else playerController.SetLane(3); // Left
        }
        else
        {
            // Vertical
            if (swipeDelta.y > 0) playerController.SetLane(2); // Up
            else playerController.SetLane(0); // Down
        }
    }
}