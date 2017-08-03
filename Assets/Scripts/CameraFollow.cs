using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //Components
    public Transform target;

    //Smooth Variables
    public float smoothSpeed = 0.125f;
    public Vector3 offest; 
	
    private Vector3 velocity = Vector3.zero;

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    //Always match the update of the camera to the update of the player (possibly lateupdate to update)
    void FixedUpdate ()
    {
        if (target != null)
        {
            Vector3 desiredPos = target.position + offest;
            Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);

            transform.position = smoothPos;
        }
    }
}
