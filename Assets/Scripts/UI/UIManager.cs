using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header ("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("You Won")]
    [SerializeField] private GameObject youWinScreen;
    [SerializeField] private AudioClip youWinSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    [Header("Menu Screen")]
    [SerializeField] private GameObject menuScreen;

    [Header("Coin Reference")]
    [SerializeField] private GameObject coinCount;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        youWinScreen.SetActive(false);
        pauseScreen.SetActive(false);
        coinCount.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //if pause screen is already active unpause and viceversa
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }

    }

    #region Game Over

    //Activate GameOver Screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    //GameOver functions
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit(); //Quits the game (only on build game)

        //exits play mode in unity(only executed in the editor)
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false; 
     #endif
    }
    #endregion

    #region You Win
    //Activate You Won Screen
    public void YouWin()
    {
        youWinScreen.SetActive(true);
        SoundManager.instance.PlaySound(youWinSound);
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        //If status == true pause | if status == false unpause
        pauseScreen.SetActive(status);

        //when pause status is true change time scale to 0 when false its 1 (time goes by normally)
        if(status)
           Time.timeScale = 0;
        else
           Time.timeScale = 1;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion
}
