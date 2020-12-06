using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardCharacterController : BasePlayerCharacterController
{
    public GameObject projectilePrefab;

    private int gravityPausedTicks = 0;
    private float blinkDistance = 10;
    private int hoverTime = (int)(50 * 0.2);

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 movementVector = characterCamera.transform.forward * blinkDistance;
            lastCollisionFlags = characterController.Move(movementVector);

            gravityPausedTicks = hoverTime;
            movementActive = false;
        }
        else if (Input.GetMouseButton(1))
        {
            gravityPausedTicks = hoverTime;
        }


        if (gravityPausedTicks > 0 && characterController.isGrounded)
        {
            gravityPausedTicks = 0;
            movementActive = true;
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
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
