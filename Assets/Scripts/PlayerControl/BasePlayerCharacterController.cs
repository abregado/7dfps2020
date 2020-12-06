using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerCharacterController : FirstPersonController
{
    protected Camera overlayCamera;
    protected GameObject weapon;
    protected BoxCollider meleeHitbox;

    protected int attackFrame = 0;
    protected bool attacking = false;

    protected bool canAttack = true;

    protected override void Start()
    {
        overlayCamera = transform.Find("FirstPersonCharacter").Find("OverlayCamera").gameObject.GetComponent<Camera>();
        weapon = overlayCamera.transform.GetChild(0).GetChild(0).gameObject;
        meleeHitbox = transform.Find("FirstPersonCharacter").Find("MeleeHitbox").gameObject.GetComponent<BoxCollider>();
        base.Start();
    }

    protected override void Update()
    {
        if (canAttack && Input.GetMouseButtonDown(0))
        {
            attacking = true;
            attackFrame = 0;
        }
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (attacking)
        {
            Vector3 position = weapon.transform.localPosition;
            attackFrame++;

            int attackLength = 50 / 8;

            float scale = attackFrame;
            if (attackFrame > attackLength)
                scale = attackLength - (attackFrame - attackLength);

            scale *= 2;

            position.y = -0.01f * scale; // down
            position.x = -0.04f * scale; // left
            position.z = 0.01f * scale; // forward

            weapon.transform.localPosition = position;

            if (attackFrame == attackLength)
                DoAttack();
            else if (attackFrame == attackLength * 2)
                attacking = false;
        }

        base.FixedUpdate();
    }

    protected abstract void DoAttack();

    public void DealDamage(int damage)
    {
        Debug.Log("Oww! Received " + damage + " damage");
    }
}
