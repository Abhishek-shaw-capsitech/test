// using UnityEngine;

// public class GameManager : MonoBehaviour
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



// --- GameManager.cs ---
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Use TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState CurrentState { get; private set; }

    [Header("Object References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private Spawner spawner;

    [Header("UI Panels")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject endScreen;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("Audio")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip collisionSfx;
    [SerializeField] private AudioClip scoreSfx;
    [SerializeField] private AudioClip gameOverSfx;
    [SerializeField] private GameObject pausePanel;

    // [Header("VFX")]
    // [SerializeField] private ParticleSystem collisionParticles;
    // [SerializeField] private ParticleSystem scoreParticles;

    private int score;
    private int highScore;

    public enum GameState { MainMenu, Playing, GameOver, Paused }

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        ChangeState(GameState.MainMenu);
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // public void ChangeState(GameState newState)
    // {
    //     CurrentState = newState;
    //     switch (newState)
    //     {
    //         case GameState.MainMenu:
    //             Time.timeScale = 0f;
    //             startScreen.SetActive(true);
    //             inGameHUD.SetActive(false);
    //             endScreen.SetActive(false);
    //             break;

    //         case GameState.Playing:
    //             score = 0;
    //             scoreText.text = score.ToString();
    //             Time.timeScale = 1f;

    //             startScreen.SetActive(false);
    //             inGameHUD.SetActive(true);
    //             endScreen.SetActive(false);

    //             player.ResetPosition();
    //             spawner.StartSpawning();
    //             break;

    //         case GameState.GameOver:
    //             Time.timeScale = 0f;

    //             inGameHUD.SetActive(false);
    //             endScreen.SetActive(true);

    //             if (score > highScore)
    //             {
    //                 highScore = score;
    //                 PlayerPrefs.SetInt("HighScore", highScore);
    //             }
    //             finalScoreText.text = "SCORE: " + score;
    //             highScoreText.text = "HIGH: " + highScore;

    //             sfxSource.PlayOneShot(gameOverSfx);
    //             spawner.StopSpawning();
    //             break;
    //     }
    // }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 0f;
                startScreen.SetActive(true);
                inGameHUD.SetActive(false);
                endScreen.SetActive(false);
                pausePanel.SetActive(false); // <-- ADD THIS
                break;

            case GameState.Playing:
                // ... (your code) ...
                Time.timeScale = 1f;
                startScreen.SetActive(false);
                inGameHUD.SetActive(true);
                endScreen.SetActive(false);
                pausePanel.SetActive(false); // <-- ADD THIS
                spawner.StartSpawning();
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                inGameHUD.SetActive(false);
                endScreen.SetActive(true);
                pausePanel.SetActive(false); // <-- ADD THIS
                                             // ... (your code) ...
                spawner.StopSpawning();
                break;

                // We don't need a "case" for Paused, because TogglePause() handles it.
        }
    }

    // --- Public UI Methods ---
    public void OnPlayButton() => ChangeState(GameState.Playing);
    public void OnRetryButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void OnExitButton() => Application.Quit();

    // --- Gameplay Methods ---
    public void IncreaseScore()
    {
        score += 1;
        scoreText.text = score.ToString();
        sfxSource.PlayOneShot(scoreSfx);
        // if(scoreParticles) scoreParticles.Play();
    }

    public void EndGame()
    {
        if (CurrentState == GameState.GameOver) return;

        sfxSource.PlayOneShot(collisionSfx);
        // if(collisionParticles) collisionParticles.Play(); // Play at player's position
        ChangeState(GameState.GameOver);
    }

    // This function will be called by your "Resume" and "Pause" buttons
    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
        {
            // Pause the game
            CurrentState = GameState.Paused;
            Time.timeScale = 0f; // This freezes all physics and movement
            pausePanel.SetActive(true);
        }
        else if (CurrentState == GameState.Paused)
        {
            // Un-pause the game
            CurrentState = GameState.Playing;
            Time.timeScale = 1f; // This un-freezes the game
            pausePanel.SetActive(false);
        }
    }

    // This will be called by your "Exit to Menu" button
    public void ExitToMainMenu()
    {
        // Must un-pause time before going to the menu
        Time.timeScale = 1f;
        ChangeState(GameState.MainMenu);
    }


}