using UnityEngine;

public class EnemyBullet : Bullet
{
    
    protected void Start()
    {
        targetName = "troop";
    }

    protected override void Update()
    {
        base.Update();
    }


}
