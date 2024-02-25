using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<Weapon> availableWeapons;
    private Weapon currentWeapon;

    private void Start()
    {
        GameObject pistolPrefab = Resources.Load<GameObject>("Weapon_Projectile");
        
        // Instantiate and set up weapons here
        Pistol pistol = gameObject.AddComponent<Pistol>();
        pistol.SetProjectilePrefab(pistolPrefab);
        AddWeapon(pistol);
        
        Shotgun shotgun = gameObject.AddComponent<Shotgun>();
        shotgun.SetProjectilePrefab(pistolPrefab);
        AddWeapon(shotgun);
        
        // Set initial weapon (Pistol in this case)
        SwitchWeapon(0);
    }

    public void AddWeapon(Weapon weapon)
    {
        availableWeapons.Add(weapon);
    }
    
    public void SwitchWeapon(int index)
    {
        if (index >= 0 && index < availableWeapons.Count)
        {
            currentWeapon = availableWeapons[index];
        }
    }

    public void FireCurrentWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }
}