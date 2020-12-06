using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardCharacterController : BasePlayerCharacterController
{
    public GameObject projectilePrefab;

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
        GameObject.Instantiate(projectilePrefab, characterCamera.transform.position, characterCamera.transform.rotation);
        Debug.Log("Pew pew!");
    }
}
