
using Unity.Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem MuzzleFlash;
    [SerializeField] LayerMask interactionLayers;
    [SerializeField] AudioSource audioSource;

    CinemachineImpulseSource ImpulseSource;

    void Awake()
    {
        ImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public bool Shoot(WeaponSO weaponSO)
    {
        RaycastHit hit;

        ImpulseSource.GenerateImpulse();
        MuzzleFlash.Play();

        if (weaponSO.ShootSound != null)
        {
            audioSource.PlayOneShot(weaponSO.ShootSound);
        }

        if (Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            Mathf.Infinity,
            interactionLayers,
            QueryTriggerInteraction.Ignore))
        {
            Instantiate(
                weaponSO.HitVFXPrefab,
                hit.point,
                Quaternion.identity
            );

            Enemyhealth enemyhealth =
                hit.collider.GetComponentInParent<Enemyhealth>();

            enemyhealth?.TakeDamage(weaponSO.Damage);
        }

        return true;
    }
}
