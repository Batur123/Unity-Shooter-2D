using System.Collections;
using UnityEngine;

public class CharacterShooting : MonoBehaviour {
    private enum WeaponState {
        RELOADING,
        READY,
    }
   
    public GameObject projectilePrefab;
    private WeaponState _weaponState = WeaponState.READY;
    private static int _maxAmmunition = 10;
    private static int _ammunition = 10;

    private void Start() {
        UIController.instance.CreateAmmoText();
        UIController.instance.SetAmmunitionText(_ammunition, _maxAmmunition);
    }

    void Update() {
        if (_weaponState == WeaponState.RELOADING) {
            Debug.Log("Weapon is reloading, you cannot use weapon.");
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (_ammunition <= 0) {
                UIController.instance.SetAmmunitionText("Reloading");
                _weaponState = WeaponState.RELOADING;
                StartCoroutine(ReloadAfterDelay(2f));
            }
            else {
                ShootProjectile();
                UIController.instance.SetAmmunitionText(_ammunition, _maxAmmunition);
            }
        }
    }

    IEnumerator ReloadAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        _ammunition = 10;
        _weaponState = WeaponState.READY;
        UIController.instance.SetAmmunitionText(_ammunition, _maxAmmunition);
    }

    void ShootProjectile() {
        Vector2 targetPosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 shootDirection = targetPosition - (Vector2)transform.position;

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
