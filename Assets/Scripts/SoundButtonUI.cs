// using UnityEngine;

// public class SoundButtonUI : MonoBehaviour
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
using UnityEngine.UI; // We need this to control the Image

[RequireComponent(typeof(Image))] // Automatically adds an Image component
public class SoundButtonUI : MonoBehaviour
{
    [Header("Icon Sprites")]
    [SerializeField] private Sprite soundOnSprite;   // The 'Sound On' (🔈) sprite
    [SerializeField] private Sprite soundOffSprite;  // The 'Sound Off' (🔇) sprite

    private Image buttonImage;
    private Button button;

    private void Awake()
    {
        // Get the Image component on this same object
        buttonImage = GetComponent<Image>();
        
        // --- Hook up the button's OnClick event ---
        button = GetComponent<Button>();
        if (button != null)
        {
            // Tell our button to call the SettingsManager's function when clicked
            button.onClick.AddListener(SettingsManager.Instance.ToggleMute);
        }
    }

    // This is called when the script first loads
    private void OnEnable()
    {
        // "Subscribe" to the SettingsManager's event
        SettingsManager.OnSoundStateChanged += UpdateIcon;
        
        // Immediately check the state to set the correct icon on load
        UpdateIcon();
    }

    // This is called when the object is disabled or destroyed
    private void OnDisable()
    {
        // "Unsubscribe" to prevent errors
        SettingsManager.OnSoundStateChanged -= UpdateIcon;
    }

    // This function runs when the SettingsManager "shouts"
    private void UpdateIcon()
    {
        // Check the SettingsManager's state and set the correct sprite
        if (SettingsManager.Instance.isMuted)
        {
            buttonImage.sprite = soundOffSprite;
        }
        else
        {
            buttonImage.sprite = soundOnSprite;
        }
    }
}