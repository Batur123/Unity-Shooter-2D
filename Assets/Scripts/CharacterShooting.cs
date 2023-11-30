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
    
    private static Camera _mainCamera;
    public GameObject projectilePrefab;
    private WeaponState _weaponState = WeaponState.READY;
    private static int _maxAmmunition = 10;
    private static int _ammunition = 10;
    private UpgradeState _upgradeState = UpgradeState.NOT_UPGRADED;

    private void Start() {
        _mainCamera = Camera.main;
        UIController.Instance.SetAmmunitionText(_ammunition, _maxAmmunition);
    }

    void Update() {
        HandleInputs();
    }

    void HandleInputs() {
        if (Input.GetKey(KeyCode.R) && _weaponState != WeaponState.RELOADING &&
            _ammunition >= 0 && _ammunition != _maxAmmunition) {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.U) &&
            _upgradeState == UpgradeState.NOT_UPGRADED) {
            _upgradeState = UpgradeState.UPGRADED;
            Debug.Log("Ammunition upgraded.");
            _maxAmmunition = 20;
            UIController.Instance.SetAmmunitionText(_ammunition,
                _maxAmmunition);
        }

        if (_weaponState == WeaponState.RELOADING) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (_ammunition <= 0) {
                Reload();
            }
            else {
                ShootProjectile();
                UIController.Instance.SetAmmunitionText(_ammunition,
                    _maxAmmunition);
            }
        }
    }

    void Reload() {
        UIController.Instance.SetAmmunitionText("Reloading");
        _weaponState = WeaponState.RELOADING;
        StartCoroutine(ReloadAfterDelay(2f));
    }

    IEnumerator ReloadAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        _ammunition = _maxAmmunition;
        _weaponState = WeaponState.READY;
        UIController.Instance.SetAmmunitionText(_ammunition, _maxAmmunition);
    }

    void ShootProjectile() {
        var shootDirection = (Vector2)_mainCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;

        GameObject projectile = Instantiate(projectilePrefab,
            transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        if (projectileRb) {
            projectileRb.AddForce(shootDirection.normalized * 20f,
                ForceMode2D.Impulse);
        }

        _ammunition -= 1;
    }
}
