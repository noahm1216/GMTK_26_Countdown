using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Create 2nd enemy variation and develop varied wave comp

    GameObject pathObj;

    Transform targetPathNode;
    int pathNodeIndex = 0;

    [SerializeField] protected float speed = 3f;   
    [SerializeField] protected float health = 1f;
    [SerializeField] protected int scoreValue = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        pathObj = GameObject.Find("Path");
    }

    protected void GetNextPathNode()
    {
        targetPathNode = pathObj.transform.GetChild(pathNodeIndex);
        pathNodeIndex++;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(targetPathNode == null)
        {
            GetNextPathNode();
            if(targetPathNode == null)
            {
                // Ran out of path
                EndPath();
            }
        }

        // Move towards the target path node
        Vector3 direction = targetPathNode.position - this.transform.localPosition;

        float distanceThisFrame = speed * Time.deltaTime;
    
        if(direction.magnitude <= distanceThisFrame)
        {
            // Reached next node
            targetPathNode = null;
        }
        else
        {
            // Move towards the node
            transform.Translate(direction.normalized * distanceThisFrame, Space.World);
            // Add rotation for smooth turning between nodes
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, speed * Time.deltaTime*5);
        }
    }

    public void EndPath()
    {
        GameObject.FindObjectsByType<TD_ScoreManager>()[0].LoseLife();
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameObject.FindObjectsByType<TD_ScoreManager>()[0].score += scoreValue;
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
