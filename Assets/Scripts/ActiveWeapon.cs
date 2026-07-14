using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{

    [SerializeField] WeaponSO weaponSO;
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
        CurrentWeapon = GetComponentInChildren<Weapon>();
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

    public void SwitchWeapon(WeaponSO weaponSO) 
    {
        if (CurrentWeapon)
        {
            Destroy(CurrentWeapon.gameObject);
        }
        Weapon newWeapon = Instantiate(weaponSO.weaponPrefab, transform).GetComponent<Weapon>();
        CurrentWeapon = newWeapon;
        this.weaponSO = weaponSO;

    }



    private bool Shoot()
    {
        TimeSinceLastShot += Time.deltaTime;
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

     void HandleZoom()
    {
        if (!weaponSO.CanZoom) return;

        if (starterAssetsInputs.zoom) 
        {
            PlayerFollowCamera.m_Lens.FieldOfView = weaponSO.ZoomAmount;
            Zoom.SetActive(true);
            firstPersonController.ChangeRotationSpeed(weaponSO.ZoomRotationSpeed);
        }
        else 
        {
            PlayerFollowCamera.m_Lens.FieldOfView = DefaultFOV;
            Zoom.SetActive(false);
            firstPersonController.ChangeRotationSpeed(DefaultRotationSpeed);
        }
    }

}
