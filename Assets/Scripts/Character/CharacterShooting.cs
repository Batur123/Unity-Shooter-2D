using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour {
    private enum WeaponState {
        RELOADING,
        READY,
    }

    private enum UpgradeState {
        NOT_UPGRADED,
        UPGRADED,
    }
    
    private IWeapon _currentWeapon;
    private Dictionary<KeyCode, IWeapon> _weapons;
    
    private static Camera _mainCamera;
    public GameObject projectilePrefab;
    
    private WeaponState _weaponState = WeaponState.READY;
    private UpgradeState _upgradeState = UpgradeState.NOT_UPGRADED;
    
    private static int _maxAmmunition = 10;
    private static int _ammunition = 10;
    
    public float fireRate = 0.1f;
    public float nextShootTime = 0f;
    public float projectileSpeed = 50f;
    
    public int damageAmount = 5;

    private void Start() {
        _mainCamera = Camera.main;
        
        _weapons = new Dictionary<KeyCode, IWeapon> {
            {KeyCode.Alpha1, new Pistol()},
            {KeyCode.Alpha2, new Shotgun()},
        };
        _currentWeapon = _weapons[KeyCode.Alpha1]; // default weapon
        
        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, 
            $"Ammo: {_currentWeapon.Ammunition}");
    }

    void Update() {
        HandleRotation();

        foreach (var weapon in _weapons)
        {
            if (Input.GetKeyDown(weapon.Key))
            {
                _currentWeapon = weapon.Value;
                UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, 
                    $"Ammo: {_currentWeapon.Ammunition}");
            }
        }
        
        if (Input.GetMouseButton(0) && Time.time > nextShootTime && !_currentWeapon.IsReloading)
        {
            Debug.Log("Ammunition: " + _currentWeapon.Ammunition);
            Debug.Log("IsReloading: " + _currentWeapon.IsReloading);

            
            nextShootTime = Time.time + fireRate;
            Vector2 shootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            _currentWeapon.Shoot(shootDirection, transform.position, projectilePrefab, projectileSpeed);
            // If out of ammo, start reloading. Make sure the reload coroutine runs only once
            if (_currentWeapon.Ammunition <= 0 && !_currentWeapon.IsReloading)
            { Debug.Log("Not Reloading. Starting Reload Coroutine.");
                StartCoroutine(Reload(_currentWeapon)); // removed 2f reloadTime, as it's inside weapon now
            }
            else {
                Debug.Log("Already Reloading");
            }

            // Update the ammunition count for the current weapon
            UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, 
                $"Ammo: {_currentWeapon.Ammunition}");
        }
    }

    void HandleRotation() {
        Vector2 weaponPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angleRad = Mathf.Atan2(mousePosition.y - weaponPosition.y, mousePosition.x - weaponPosition.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    IEnumerator Reload(IWeapon weapon)
    {
        Debug.Log($"Reloading {weapon.Name}");
        weapon.IsReloading = true;
        yield return new WaitForSeconds(weapon.ReloadTime);
        Debug.Log($"Reloaded {weapon.Name}");
    
        weapon.Ammunition = 10; // Restock ammo
        weapon.IsReloading = false;
    }
}
