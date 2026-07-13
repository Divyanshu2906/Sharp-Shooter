using StarterAssets;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{

    [SerializeField] WeaponSO weaponSO;
    Animator animator;
    StarterAssetsInputs starterAssetsInputs;
    Weapon CurrentWeapon;
    const string Shoot_string = "Shooting";
    float TimeSinceLastShot = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>(); 
    }
    void Start()
    {
        CurrentWeapon = GetComponentInChildren<Weapon>();
    }
    void Update()
    {
        TimeSinceLastShot += Time.deltaTime; 
        bool flowControl = Shoot();
        if (!flowControl)
        {
            return;
        }
    }

    private bool Shoot()
    {
        if (!starterAssetsInputs.Shoot) return false;

        if(TimeSinceLastShot >= weaponSO.FireRate)
        {
            CurrentWeapon.Shoot(weaponSO);
            animator.Play(Shoot_string, 0, 0f);
            TimeSinceLastShot = 0f;
        }

        if (!weaponSO.IsAutomatic)
        {
            starterAssetsInputs.ShootInput(false);    
        }
        
        return true;
    }
}
