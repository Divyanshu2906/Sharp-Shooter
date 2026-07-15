using UnityEngine;

public class AmmoPickup : Pickup
{
    [SerializeField] int AmmoAmount = 100;
    protected override void OnPickUp(ActiveWeapon activeWeapon)
    {
        activeWeapon.AdjustAmmo(AmmoAmount);
    }
}
