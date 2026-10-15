
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] CanvasGroup gameOverCanvasGroup;

    [Header("Death Audio")]
    [SerializeField] AudioSource deathAudio;

    [Header("Cinematic Settings")]
    [SerializeField] float slowMotionScale = 0.12f;
    [SerializeField] float slowMotionDuration = 2f;
    [SerializeField] float screenFadeDuration = 2f;
    [SerializeField] float musicLeadIn = 1.5f;

    [Header("Win Condition")]
    [SerializeField] WinConditionManager winConditionManager;

    bool isGameOver;

    public bool IsGameOver => isGameOver;

    void Start()
    {
        Time.timeScale = 1f;
        isGameOver = false;

        if (winConditionManager == null)
            winConditionManager =
                FindFirstObjectByType<WinConditionManager>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (gameOverCanvasGroup != null)
            gameOverCanvasGroup.alpha = 0f;
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        // Losing takes priority over an unfinished win cinematic.
        if (winConditionManager != null)
            winConditionManager.CancelVictory();

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        Time.timeScale = slowMotionScale;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (deathAudio != null)
            deathAudio.Play();

        yield return new WaitForSecondsRealtime(
            slowMotionDuration
        );

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(musicLeadIn);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverCanvasGroup != null)
        {
            float elapsed = 0f;

            while (elapsed < screenFadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;

                gameOverCanvasGroup.alpha = Mathf.Clamp01(
                    elapsed / screenFadeDuration
                );

                yield return null;
            }

            gameOverCanvasGroup.alpha = 1f;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
