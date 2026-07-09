using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem MuzzleFlash;
    [SerializeField] int damageAmount = 1;
    StarterAssetsInputs starterAssetsInputs;

    void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>(); 
    }
    void Start()
    {
        starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
    }
    void Update()
    {
        bool flowControl = HandleShoot();
        if (!flowControl)
        {
            return;
        }
    }

    bool HandleShoot()
    {
        if (!starterAssetsInputs.Shoot) return false;
        MuzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            Enemyhealth enemyhealth = hit.collider.GetComponent<Enemyhealth>();
            enemyhealth?.TakeDamage(damageAmount);
            starterAssetsInputs.ShootInput(false);
        }
        return true;
    }
}
