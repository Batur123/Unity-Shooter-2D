using UnityEngine;

public class Shotgun : Weapon
{
    protected override float ProjectileSpeed => 30f;
    protected override int MaxAmmunition => 10;

    protected override void ShootProjectile()
    {
        var bulletsPerShot = 3;
        var spreadAngle = 2.5f;
        var shootDirection = (Vector2)MainCamera.ScreenToWorldPoint(Input.mousePosition) -
                             (Vector2)transform.position;
        
        for (int i = 0; i < bulletsPerShot; i++) {
            var bulletSpread =
                Quaternion.Euler(0, 0, spreadAngle * (i - (bulletsPerShot - 1) / 2f));
        
            GameObject projectile =
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        
            if (projectileRb) {
                projectileRb.AddForce(
                    bulletSpread * shootDirection.normalized * 30f,
                    ForceMode2D.Impulse);
            }
        }
        
        ammunition -= bulletsPerShot;
    }
}