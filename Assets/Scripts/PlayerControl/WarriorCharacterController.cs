﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorCharacterController : BasePlayerCharacterController
{
    public Image rageBarImage;

    private static float maxRage = 100;
    private static float chargeCost = 25;
    private static float rageRegenPerTick = (1 / 50f) / 10f * chargeCost;
    private static float hitRageRecharge = chargeCost / 2;
    private float currentRage = chargeCost;

    bool spendRage(float amount)
    {
        if (currentRage >= amount)
        {
            currentRage -= amount;
            return true;
        }

        return false;
    }

    enum ChargeState
    {
        Charging,
        SwingAtEnd,
        NotCharging,
    }

    private ChargeState chargeState = ChargeState.NotCharging;
    private float chargeSpeed = 50;

    protected override void Start()
    {
        maxHearts = 6;
        hearts = maxHearts;

        base.Start();
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(1) && spendRage(chargeCost))
        {
            chargeState = ChargeState.Charging;
            movementActive = false;
            canAttack = false;

            attacking = false;
            attackFrame = 0;
        }

        if (currentRage < chargeCost)
            rageBarImage.color = new Color(1, 1, 1, 0.5f);
        else
            rageBarImage.color = Color.white;

        rageBarImage.fillAmount = currentRage / maxRage;

        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (chargeState == ChargeState.Charging)
        {
            Vector3 movementVector = characterCamera.transform.forward;
            movementVector.Normalize();
            movementVector *= chargeSpeed;

            movementVector.y = -chargeSpeed / 2.0f;

            lastCollisionFlags = characterController.Move(movementVector * Time.fixedDeltaTime);

            Vector3 weaponPosition = weapon.transform.localPosition;
            Vector3 targetPosition = new Vector3(-0.5f, 0.42f, 0f);

            float maxMove = 0.2f;

            Func<float, float, float> moveComponent = (float from, float to) =>
            {
                if (Math.Abs(from - to) < maxMove)
                    from = to;
                else if (from > to)
                    from -= maxMove;
                else if (to > from)
                    from += maxMove;

                return from;
            };

            weaponPosition.x = moveComponent(weaponPosition.x, targetPosition.x);
            weaponPosition.y = moveComponent(weaponPosition.y, targetPosition.y);
            weaponPosition.z = moveComponent(weaponPosition.z, targetPosition.z);

            weapon.transform.localPosition = weaponPosition;
        }
        else if (chargeState == ChargeState.SwingAtEnd)
        {
            Vector3 position = weapon.transform.localPosition;
            attackFrame++;

            int attackLength = 50 / 2;

            position.y -= 1;

            weapon.transform.localPosition = position;

            if (attackFrame == attackLength)
            {
                weapon.transform.localPosition = new Vector3();
                chargeState = ChargeState.NotCharging;
                movementActive = true;
                canAttack = true;
            }
        }
        else if (chargeState == ChargeState.NotCharging && currentRage < chargeCost)
        {
            currentRage += rageRegenPerTick;
        }

        base.FixedUpdate();
    }

    override protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (chargeState == ChargeState.Charging)
        {
            if (hit.gameObject.layer != LayerMask.NameToLayer("Walkable"))
            {
                HashSet<Collider> allHits = new HashSet<Collider>(meleeHitbox.GetComponent<QueryableTrigger>().getOverlaps());
                allHits.Add(hit.collider);

                foreach (Collider otherHit in allHits)
                {
                    if (otherHit.GetComponent<EnemyAI>())
                    {
                        otherHit.GetComponent<EnemyAI>().DealDamage(3);
                        currentRage = Math.Min(currentRage + hitRageRecharge, maxRage);
                    }
                }

                chargeState = ChargeState.SwingAtEnd;
            }
        }

        base.OnControllerColliderHit(hit);
    }

    protected override void DoAttack()
    {
        bool success = false;
        foreach(Collider hit in meleeHitbox.GetComponent<QueryableTrigger>().getOverlaps())
        {
            if (hit.GetComponent<EnemyAI>())
            {
                hit.GetComponent<EnemyAI>().DealDamage(1);
                currentRage = Math.Min(currentRage + hitRageRecharge, maxRage);
                success = true;
            }
        }

        Debug.Log("HIT: " + success);
    }
}
