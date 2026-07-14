using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] float rotationspeed = 100f;
    const string PLAYER_STRING = "Player";

    void Update()
    {
        transform.Rotate(0f, rotationspeed*Time.deltaTime, 0f);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
            ActiveWeapon activeWeapon = other.GetComponentInChildren<ActiveWeapon>();
            OnPickUp(activeWeapon);
            Destroy(this.gameObject);
        }
    }

    protected abstract void OnPickUp(ActiveWeapon activeWeapon);
}
