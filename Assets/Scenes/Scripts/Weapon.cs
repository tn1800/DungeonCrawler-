using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField] public UnityEvent<Projectile> ProjectileShootEvent;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] public float ShootingSpeed = 5;
    [SerializeField] public float ShootingCooldown = 1;
    [SerializeField] public float Damage = 1;
    [SerializeField] public float ProjectileLifetime = 5;
    [SerializeField] public float ProjectileScale = 1;

    float lastShotTime;

    // Start is called before the first frame update
    void Start()
    {
    
        lastShotTime = -ShootingCooldown - 1;
    }

    // shooting from enemy and player 
    public void Shoot(Vector2 direction)
    {
        
        if (Time.time - lastShotTime > ShootingCooldown)
        {

            // Load Projectile. 
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.localScale *= ProjectileScale;

            // Set life/damage. 
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.setLifetimeAndDamage(ProjectileLifetime, Damage);
            projectileScript.SourceTag = gameObject.tag;

            //add projectile velocity 
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y, 0) * ShootingSpeed;

            
            lastShotTime = Time.time;

            ProjectileShootEvent.Invoke(projectileScript);
        }
    }

}
