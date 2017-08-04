using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour {

    //Components
    public Transform target;

    //Zoom Variables
    private float currentZoom;
    public static float defaultZoom = 5f;
    public static float revengeZoom = 1f;

    //Revenge Variables
    public GameObject cutsceneBars;
    public Text revengeTextName;

    //Smooth Variables
    public float smoothSpeed = 0.125f;
    public Vector3 offest; 
	
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        defaultZoom = Camera.main.orthographicSize;
        currentZoom = defaultZoom;
    }

    void smoothZoom()
    {
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, revengeZoom, 0.1f);
    }

    public void resetCamera(Transform target)
    {
        currentZoom = defaultZoom;
        Camera.main.orthographicSize = defaultZoom;
        this.target = target;

        cutsceneBars.SetActive(false);
    }

    public void setZoom(float zoom, Transform newTarget)
    {
        currentZoom = zoom;
        target = newTarget;

        cutsceneBars.SetActive(true);
        revengeTextName.text = newTarget.GetComponent<C>().firstName + " " + newTarget.GetComponent<C>().lastName;
    }

    //Always match the update of the camera to the update of the player (possibly lateupdate to update)
    void FixedUpdate ()
    {
        if (currentZoom != Camera.main.orthographicSize)
            smoothZoom();

        if (target != null)
        {
            Vector3 desiredPos = target.position + offest;
            Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);

            transform.position = smoothPos;
        }
    }
}
