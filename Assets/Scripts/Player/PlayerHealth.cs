using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int startingHealth = 10;

    [SerializeField] CinemachineCamera deathVirtualCamera;
    [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] shieldBars;

    [Header("Game Over")]
    [SerializeField] GameOverManager gameOverManager;

    int currentHealth;
    int gameOverVirtualCameraPriority = 20;
    bool isDead;

    void Awake()
    {
        currentHealth = startingHealth;
        AdjustShieldUI();
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        AdjustShieldUI();

        if (currentHealth <= 0)
        {
            isDead = true;

            weaponCamera.parent = null;
            deathVirtualCamera.Priority = gameOverVirtualCameraPriority;

            gameOverManager.GameOver();

            Destroy(gameObject);
        }
    }

    void AdjustShieldUI()
    {
        for (int i = 0; i < shieldBars.Length; i++)
        {
            if (i < currentHealth)
            {
                shieldBars[i].gameObject.SetActive(true);
            }
            else
            {
                shieldBars[i].gameObject.SetActive(false);
            }
        }
    }
}
