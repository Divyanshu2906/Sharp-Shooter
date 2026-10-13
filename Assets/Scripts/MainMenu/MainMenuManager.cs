using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Controls Panel")]
    [SerializeField] GameObject controlsPanel;

    [Header("Sound Button")]
    [SerializeField] TMP_Text soundButtonText;

    [Header("Gameplay Scene")]
    [SerializeField] string gameplaySceneName = "Level1";

    [Header("Button Sound")]
    [SerializeField] AudioSource buttonAudioSource;
    bool soundOn;
    public void PlayButtonSound()
    {
        if (buttonAudioSource != null)
        {
            buttonAudioSource.PlayOneShot(
                buttonAudioSource.clip
            );
        }
    }
    void Start()
    {
        // Keep the controls panel hidden when the menu opens.
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }

        // Read the current global audio state.
        soundOn = AudioListener.volume > 0f;

        UpdateSoundButton();
    }

    public void PlayGame()
    {
        if (string.IsNullOrWhiteSpace(gameplaySceneName))
        {
            Debug.LogError("Gameplay scene name is empty.");
            return;
        }

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Controls Panel is not assigned.");
        }
    }

    public void CloseControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;

        AudioListener.volume = soundOn ? 1f : 0f;

        UpdateSoundButton();
    }

    void UpdateSoundButton()
    {
        if (soundButtonText != null)
        {
            soundButtonText.text = soundOn
                ? "SOUND: ON"
                : "SOUND: OFF";
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exit button pressed.");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
