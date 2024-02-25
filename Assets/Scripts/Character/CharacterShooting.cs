using System.Collections;
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

    private enum EquippedWeapon {
        PISTOL,
        SHOTGUN,
    }
    private WeaponManager weaponManager;
    private Weapon currentWeapon;

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

    private EquippedWeapon _currentWeapon;

    private void Start() {
        weaponManager = GetComponent<WeaponManager>();
        _mainCamera = Camera.main;
        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, SetAmmoValue());
        _currentWeapon = EquippedWeapon.PISTOL;
        
        currentWeapon = GetComponent<Pistol>();
        
        UIController.Instance.SetTextValue(UIController.TextType.WEAPON_TEXT, "Equipped: Pistol");
    }

    void Update() {
        HandleRotation();
        HandleInputs();
    }

    private string SetAmmoValue() {
        return $"Ammo: {_ammunition}/{_maxAmmunition}";
    }

    void HandleRotation() {
        Vector2 weaponPosition = transform.position;
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float angleRad = Mathf.Atan2(mousePosition.y - weaponPosition.y, mousePosition.x - weaponPosition.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    bool IsReloading() {
        return _weaponState == WeaponState.RELOADING;
    }
    
    bool CanReload() {
        return Input.GetKey(KeyCode.R) && !IsReloading() && _ammunition >= 0 && _ammunition != _maxAmmunition;
    }

    bool CanUpgrade() {
        return Input.GetKeyDown(KeyCode.U) && !IsReloading() && _upgradeState == UpgradeState.NOT_UPGRADED;
    }
    
    
    void HandleInputs() {
        if (Input.GetKey(KeyCode.Alpha1) && _currentWeapon != EquippedWeapon.SHOTGUN) {
            _currentWeapon = EquippedWeapon.SHOTGUN;
            UIController.Instance.SetTextValue(UIController.TextType.WEAPON_TEXT, "Equipped: Shotgun");
        }
        
        if (CanReload()) {
            Reload();
        }

        if (CanUpgrade()) {
            _upgradeState = UpgradeState.UPGRADED;
            _maxAmmunition = 20;
            UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, SetAmmoValue());
        }

        if (IsReloading()) {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            weaponManager.FireCurrentWeapon();
        }
    }

    void Reload() {
        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, "Reloading");
        _weaponState = WeaponState.RELOADING;
        StartCoroutine(ReloadAfterDelay(2f));
    }

    IEnumerator ReloadAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        _ammunition = _maxAmmunition;
        _weaponState = WeaponState.READY;
        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT, SetAmmoValue());
    }
}
