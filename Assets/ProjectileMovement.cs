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
        speed = 0;
        
        Destroy(gameObject);
    }

}
