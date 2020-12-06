using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour {
    public float speed = 50f;
    public float fireRate = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0) {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }    
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<BasePlayerCharacterController>())
            other.gameObject.GetComponent<BasePlayerCharacterController>().DealDamage(1);
        else if (other.gameObject.GetComponent<EnemyAI>())
            other.gameObject.GetComponent<EnemyAI>().DealDamage(1);

        speed = 0;
        Debug.Log("hit!");
        Destroy(gameObject);
    }

}
