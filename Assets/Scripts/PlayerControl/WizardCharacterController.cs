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
    private static float blinkManaCost = 20;
    private static float blinkHoverDrain = 5 / 50.0f;


    private static float maxMana = 100;
    private static float manaRegenPerTick = 50 / 50.0f;
    private float currentMana = maxMana;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(1) && currentMana >= blinkManaCost)
        {
            Vector3 movementVector = characterCamera.transform.forward * blinkDistance;
            lastCollisionFlags = characterController.Move(movementVector);

            gravityPausedTicks = hoverTime;
            movementActive = false;

            currentMana -= blinkManaCost;
        }

        if (gravityPausedTicks > 0 && characterController.isGrounded)
        {
            gravityPausedTicks = 0;
            movementActive = true;
        }

        manaBarImage.fillAmount = currentMana / maxMana;

        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && currentMana > 0 && gravityPausedTicks != 0)
        {
            gravityPausedTicks = hoverTime;
            currentMana = Math.Max(currentMana - blinkHoverDrain, 0);
        }
        else if (gravityPausedTicks == 0)
        {
            currentMana = Math.Min(currentMana + manaRegenPerTick, maxMana);
        }

        if (gravityPausedTicks == 0)
            movementActive = true;
        else
            gravityPausedTicks--;

        base.FixedUpdate();
    }

    protected override void DoAttack()
    {
        GameObject.Instantiate(projectilePrefab, characterCamera.transform.position, characterCamera.transform.rotation);
        Debug.Log("Pew pew!");
    }
}
