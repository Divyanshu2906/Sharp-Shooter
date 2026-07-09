using System.Net.WebSockets;
using UnityEngine;

public class Enemyhealth : MonoBehaviour
{
    [SerializeField] int CurrentHealth = 0;
    [SerializeField] int StartingHealth = 3;

    void Awake()
    {
        CurrentHealth = StartingHealth;
    }
    
    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        
        if(CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
