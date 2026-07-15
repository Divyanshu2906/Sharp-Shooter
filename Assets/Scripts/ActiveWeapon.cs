using StarterAssets;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] WeaponSO StartingWeapon;
    [SerializeField] TMP_Text AmmoText;
    WeaponSO CurrentWeaponSO;
    [SerializeField] CinemachineVirtualCamera PlayerFollowCamera;   
    [SerializeField] GameObject Zoom;
    Animator animator;
    FirstPersonController firstPersonController;
    StarterAssetsInputs starterAssetsInputs;
    Weapon CurrentWeapon;
    const string Shoot_string = "Shooting";
    float TimeSinceLastShot = 0f;
    float DefaultFOV;
    float DefaultRotationSpeed;
    int CurrentAmmo;

    void Awake()
    {
        animator = GetComponent<Animator>();
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>(); 
        firstPersonController = GetComponentInParent<FirstPersonController>();
        DefaultFOV = PlayerFollowCamera.m_Lens.FieldOfView;
        DefaultRotationSpeed = firstPersonController.RotationSpeed;
    }
    void Start()
    {
        SwitchWeapon(StartingWeapon);
        AdjustAmmo(CurrentWeaponSO.MagazineSize);
    }
    void Update()
    { 
        HandleZoom();
        bool flowControl = Shoot();
        if (!flowControl)
        {
            return;
        }
        
    }

    public void AdjustAmmo(int amount)
    {
        CurrentAmmo += amount;
        if(CurrentAmmo > CurrentWeaponSO.MagazineSize)
        {
            CurrentAmmo = CurrentWeaponSO.MagazineSize;
        } 
        AmmoText.text = CurrentAmmo.ToString("D2");


    }

    public void SwitchWeapon(WeaponSO weaponSO) 
    {
        if (CurrentWeapon)
        {
            Destroy(CurrentWeapon.gameObject);
        }
        Weapon newWeapon = Instantiate(weaponSO.weaponPrefab, transform).GetComponent<Weapon>();
        CurrentWeapon = newWeapon;
        this.CurrentWeaponSO = weaponSO;
        AdjustAmmo(CurrentWeaponSO.MagazineSize);

    }



    private bool Shoot()
    {
        TimeSinceLastShot += Time.deltaTime;
        if (!starterAssetsInputs.Shoot) return false;

        if(TimeSinceLastShot >= CurrentWeaponSO.FireRate && CurrentAmmo > 0)
        {
            CurrentWeapon.Shoot(CurrentWeaponSO);
            animator.Play(Shoot_string, 0, 0f);
            TimeSinceLastShot = 0f;
            AdjustAmmo(-1);
        }

        if (!CurrentWeaponSO.IsAutomatic)
        {
            starterAssetsInputs.ShootInput(false);    
        }
        
        return true;
    }

     void HandleZoom()
    {
        if (!CurrentWeaponSO.CanZoom) return;

        if (starterAssetsInputs.zoom) 
        {
            PlayerFollowCamera.m_Lens.FieldOfView = CurrentWeaponSO.ZoomAmount;
            Zoom.SetActive(true);
            firstPersonController.ChangeRotationSpeed(CurrentWeaponSO.ZoomRotationSpeed);
        }
        else 
        {
            PlayerFollowCamera.m_Lens.FieldOfView = DefaultFOV;
            Zoom.SetActive(false);
            firstPersonController.ChangeRotationSpeed(DefaultRotationSpeed);
        }
    }

}
