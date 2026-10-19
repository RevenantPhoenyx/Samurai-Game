using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private UIManager uiManager;

    private int coinsCollected = 0;
    private bool room3EnemyDefeated = false;

    private int enemiesInLevel2 = 0;
    private int enemiesDefeatedInLevel2 = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void AddCoin()
    {
        coinsCollected++;

        // Only check Level 1 win condition in Level 1
        if (SceneManager.GetActiveScene().buildIndex == 1)
            CheckWinCondition();
    }


    public int GetCoinCount()
    {
        return coinsCollected;
    }

    public void EnemyDefeatedInRoom3()
    {
        room3EnemyDefeated = true;

        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (room3EnemyDefeated)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level == 2) // Level 2
        {
            room3EnemyDefeated = false;
            enemiesInLevel2 = 0;
            enemiesDefeatedInLevel2 = 0;
        }
    }

    public void RegisterLevel2Enemy()
    {
        enemiesInLevel2++;
    }

    public void Level2EnemyDefeated()
    {
        enemiesDefeatedInLevel2++;
        CheckFinalWinCondition();
    }

    private void CheckFinalWinCondition()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) // Level 2
        {
            if (enemiesDefeatedInLevel2 >= enemiesInLevel2)
            {
                // Refresh UIManager reference to avoid destroyed object
                if (uiManager == null)
                    uiManager = FindObjectOfType<UIManager>();

                uiManager.YouWin();
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //If in the main menu, (build index 0), destroy the game manager
        if(scene.buildIndex == 0)
        {
            Destroy(gameObject);
            return;
        }

        // Find the UIManager in the newly loaded scene
        uiManager = FindObjectOfType<UIManager>();
    }
}