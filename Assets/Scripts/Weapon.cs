using StarterAssets;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject HitVFXPrefab ;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem MuzzleFlash;
    [SerializeField] int damageAmount = 1;
    StarterAssetsInputs starterAssetsInputs;
    const string Shoot_string = "Shooting";

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
        animator.Play(Shoot_string, 0, 0f);
        starterAssetsInputs.ShootInput(false);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            Instantiate(HitVFXPrefab,hit.point,Quaternion.identity);
            Enemyhealth enemyhealth = hit.collider.GetComponent<Enemyhealth>();
            enemyhealth?.TakeDamage(damageAmount);
            
        }
        return true;
    }
}
