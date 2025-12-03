using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerVR : MonoBehaviour
{
    public static GameManagerVR Instance;

    [Header("Timer")]
    public float matchTime = 180f; // 3 minutes
    public TextMeshProUGUI timerText;

    [Header("UI")]
    public GameObject winUI;
    public GameObject loseUI;

    public bool matchEnded = false;
    public bool MatchEnded => matchEnded;   // <-- ADD THIS


    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (matchEnded) return;

        matchTime -= Time.deltaTime;
        UpdateTimerUI();

        if (matchTime <= 0f)
        {
            matchEnded = true;
            TimeOverCheck();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(matchTime / 60f);
        int seconds = Mathf.FloorToInt(matchTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void TimeOverCheck()
    {
        BaseHealth playerBase = GameObject.FindWithTag("PlayerBase").GetComponent<BaseHealth>();
        BaseHealth enemyBase = GameObject.FindWithTag("EnemyBase").GetComponent<BaseHealth>();

        if (playerBase.currentHealth > enemyBase.currentHealth)
            Win();
        else
            Lose();
    }

    public void OnBaseDestroyed(bool playerLost)
    {
        if (playerLost)
            Lose();
        else
            Win();
    }

    public void Win()
    {
        matchEnded = true;
        winUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Lose()
    {
        matchEnded = true;
        loseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // stop play mode in editor
    #else
        Application.Quit();  // quit app on real device / build
    #endif
    }

}
