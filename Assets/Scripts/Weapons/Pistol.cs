using UnityEngine;

public class Pistol : Weapon
{
    protected override float ProjectileSpeed => 50f;
    protected override int MaxAmmunition => 20;
    
    protected override void ShootProjectile()
    {
        var shootDirection = (Vector2)MainCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb) {
            projectileRb.AddForce(shootDirection.normalized * ProjectileSpeed, ForceMode2D.Impulse);
            ammunition -= 1;
        }
    }
}