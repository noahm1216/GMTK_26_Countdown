using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 15f;
    public Transform target;
    protected string targetName = "enemy";

    public float damage = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (target == null)
        {
            // Enemy/troop destroyed, no more bullets
            Destroy(gameObject);
            return;
        }
        
        Vector3 direction = target.position - this.transform.localPosition;

        float distanceThisFrame = speed * Time.deltaTime;
    
        if(direction.magnitude <= distanceThisFrame)
        {
            // Reached target
            DoBulletHit(targetName);
        }
        else
        {
            // Move towards the target
            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
            // Add rotation to face target on hit
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, speed * Time.deltaTime*5);
        }
    }

    protected virtual void DoBulletHit(string targetName)
    {
        if (target != null)
        {
            switch (targetName)
            {
                case "enemy":
                    Enemy enemy = target.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                    break;

                case "troop":
                    Troop troop = target.GetComponent<Troop>();
                    if (troop != null)
                    {
                        troop.TakeDamage(damage);
                    }
                    break; 
            }
        }

        Destroy(gameObject);
    }
}
