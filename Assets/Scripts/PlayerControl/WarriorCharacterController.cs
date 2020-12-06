using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCharacterController : BasePlayerCharacterController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAttack()
    {
        bool success = false;
        foreach(Collider hit in meleeHitbox.GetComponent<QueryableTrigger>().getOverlaps())
        {
            if (hit.GetComponent<EnemyAI>())
            {
                hit.GetComponent<EnemyAI>().DealDamage(1);
                success = true;
            }
        }

        Debug.Log("HIT: " + success);
    }
}
