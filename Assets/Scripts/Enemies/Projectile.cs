using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] GameObject projectileHitVFX;

    Rigidbody rb;

    int damage;
    
    AudioSource audiosource;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }

    void Start()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    public void Init(int damage) 
    {
        this.damage = damage;
    }

    
    void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        if (projectileHitVFX != null)
        {
            
            Instantiate(
                projectileHitVFX,
                transform.position,
                Quaternion.identity
            );
            audiosource.Play();
        }

        Destroy(gameObject);
    }
}
