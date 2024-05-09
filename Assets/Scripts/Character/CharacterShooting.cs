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

    private IEnumerator _reloadRoutine;
    private Dictionary<IWeapon, Coroutine> _reloadRoutines = new Dictionary<IWeapon, Coroutine>();

    private static Camera _mainCamera;
    public GameObject projectilePrefab;

    private WeaponState _weaponState = WeaponState.READY;
    private UpgradeState _upgradeState = UpgradeState.NOT_UPGRADED;

    private static int _maxAmmunition = 10;
    private static int _ammunition = 10;

    public float nextShootTime;
    public float projectileSpeed = 50f;

    public int damageAmount = 5;

    private void Start() {
        _mainCamera = Camera.main;
        _weapons = new Dictionary<KeyCode, IWeapon> {
            { KeyCode.Alpha1, new Pistol() },
            { KeyCode.Alpha2, new Shotgun() },
        };
        _currentWeapon = _weapons[KeyCode.Alpha1]; // default weapon

        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT,
            $"[{_currentWeapon.Name}] - Ammo: {_currentWeapon.Ammunition}");
    }

    void Update() {
        HandleRotation();

        foreach (var weapon in _weapons) {
            if (Input.GetKeyDown(weapon.Key) && _currentWeapon != weapon.Value) {
                if (_currentWeapon.IsReloading && _reloadRoutines.ContainsKey(_currentWeapon)) {
                    StopCoroutine(_reloadRoutines[_currentWeapon]);
                    _reloadRoutines.Remove(_currentWeapon);
                    _currentWeapon.IsReloading = false;
                }

                _currentWeapon = weapon.Value;
                UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT,
                    $"[{_currentWeapon.Name}] - Ammo: {_currentWeapon.Ammunition}");
            }
        }

        if (CanReload(_currentWeapon)) {
            _reloadRoutines[_currentWeapon] = StartCoroutine(Reload(_currentWeapon));
        }

        if (Input.GetMouseButton(0) && Time.time > nextShootTime && !_currentWeapon.IsReloading) {
            nextShootTime = Time.time + _currentWeapon.ShootDelay;
            Vector2 shootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            _currentWeapon.Shoot(shootDirection, transform.position, projectilePrefab, projectileSpeed);

            if (_currentWeapon.Ammunition <= 0 && !_currentWeapon.IsReloading) {
                _reloadRoutines[_currentWeapon] = StartCoroutine(Reload(_currentWeapon));
            }
        }
    }

    bool CanReload(IWeapon weapon) {
        if (_currentWeapon == null) {
            return false;
        }

        return Input.GetKey(KeyCode.R) && !weapon.IsReloading && weapon.Ammunition >= 0 && weapon.Ammunition != weapon
            .MaxAmmunition;
    }

    void HandleRotation() {
        Vector2 weaponPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angleRad = Mathf.Atan2(mousePosition.y - weaponPosition.y, mousePosition.x - weaponPosition.x);
        float angleDeg = (180 / Mathf.PI) * angleRad;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    IEnumerator Reload(IWeapon weapon) {
        weapon.IsReloading = true;
        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT,
            $"[{_currentWeapon.Name}] - Reloading");
        yield return new WaitForSeconds(weapon.ReloadTime);
        weapon.Ammunition = weapon.MaxAmmunition;
        weapon.IsReloading = false;
        UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT,
            $"[{_currentWeapon.Name}] - Ammo: {weapon.Ammunition}");
        if (_reloadRoutines.ContainsKey(weapon)) {
            _reloadRoutines.Remove(weapon);
        }
    }
}
