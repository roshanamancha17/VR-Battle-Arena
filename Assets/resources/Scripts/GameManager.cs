using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Match Flow")]
    public MatchState currentState = MatchState.None;
    public bool playerWon = false;

    [Header("UI Panels (assigned in Main Scene)")]
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    private bool gameEnded = false;

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
            return;
        }
    }

    private void Start()
    {
        SetState(MatchState.Title);
    }

    // ===== START MATCH =====
    public void StartMatch()
    {
        playerWon = false;
        Time.timeScale = 1f;

        SetState(MatchState.Playing);
        SceneManager.LoadScene("Main");  // Change if your gameplay scene has different name
    }

    // ===== BASE DESTROYED =====
    public void OnBaseDestroyed(bool isPlayerBase)
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 0f;
        playerWon = !isPlayerBase;  // Win result

        // Show correct UI panel
        GameObject panelToShow = isPlayerBase ? defeatPanel : victoryPanel;
        panelToShow.SetActive(true);

        // Put the panel in front of XR camera
        Transform cam = Camera.main.transform;
        panelToShow.transform.position = cam.position + cam.forward * 1.2f;
        panelToShow.transform.rotation = Quaternion.LookRotation(cam.forward);

        SetState(isPlayerBase ? MatchState.Defeat : MatchState.Victory);
    }

    // ===== BUTTON FUNCTIONS =====
    public void RestartMatch()
    {
        Time.timeScale = 1f;
        gameEnded = false;

        SceneManager.LoadScene("Main");
        SetState(MatchState.Playing);
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        gameEnded = false;

        SceneManager.LoadScene("Title");
        SetState(MatchState.Title);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ===== CHANGE STATE =====
    public void SetState(MatchState newState)
    {
        currentState = newState;
        Debug.Log("📌 Match State changed to: " + newState);
    }
}
