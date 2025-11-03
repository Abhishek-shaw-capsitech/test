// using UnityEngine;

// public class SettingsManager : MonoBehaviour
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
// using UnityEngine.UI; // Required for UI Images

// public class SettingsManager : MonoBehaviour
// {
//     public static SettingsManager Instance;

//     [Header("UI References")]
//     [SerializeField] private Image soundButtonIcon;
//     [SerializeField] private Sprite soundOnSprite;
//     [SerializeField] private Sprite soundOffSprite;

//     private bool isMuted = false;

//     private void Awake()
//     {
//         // Singleton Pattern (so it's easy to find)
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject); // Persists between scenes
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }

//         // Load the saved setting
//         LoadSettings();
//     }

//     private void Start()
//     {
//         // Apply the loaded setting at the start
//         UpdateAudioState(); 
//     }

//     private void LoadSettings()
//     {
//         // Get the saved value from the phone's storage. Default to 0 (not muted).
//         isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
//     }

//     private void SaveSettings()
//     {
//         // Save the current value (1 for muted, 0 for not muted)
//         PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
//         PlayerPrefs.Save();
//     }

//     private void UpdateAudioState()
//     {
//         if (isMuted)
//         {
//             // This mutes ALL sound in Unity
//             AudioListener.volume = 0f; 
//             if (soundButtonIcon != null)
//                 soundButtonIcon.sprite = soundOffSprite;
//         }
//         else
//         {
//             // This un-mutes all sound
//             AudioListener.volume = 1f;
//             if (soundButtonIcon != null)
//                 soundButtonIcon.sprite = soundOnSprite;
//         }
//     }

//     // This is the public function your button will call
//     public void ToggleMute()
//     {
//         isMuted = !isMuted; // Flip the value
//         UpdateAudioState();
//         SaveSettings();
//     }
// }



using UnityEngine;
using System; // We need this for "Action"

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    
    // This is an "event" that other scripts can listen to.
    // It will fire whenever the mute state changes.
    public static event Action OnSoundStateChanged;

    // "isMuted" is now a public property.
    // "private set" means other scripts can READ it, but only THIS script can CHANGE it.
    public bool isMuted { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load the saved setting
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        UpdateAudioState();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        UpdateAudioState();
        SaveSettings();
    }

    private void UpdateAudioState()
    {
        // 1. Set the game's volume
        AudioListener.volume = isMuted ? 0f : 1f;

        // 2. "Shout" to all listeners (the buttons) that the state has changed
        OnSoundStateChanged?.Invoke();
    }
    
    private void SaveSettings()
    {
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}