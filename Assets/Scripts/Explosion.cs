using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float Radius = 1.5f;
    [SerializeField] int damage = 1;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Explode();
    }
    void ODrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Radius);

        foreach (Collider hitCollider in hitColliders)
        {
            PlayerHealth playerhealth = hitCollider.GetComponent<PlayerHealth>();

            if (!playerhealth) continue;

            playerhealth.TakeDamage(damage);
            audioSource.Play();

            break;
        }
    }
}
