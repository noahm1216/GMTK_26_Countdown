using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    float spawnCooldown = 1f;
    float spawnCooldownLeft = 0;

    public GameObject timer;

    [System.Serializable]
    public class WaveComponent
    {
        public GameObject enemyPrefab;
        public int count;
        // Omit from inspector
        public float spawned = 0;
    }

    public WaveComponent[] waveComps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Check to see if timer is active, if active spawn enemies

        if (timer.GetComponent<TD_TimerManager>().isTimerActive)
        {
            spawnCooldownLeft -= Time.deltaTime;
            if (spawnCooldownLeft < 0)
            {
                spawnCooldownLeft = spawnCooldown;

                bool didSpawn = false;
                foreach (WaveComponent wc in waveComps)
                {
                    if (wc.spawned < wc.count)
                    {
                        // Spawn an enemy

                        Instantiate(wc.enemyPrefab, this.transform.position, this.transform.rotation);
                        wc.spawned++;

                        didSpawn = true;
                        break;
                    }
                }

                if (!didSpawn)
                {
                    // Wave completed, instantiate next wave obj
                    if (transform.parent.childCount > 1)
                    {
                        transform.parent.GetChild(1).gameObject.SetActive(true);
                        transform.SetParent(null);
                    }
                    else
                    {
                        // No more waves, game completed
                    }

                    Destroy(this.gameObject);
                }
            }
        }


    }
}
