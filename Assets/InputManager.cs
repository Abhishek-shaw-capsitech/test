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

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float minSwipeDistance = 50f;

    private PlayerControls playerControls;
    private Vector2 swipeStartPosition;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerControls = new PlayerControls();

        playerControls.Game.PrimaryContact.started += OnTouchStart;
        playerControls.Game.PrimaryContact.canceled += OnTouchEnd;
    }

    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();

    private void OnTouchStart(InputAction.CallbackContext context)
    {
        swipeStartPosition = playerControls.Game.PrimaryPosition.ReadValue<Vector2>();
    }

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        Vector2 swipeEndPosition = playerControls.Game.PrimaryPosition.ReadValue<Vector2>();
        Vector2 swipeDelta = swipeEndPosition - swipeStartPosition;

        if (swipeDelta.magnitude < minSwipeDistance) return; // Not a swipe

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0) playerController.SetLane(1); // Right
            else playerController.SetLane(3); // Left
        }
        else
        {
            if (swipeDelta.y > 0) playerController.SetLane(2); // Up
            else playerController.SetLane(0); // Down
        }
    }
}
