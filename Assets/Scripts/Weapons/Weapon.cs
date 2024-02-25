using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    protected static Camera MainCamera;

    public GameObject projectilePrefab;
    protected abstract float ProjectileSpeed { get; }
    protected abstract int MaxAmmunition { get; }

    protected enum WeaponState
    {
        Reloading,
        Ready,
    }

    protected WeaponState weaponState = WeaponState.Ready;
    protected int ammunition;

    protected abstract void ShootProjectile();

    public void Start() {
        MainCamera = Camera.main;
    }

    public void SetProjectilePrefab(GameObject prefab)
    {
        projectilePrefab = prefab;
    }
    
    public void Fire()
    {
        if (ammunition > 0 && weaponState == WeaponState.Ready)
        {
            ShootProjectile();
            ammunition--;
            UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, SetAmmoValue());
        }
        else if (ammunition <= 0)
        {
            Reload();
        }
    }

    protected void Reload()
    {
        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, "Reloading");
        //_weaponState = WeaponState.RELOADING;
        StartCoroutine(ReloadAfterDelay(2f));
    }

    protected IEnumerator ReloadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ammunition = MaxAmmunition;
        weaponState = WeaponState.Ready;
        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, SetAmmoValue());
    }

    private string SetAmmoValue()
    {
        return $"Ammo: {ammunition}/{MaxAmmunition}";
    }
}