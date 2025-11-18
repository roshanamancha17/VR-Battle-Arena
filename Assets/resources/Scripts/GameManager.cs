using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Match References")]
    public BaseHealth playerBase;
    public BaseHealth enemyBase;

    [Header("State")]
    public MatchState currentState = MatchState.None;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetState(MatchState.Title);
    }

    public void SetState(MatchState newState)
    {
        currentState = newState;
        Debug.Log("Match State changed to: " + newState);

        switch (newState)
        {
            case MatchState.Title:
                // TODO: Show Title UI
                break;

            case MatchState.Playing:
                // Start gameplay timer later
                break;

            case MatchState.Victory:
                // TODO: Show win UI
                break;

            case MatchState.Defeat:
                // TODO: Show lose UI
                break;

            case MatchState.Results:
                // TODO: Results screen
                break;
        }
    }

    // Called by BaseHealth when a base reaches 0 HP
    public void OnBaseDestroyed(bool isPlayer)
    {
        if (currentState != MatchState.Playing) return;

        if (isPlayer)
            SetState(MatchState.Defeat);
        else
            SetState(MatchState.Victory);

        // Go to results screen
        SetState(MatchState.Results);
    }
}
