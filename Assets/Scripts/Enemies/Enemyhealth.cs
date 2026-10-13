
using UnityEngine;

public class Enemyhealth : MonoBehaviour
{
    [SerializeField] GameObject RobotExplosionVFX;
    [SerializeField] int CurrentHealth = 0;
    [SerializeField] int StartingHealth = 1;

    WinConditionManager winConditionManager;
    bool hasDied;

    void Awake()
    {
        CurrentHealth = StartingHealth;
    }

    void Start()
    {
        winConditionManager =
            FindFirstObjectByType<WinConditionManager>();

        if (winConditionManager != null)
            winConditionManager.AdjustEnemiesLeft(1);
    }

    public void TakeDamage(int amount)
    {
        if (hasDied)
            return;

        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
            SelfDestruct();
    }

    public void SelfDestruct()
    {
        if (hasDied)
            return;

        hasDied = true;

        if (winConditionManager != null)
            winConditionManager.AdjustEnemiesLeft(-1);

        if (RobotExplosionVFX != null)
        {
            Instantiate(
                RobotExplosionVFX,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}
