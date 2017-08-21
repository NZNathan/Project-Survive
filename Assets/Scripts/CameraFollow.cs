using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{
    //Components
    public Transform target;
    public static Camera cam;

    //Raycast Variables
    private LayerMask blockMask; //Layer that all attackable objects are on
    public static float ObjectFade = 0.5f;
    private GameObject fadedObject;

    //Zoom Variables
    private bool zooming = false;
    private float currentZoom;
    public static float defaultZoom = 5f;
    public static float revengeZoom = 1f;

    //Revenge Variables  - Extract out into different class?
    [Header("Revenge Variables")]
    public GameObject cutsceneBars;
    public Text revengeTextName;

    //Smooth Variables
    public float smoothSpeed = 0.125f;
    public Vector3 offest;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        blockMask = LayerMask.GetMask("Sprite");
        defaultZoom = cam.orthographicSize;
        currentZoom = defaultZoom;

        InvokeRepeating("checkPlayerObscured", 1, 0.1f);
    }

    void checkPlayerObscured()
    {
        if (Player.instance != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Player.instance.transform.position, cam.transform.forward, 500, blockMask);
            if (hit.collider != null)
            {
                GameObject objectHit = hit.transform.gameObject;
                if (objectHit != fadedObject)
                {

                    if (objectHit.GetComponent<SpriteRenderer>().sortingOrder > Player.instance.GetComponentInChildren<SpriteRenderer>().sortingOrder)
                        objectHit.GetComponent<SpriteObject>().setTransparency(ObjectFade);

                    if (fadedObject != null)
                        fadedObject.GetComponent<SpriteObject>().setTransparency(1);

                    fadedObject = objectHit;
                }
                return;
            }

            if (fadedObject != null)
            {
                fadedObject.GetComponent<SpriteObject>().setTransparency(1);
                fadedObject = null;
            }
        }
    }

        void smoothZoom()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, revengeZoom, (4f / Time.timeScale) * Time.deltaTime);
    }

    public void resetCamera(Transform target)
    {
        zooming = false;
        currentZoom = defaultZoom;
        cam.orthographicSize = defaultZoom;
        this.target = target;

        transform.position = target.position + offest;

        cutsceneBars.SetActive(false);
    }

    public void setZoom(float zoom, Transform newTarget)
    {
        zooming = true;
        currentZoom = zoom;
        target = newTarget;

        cutsceneBars.SetActive(true);
        revengeTextName.text = newTarget.GetComponent<C>().firstName + " " + newTarget.GetComponent<C>().lastName;
    }

    //Always match the update of the camera to the update of the player (possibly lateupdate to update)
    void FixedUpdate()
    {
        if (currentZoom != cam.orthographicSize && zooming)
            smoothZoom();

        if (target != null)
        {
            Vector3 desiredPos = target.position + offest;
            Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);

            transform.position = smoothPos;
        }
    }
}
