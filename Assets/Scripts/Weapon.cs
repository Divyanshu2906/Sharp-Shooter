using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem MuzzleFlash;

    public bool Shoot(WeaponSO weaponSO)
    {
        RaycastHit hit;
        MuzzleFlash.Play();
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            Instantiate(weaponSO.HitVFXPrefab,hit.point,Quaternion.identity);
            Enemyhealth enemyhealth = hit.collider.GetComponent<Enemyhealth>();
            enemyhealth?.TakeDamage(weaponSO.Damage);
        }
        return true;
    }
}
