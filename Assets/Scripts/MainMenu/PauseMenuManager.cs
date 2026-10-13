
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] GameObject pausePanel;

    [Header("Player Controls")]
    [SerializeField] Behaviour[] playerControlScripts;
    [Header("Pause Music")]
    [SerializeField] AudioSource pauseMusic;

    bool isPaused;

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetPlayerControlsEnabled(true);
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused)
            return;

        isPaused = true;
        Time.timeScale = 0f;

        SetPlayerControlsEnabled(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (pauseMusic != null)
            pauseMusic.Play();
    }

    public void ResumeGame()
    {
        if (!isPaused)
            return;

        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (pauseMusic != null)
            pauseMusic.Stop();

        SetPlayerControlsEnabled(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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


    public void RestartGame()
    {
        if (pauseMusic != null)
            pauseMusic.Stop();
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ReturnToMainMenu()
    {
        if (pauseMusic != null)
            pauseMusic.Stop();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
