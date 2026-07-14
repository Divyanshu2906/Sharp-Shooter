using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem MuzzleFlash;
     [SerializeField] LayerMask interactionLayers;

    public bool Shoot(WeaponSO weaponSO)
    {
        RaycastHit hit;
        MuzzleFlash.Play();
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, interactionLayers, QueryTriggerInteraction.Ignore))
        {
            Instantiate(weaponSO.HitVFXPrefab,hit.point,Quaternion.identity);
            Enemyhealth enemyhealth = hit.collider.GetComponent<Enemyhealth>();
            enemyhealth?.TakeDamage(weaponSO.Damage);
        }
        return true;
    }
}
