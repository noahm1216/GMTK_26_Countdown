using UnityEngine;

public class Troop : MonoBehaviour
{

    // Create 2nd troop type for upgrade, create tower manager script to perform upgrades

    Transform turretTransform;
    
    float health = 15f;
    float range = 10f;
    public int cost = 5;
    public GameObject bulletPrefab;

    float fireCooldown = 0.5f;
    float fireCooldownLeft = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turretTransform = this.transform.Find("Mage");
    }

    // Update is called once per frame
    void Update()
    {
        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>();

        Enemy nearestEnemy = null;
        float distance = Mathf.Infinity;

        // Find location of closest enemy
        foreach (Enemy e in enemies) {
            float d = Vector3.Distance(this.transform.position, e.transform.position);
            if (nearestEnemy == null || d < distance)
            {
                nearestEnemy = e;
                distance = d;
            }
        }

        if (nearestEnemy == null)
        {
            Debug.Log("No more enemies");
            return;
        }
        
        Vector3 direction = nearestEnemy.transform.position - this.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turretTransform.rotation = Quaternion.Euler(90, lookRotation.eulerAngles.y, 0);

        fireCooldownLeft -= Time.deltaTime;
        if (fireCooldownLeft <= 0 && direction.magnitude <= range)
        {
            // Fire at the enemy
            fireCooldownLeft = fireCooldown;
            FireAtEnemy(nearestEnemy);
        }

    }

    void FireAtEnemy(Enemy e)
    {
        GameObject bulletGameObj = (GameObject)Instantiate(bulletPrefab, turretTransform.position, turretTransform.rotation);
        Bullet b = bulletGameObj.GetComponent<Bullet>();
        b.target = e.transform;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
