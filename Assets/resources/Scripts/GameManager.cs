using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MatchState currentState = MatchState.None;
    public bool playerWon = false;     // Result shared with Results scene

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetState(MatchState.Title);
    }

    public void StartMatch()
    {
        playerWon = false;
        SetState(MatchState.Playing);
        SceneManager.LoadScene("Main");  // your battle scene name
    }

    public void OnBaseDestroyed(bool isPlayerBase)
    {
        if (currentState != MatchState.Playing) return;

        playerWon = !isPlayerBase;

        if (playerWon)
            SetState(MatchState.Victory);
        else
            SetState(MatchState.Defeat);

        SceneManager.LoadScene("Results");   // Single results screen
    }

    public void RestartMatch()
    {
        StartMatch();
    }

    public void GoToTitle()
    {
        SetState(MatchState.Title);
        SceneManager.LoadScene("Title");
    }

    public void SetState(MatchState newState)
    {
        currentState = newState;
        Debug.Log("Match State changed to: " + newState);

    }

    public void QuitGame()
    {
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
