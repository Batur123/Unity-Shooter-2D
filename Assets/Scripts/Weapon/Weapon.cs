using UnityEngine;

public interface IWeapon 
{
    string Name { get; }
    int Ammunition { get; set; }
    int MaxAmmunition { get; set; }
    bool IsReloading { get; set; } 
    float ReloadTime { get; }
    float ShootDelay { get; }
    void Shoot(Vector2 shootDirection, Vector3 position, GameObject projectilePrefab, float projectileSpeed);
}

public class Pistol : IWeapon {
    public string Name => "Pistol";
    public int Ammunition { get; set; } = 10;
    public int MaxAmmunition { get; set; } = 10;
    public bool IsReloading { get; set; } = false;
    public float ReloadTime => 2f;
    public float ShootDelay => 0.5f;

    public void Shoot(Vector2 shootDirection, Vector3 position, GameObject projectilePrefab, float projectileSpeed) {
        if (IsReloading) {
            return;
        }
        
        if (Ammunition > 0 && !IsReloading) {
            Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg);
            GameObject projectile = Object.Instantiate(projectilePrefab, position, rotation);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null) {
                projectileRb.AddForce(shootDirection.normalized * projectileSpeed, ForceMode2D.Impulse);
                Ammunition -= 1;
                UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT,
                    $"[{Name}] - Ammo: {Ammunition}");
            }
        }
    }
}

public class Shotgun : IWeapon
{
    public string Name => "Shotgun";
    public int Ammunition { get; set; } = 5;
    public int MaxAmmunition { get; set; } = 5;
    public bool IsReloading { get; set; } = false; 
    public float ReloadTime => 2f;
    public float ShootDelay => 1f;


    public void Shoot(Vector2 shootDirection, Vector3 position, GameObject projectilePrefab, float projectileSpeed)
    {
        if (IsReloading) {
            return;
        }
        
        if (Ammunition > 0 && !IsReloading) {
            var spreadAngle = 20;
            var pellets = 3;

            for (var i = 0; i < pellets; i++) {
                float spread = (spreadAngle * (-0.5f + (i / (float)(pellets - 1)))); // Calculates spread angle
                var projectileDirection = Quaternion.Euler(0, 0, spread) * shootDirection;
                Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg + spread);
                GameObject projectile = Object.Instantiate(projectilePrefab, position, rotation);

                Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
                if (projectileRb != null) {
                    projectileRb.AddForce(projectileDirection.normalized * projectileSpeed, ForceMode2D.Impulse);
                }
            }

            Ammunition -= 1;
            UIController.Instance.SetTextValue(UIController.TextType.AMMO_TEXT,
                $"[{Name}] - Ammo: {Ammunition}");
        }
    }
}