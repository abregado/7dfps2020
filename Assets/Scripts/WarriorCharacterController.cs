﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCharacterController : FirstPersonController
{
    Camera overlayCamera;
    GameObject weapon;
    BoxCollider meleeHitbox;

    int attackFrame = 0;
    bool attacking = false;

    protected override void Start()
    {
        overlayCamera = transform.Find("FirstPersonCharacter").Find("OverlayCamera").gameObject.GetComponent<Camera>();
        weapon = overlayCamera.transform.GetChild(0).GetChild(0).gameObject;
        meleeHitbox = transform.Find("FirstPersonCharacter").Find("MeleeHitbox").gameObject.GetComponent<BoxCollider>();
        base.Start();
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
            {
                bool success = false;
                foreach(Collider hit in meleeHitbox.GetComponent<QueryableTrigger>().getOverlaps())
                {
                    if (hit.GetComponent<EnemyAI>())
                    {
                        Object.Destroy(hit.gameObject);
                        success = true;
                    }
                }

                Debug.Log("HIT: " + success);
            }
            else if (attackFrame == attackLength * 2)
            {
                attacking = false;
            }
        }

        base.FixedUpdate();
    }
}
