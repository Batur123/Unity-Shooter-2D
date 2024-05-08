using UnityEngine;

public interface IWeapon 
{
    string Name { get; }
    int Ammunition { get; set; }
    bool IsReloading { get; set; } 
    float ReloadTime { get; }  // Add this line
    void Shoot(Vector2 shootDirection, Vector3 position, GameObject projectilePrefab, float projectileSpeed);
}

public class Pistol : IWeapon {
    public string Name => "Pistol";
    public int Ammunition { get; set; } = 10;
    public bool IsReloading { get; set; } = false;
    public float ReloadTime => 2f; //example reload time

    public void Shoot(Vector2 shootDirection, Vector3 position, GameObject projectilePrefab, float projectileSpeed) {
        if (IsReloading) {
            return;
        }

        if (Ammunition <= 0) {
            IsReloading = true;
        }

        if (Ammunition > 0 && !IsReloading) {
            GameObject projectile = Object.Instantiate(projectilePrefab, position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            if (projectileRb != null) {
                projectileRb.AddForce(shootDirection.normalized * projectileSpeed, ForceMode2D.Impulse);
                Ammunition -= 1;
            }
        }
    }
}

public class Shotgun : IWeapon
{
    public string Name => "Shotgun";
    public int Ammunition { get; set; } = 5;
    public bool IsReloading { get; set; } = false; 
    public float ReloadTime => 2f; //example reload time


    public void Shoot(Vector2 shootDirection, Vector3 position, GameObject projectilePrefab, float projectileSpeed)
    {
        // Implement shotgun shooting mechanics here
    }
}