using UnityEngine;

public class EliteEnemy : Enemy
{

    Transform eliteEnemyTransform;
    public GameObject bulletPrefab;

    float range = 10f;
    float fireCooldown = 1f;
    float fireCooldownLeft = 0;

    // TO FIX: when elite enemy reaches last path node it is throwing errors

    protected override void Start()
    {
        speed = 5f;
        health = 5f;
        scoreValue = 25;
        eliteEnemyTransform = this.transform;

        base.Start();
    }

    protected override void Update()
    {
        base.Update(); 
        ExecuteAdditionalChildLogic();
    }

    private void ExecuteAdditionalChildLogic()
    {
        
        Troop[] troops = GameObject.FindObjectsByType<Troop>();

        Troop nearestTroop = null;
        float distance = Mathf.Infinity;

        // Find location of closest troop
        foreach (Troop t in troops) {
            float d = Vector3.Distance(this.transform.position, t.transform.position);
            if (nearestTroop == null || d < distance)
            {
                nearestTroop = t;
                distance = d;
            }
        }

        if (nearestTroop == null)
        {
            Debug.Log("No more troops");
            return;
        }
        
        Vector3 direction = nearestTroop.transform.position - this.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        eliteEnemyTransform.rotation = Quaternion.Euler(90, lookRotation.eulerAngles.y, 0);

        fireCooldownLeft -= Time.deltaTime;
        if (fireCooldownLeft <= 0 && direction.magnitude <= range)
        {
            // Fire at the troop
            fireCooldownLeft = fireCooldown;
            FireAtTroop(nearestTroop);
        }
    }

    void FireAtTroop(Troop e)
    {
        GameObject bulletGameObj = (GameObject)Instantiate(bulletPrefab, eliteEnemyTransform.position, eliteEnemyTransform.rotation);
        Bullet b = bulletGameObj.GetComponent<Bullet>();
        b.target = e.transform;
    }

}
