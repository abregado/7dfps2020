using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using Random = UnityEngine.Random;

public class RangedEnemyAI : EnemyAI {
    public GameObject projectile;
    public GameObject launcher;
    protected override void AttackPlayer() {
        if (!_alreadyAttacked) {
            Vector3 direction = _player.position - launcher.transform.position;
            GameObject bullet = GameObject.Instantiate(projectile, launcher.transform.position,
                Quaternion.LookRotation(direction));
        }

        base.AttackPlayer();
    }
}
