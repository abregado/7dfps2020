using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAtMidpoint : MonoBehaviour
{
    public Transform firstPoint;
    public Transform secondPoint;
    private Vector3 _lastPosition;

    void Start() {
        _lastPosition = transform.position;
    }
    
    // Update is called once per frame
    void Update() {
        var direction = _lastPosition + transform.position;
        transform.SetPositionAndRotation(firstPoint.position + (secondPoint.position - firstPoint.position) /2, Quaternion.Euler(direction));    
    }
}
