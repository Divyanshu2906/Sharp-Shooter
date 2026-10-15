using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionManager : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text enemiesLeftText;
    const string ENEMIES_LEFT_STRING = "Enemies Left: ";

    [Header("Victory UI")]
    [SerializeField] GameObject winPanel;
    [SerializeField] CanvasGroup winCanvasGroup;

    [Header("Victory Music")]
    [SerializeField] AudioSource victoryMusic;

    [Header("Cinematic Settings")]
    [SerializeField] float slowMotionScale = 0.15f;
    [SerializeField] float slowMotionDuration = 1.5f;
    [SerializeField] float musicLeadIn = 0.8f;
    [SerializeField] float screenFadeDuration = 1.5f;

    [Header("Lose Condition")]
    [SerializeField] GameOverManager gameOverManager;

    [Header("Player Controls")]
    [SerializeField] Behaviour[] playerControlScripts;

    int kills;
    bool hasWon;
    Coroutine victoryCoroutine;

    int enemiesLeft;
    bool hasSeenEnemy;

    void Start()
    {
        if (gameOverManager == null)
        {
            gameOverManager =
                FindFirstObjectByType<GameOverManager>();
        }

        if (winPanel != null)
            winPanel.SetActive(false);

        if (winCanvasGroup != null)
            winCanvasGroup.alpha = 0f;
    }

    
    public void AdjustEnemiesLeft(int amount)
    {
        if (hasWon)
            return;

        if (gameOverManager != null && gameOverManager.IsGameOver)
            return;

        enemiesLeft = Mathf.Max(0, enemiesLeft + amount);

        if (amount > 0)
            hasSeenEnemy = true;

        // Update the UI after calculating the final count.
        if (enemiesLeftText != null)
        {
            enemiesLeftText.text = ENEMIES_LEFT_STRING + enemiesLeft;
        }
        else
        {
            Debug.LogError("Enemies Left Text is not assigned!", this);
        }

        Debug.Log("Enemies Left: " + enemiesLeft);

        if (hasSeenEnemy && enemiesLeft == 0)
        {
            hasWon = true;
            victoryCoroutine = StartCoroutine(VictorySequence());
        }
    }

    IEnumerator VictorySequence()
    {

        SetPlayerControlsEnabled(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Slow down the final moment.
        Time.timeScale = slowMotionScale;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (victoryMusic != null)
            victoryMusic.Play();

        yield return new WaitForSecondsRealtime(
            slowMotionDuration
        );

        // Freeze gameplay while the victory screen appears.
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(musicLeadIn);

        if (winPanel != null)
            winPanel.SetActive(true);

        // Fade in the victory screen.
        if (winCanvasGroup != null)
        {
            float elapsed = 0f;

            while (elapsed < screenFadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;

                winCanvasGroup.alpha = Mathf.Clamp01(
                    elapsed / screenFadeDuration
                );

                yield return null;
            }

            winCanvasGroup.alpha = 1f;
        }

        victoryCoroutine = null;
    }

    public void CancelVictory()
    {
        if (victoryCoroutine != null)
        {
            StopCoroutine(victoryCoroutine);
            victoryCoroutine = null;
        }

        if (victoryMusic != null)
            victoryMusic.Stop();

        if (winPanel != null)
            winPanel.SetActive(false);

        if (winCanvasGroup != null)
            winCanvasGroup.alpha = 0f;

        // Allow the lose cinematic to take over.
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    
    void SetPlayerControlsEnabled(bool isEnabled)
    {
        foreach (Behaviour controlScript in playerControlScripts)
        {
            if (controlScript == null)
                continue;

            controlScript.enabled = isEnabled;

            if (controlScript is StarterAssets.StarterAssetsInputs inputs)
            {
                inputs.cursorLocked = isEnabled;
                inputs.cursorInputForLook = isEnabled;
            }
        }
    }
}
