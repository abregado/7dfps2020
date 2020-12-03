using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToRayHit : MonoBehaviour
{
    
    public Camera cam;
    public int mouseButtonIndex = 0;

    void Update() {
        if (Input.GetMouseButtonDown(mouseButtonIndex)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                transform.SetPositionAndRotation(hit.point, Quaternion.identity);
            }

        }
    }
}
