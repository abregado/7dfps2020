using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardCharacterController : BasePlayerCharacterController
{
    public GameObject projectilePrefab;
    public Image manaBarImage;

    private int gravityPausedTicks = 0;
    private float blinkDistance = 10;
    private int hoverTime = (int)(50 * 0.2);

    private static float maxMana = 100;
    private static float blinkManaCost = 20;
    private static float blinkHoverDrain = 5 / 50.0f;
    private static float manaRegenPerTick = 50 / 50.0f;
    private static float attackManaCost = 10;
    private static int pauseRegenForTicks = (int)(50 * 1f);
    
    private float currentMana = maxMana;
    private int manaRegenPausedTicks = 0;

    bool spendMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            manaRegenPausedTicks = pauseRegenForTicks;
            return true;
        }

        return false;
    }

    protected override void Start()
    {
        maxHearts = 3;
        hearts = maxHearts;

        base.Start();
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(1) && spendMana(blinkManaCost))
        {
            Vector3 movementVector = characterCamera.transform.forward * blinkDistance;
            lastCollisionFlags = characterController.Move(movementVector);

            gravityPausedTicks = hoverTime;
            movementActive = false;
        }

        if (gravityPausedTicks > 0 && characterController.isGrounded)
        {
            gravityPausedTicks = 0;
            movementActive = true;
        }

        manaBarImage.fillAmount = currentMana / maxMana;

        canAttack = currentMana > attackManaCost;
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && gravityPausedTicks != 0 && spendMana(blinkHoverDrain))
            gravityPausedTicks = hoverTime;

        if (manaRegenPausedTicks > 0)
            manaRegenPausedTicks--;
        else
            currentMana = Math.Min(currentMana + manaRegenPerTick, maxMana);


        if (gravityPausedTicks == 0)
            movementActive = true;
        else
            gravityPausedTicks--;

        base.FixedUpdate();
    }

    protected override void DoAttack()
    {
        if (spendMana(attackManaCost))
            GameObject.Instantiate(projectilePrefab, characterCamera.transform.position, characterCamera.transform.rotation);
    }
}
